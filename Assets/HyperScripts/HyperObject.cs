using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;

public class HyperObject : MonoBehaviour {

	public int w;                                   //point on the w axis
    public int w_depth;                             //how far along the w axis this object extends

    public float dullCoef = 1.0f;                   //how much to dull the color of the object by

    public bool isParent = false;                   //is this object the parent?
    //NOTE: Enable even if no children and if has both HyperObject and HyperColliderManager enable this as parent and not the other

    public bool vanishWhenTransparent = false;      //enable if alpha should be 0 at all times if not visible from the player's w point

    bool controllersReady = false;                  //give time before trying to get input from the controllers

    FourthDManager IVDManager;                      //the 4D manager

    SteamVR_ControllerManager controllerManager;    //The steam controller manager that holds the controller indices

	Renderer _cachedRenderer;						//The renderer for this object

	HyperCreature player;							//Reference to the player

    const int TRANSPARENT_QUEUE_ORDER = 3000;

    void Start()
    {
        //locate the 4Dmanager
        IVDManager = Object.FindObjectOfType<FourthDManager>();

        controllerManager = Object.FindObjectOfType<SteamVR_ControllerManager>();

		_cachedRenderer = GetComponent<Renderer>();

		player = Object.FindObjectOfType<HyperCreature>();

        //Invoke("GetReady", 5);

        //if this is the parent perform all the initialization for this object
        if (isParent)
        {
            setW(w);
            WMove();
        }
    }

    void GetReady()
    {
        controllersReady = true;
    }

    //have the objects detect the vive controller inputs to move themselves
    /*void LateUpdate()
    {
        if (controllersReady && isParent)
        {
            for (int i = 0; i < controllerManager.indices.Length; i++)
            {
                if (SteamVR_Controller.Input((int)controllerManager.indices[i]).GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger))
                {
                    WMove();
                }
            }
        }
    }*/

    //the player has moved to a new w point, remove once 4D shader is implemented
    public void WMove()
    {
		int newW = player.w;
        if (isVisibleSolid(newW))
        {//this object is on the player's w point or is wide enough to be seen and touched
            StartCoroutine(ColorTrans(newW, 1.0f));
        }
        else {
            float targA = .2f;
            if (vanishWhenTransparent)
                targA = 0.0f;

            //fade out if not on player's w point
            if (w_depth > 0)
            {
                if (w > newW)
                    StartCoroutine(ColorTrans(w, targA));
                else
                    StartCoroutine(ColorTrans(w + w_depth, targA));
            }
            else
            {
                if (w < newW)
                    StartCoroutine(ColorTrans(w, targA));
                else
                    StartCoroutine(ColorTrans(w + w_depth, targA));
            }
        }
        if (GetComponent<HyperColliderManager>())
            GetComponent<HyperColliderManager>().WMove(newW);
        else
        {
            recurseChildrenWMove(transform, newW);
        }

        //GetComponent<MeshRenderer>().material.SetFloat("_WPos", (float)w);
        //GetComponent<MeshRenderer>().material.renderQueue = TRANSPARENT_QUEUE_ORDER + getNewOrder(newW);
    }

    void recurseChildrenWMove(Transform t, int newW)
    {
        foreach (Transform child in t)
        {
            if (child.GetComponent<HyperObject>())
                child.GetComponent<HyperObject>().WMove();
            else if (child.GetComponent<HyperColliderManager>())
                child.GetComponent<HyperColliderManager>().WMove(newW);
            else if (child.childCount > 0)
                recurseChildrenWMove(child, newW);
        }
    }

    public void setW(int newW)
    {
        w = newW;
        if (GetComponent<HyperColliderManager>())
            GetComponent<HyperColliderManager>().setW(newW);
        else
        {
            recurseChildrenSetW(transform, newW);
        }
    }

    void recurseChildrenSetW(Transform t, int newW)
    {
        if (GetComponent<HyperColliderManager>())
            GetComponent<HyperColliderManager>().setW(newW);
        else
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<HyperObject>())
                    child.GetComponent<HyperObject>().setW(newW);
                else if (child.GetComponent<HyperColliderManager>())
                    child.GetComponent<HyperColliderManager>().setW(newW);
                else if (child.childCount > 0)
                    recurseChildrenSetW(child, newW);
            }
        }
    }

    int getNewOrder(int creatureW)
    {
        int controllerPos = creatureW;

        if (controllerPos == 0)
        {
            return 7 - w;
        }
        else if (controllerPos == 1)
        {
            if (w == 1)
                return 7;
            else if (w == 0)
                return 6;
            else
                return 7 - w;
        }
        else if (controllerPos == 2)
        {
            if (w == 2)
                return 7;
            else if (w == 1)
                return 6;
            else if (w == 3)
                return 5;
            else if (w == 0)
                return 4;
            else
                return 7 - w;
        }
        else if (controllerPos == 3)
        {
            if (w == 3)
                return 7;
            else if (w == 2)
                return 6;
            else if (w == 4)
                return 5;
            else if (w == 1)
                return 4;
            else if (w == 5)
                return 3;
            else if (w == 0)
                return 2;
            else
                return 1;
        }
        else if (controllerPos == 4)
        {
            if (w == 4)
                return 7;
            else if (w == 3)
                return 6;
            else if (w == 5)
                return 5;
            else if (w == 2)
                return 4;
            else if (w == 6)
                return 3;
            else
                return 1 + w;
        }
        else if (controllerPos == 5)
        {
            if (w == 5)
                return 7;
            else if (w == 4)
                return 6;
            else if (w == 6)
                return 5;
            else
                return 1 + w;
        }
        else if (controllerPos == 6)
        {
            return 1 + w;
        }

        return 0;
    }

    //smoothly change the color of this object, rmove once 4D shader is implemented
    IEnumerator ColorTrans(int newW, float targetA){
        Color targetColor;
        Color curColor;
		curColor = gameObject.GetComponent<Renderer>().material.color;
		
		//deturmine the target color based on w point
        
		if(newW == 0)
			targetColor = Color.red;
		else if(newW == 1)
			targetColor = new Color(1,.45f,0);
		else if(newW == 2)
			targetColor = Color.yellow;
		else if(newW == 3)
			targetColor = Color.green;
		else if(newW == 4)
			targetColor = Color.cyan;
		else if(newW == 5)
			targetColor = Color.blue;
		else
			targetColor = Color.magenta;
            

		targetColor.a = targetA;
		targetColor.r /= dullCoef;
		targetColor.g /= dullCoef;
		targetColor.b /= dullCoef;
		
		for(float i = 0.0f; i < 1.0f; i += .05f){

			gameObject.GetComponent<Renderer>().material.color = Color.Lerp(curColor, targetColor, i);
			if(transform.childCount > 0){
				foreach(Transform child in transform){
					child.gameObject.GetComponent<Renderer>().material.color = Color.Lerp(curColor, targetColor, i);
				}
			}

			yield return null;
		}
	}

    //move this object along the w axis by deltaW
	public bool SlideW(int deltaW){
        if ((deltaW > 0 && w != 6 && w + w_depth != 6) || (deltaW < 0 && w != 0 && w + w_depth != 0))
        {
            bool childrenClear = true;


            recurseChildrenSlideW(transform, deltaW, childrenClear);

            if (childrenClear)
            {
                w += deltaW;
                return true;
            }
        }
        return false;
        //SetCollisions();
    }

    void recurseChildrenSlideW(Transform t, int deltaW, bool childrenClear)
    {
        foreach (Transform child in t)
        {
            if (child.GetComponent<HyperColliderManager>())
                childrenClear = childrenClear && child.GetComponent<HyperColliderManager>().SlideW(deltaW);
            else if (child.GetComponent<HyperObject>())
                childrenClear = childrenClear && child.GetComponent<HyperObject>().SlideW(deltaW);
            else if (child.childCount > 0)
                recurseChildrenSlideW(child, deltaW, childrenClear);
        }
    }

    //deturmine if this can be seen from the other w as a solid object, remove once 4D shader is implemented
    public bool isVisibleSolid(int otherW)
    {
        if (w_depth > 0)
        {
            if (otherW <= w + w_depth && otherW >= w)
                return true;
            else
                return false;
        }
        else if (w_depth < 0)
        {
            if (otherW >= w + w_depth && otherW <= w)
                return true;
            else
                return false;
        }
        else
            return (w == otherW);
    }
}
