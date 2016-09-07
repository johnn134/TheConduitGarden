using UnityEngine;
using System.Collections;
using Valve.VR;

public class HyperColliderManager : MonoBehaviour {

    public int w;                           		//point on the w axis
    public int w_depth;                     		//how far along the w axis this object extends

    public bool movable = true;            			//is the object movable?

    public bool isParent = false;           		//is this object the parent?
    //NOTE: Enable even if no children OR if has both HyperObject and HyperColliderManager DO NOT enable this as parent and enable on the other

    bool controllersReady = false;          		//give time before trying to get input from the controllers

    SteamVR_ControllerManager controllerManager;    //The steam controller manager that holds the controller indices

    public FourthDManager IVDManager;       		//the 4D manager

	HyperCreature hypPlayer;						//Reference to the player

	void Awake(){
		//if this is the parent perform all the initialization for this object
		if (isParent)
			setW(w);

		//locate the 4Dmanager
		IVDManager = Object.FindObjectOfType<FourthDManager>();

		controllerManager = Object.FindObjectOfType<SteamVR_ControllerManager>();

		hypPlayer = Object.FindObjectOfType<HyperCreature>();
	}

    void Start()
    {
        Invoke("GetReady", 5);

        SetCollisions();
    }

    void GetReady()
    {
        controllersReady = true;
    }

    //have the objects detect the vive controller inputs to move themselves
    void LateUpdate()
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
    }

    public void WMove(int newW)
    {
        //newW = Object.FindObjectOfType<HyperCreature>().w;
        //recurseChildrenWMove(transform, newW);
    }

    public void WMove()
    {
        //int newW = Object.FindObjectOfType<HyperCreature>().w;
        //recurseChildrenWMove(transform, newW);
    }

    void recurseChildrenWMove(Transform t, int newW)
    {
        foreach (Transform child in t)
        {
            if (child.GetComponent<HyperObject>())
                child.GetComponent<HyperObject>().WMove(newW);
            else if (child.GetComponent<HyperColliderManager>())
                child.GetComponent<HyperColliderManager>().WMove(newW);
            else if (child.childCount > 0)
                recurseChildrenWMove(child, newW);
        }
    }

    public void setW(int newW)
    {
        recurseChildrenSetW(transform, newW);
    }

    void recurseChildrenSetW(Transform t, int newW)
    {
        w = newW;
        foreach (Transform child in t)
        {
            if (child.GetComponent<HyperObject>())
                child.GetComponent<HyperObject>().setW(newW);
            else if (child.GetComponent<HyperColliderManager>())
                child.GetComponent<HyperColliderManager>().setW(newW);
            else if (child.childCount > 0)
                recurseChildrenSetW(child, newW);
        }
    }

    //move this object along the w axis by deltaW
    public bool SlideW(int deltaW)
    {
        if ((deltaW > 0 && w != 6 && w + w_depth != 6) || (deltaW < 0 && w != 0 && w + w_depth != 0))
        {
            bool childrenClear = true;

            if (GetComponent<HyperObject>())
                childrenClear = childrenClear && GetComponent<HyperObject>().SlideW(deltaW);
            else
            {
                recurseChildrenSlideW(transform, deltaW, childrenClear);
            }

            if (childrenClear)
            {
                w += deltaW;
                SetCollisions();
                return true;
            }
        }
        return false;
        
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

    bool CanCollide(GameObject other)
    {
        if (w < other.GetComponent<HyperColliderManager>().w)
        {
            if (other.GetComponent<HyperColliderManager>().w_depth >= 0)
                return (w + w_depth >= other.GetComponent<HyperColliderManager>().w);
            else
                return (w + w_depth >= other.GetComponent<HyperColliderManager>().w + other.GetComponent<HyperColliderManager>().w_depth);
        }
        else if (w > other.GetComponent<HyperColliderManager>().w)
        {
            if (other.GetComponent<HyperColliderManager>().w_depth > 0)
                return (w + w_depth <= other.GetComponent<HyperColliderManager>().w + other.GetComponent<HyperColliderManager>().w_depth);
            else
                return (w + w_depth <= other.GetComponent<HyperColliderManager>().w);
        }
        else
            return (w == other.GetComponent<HyperColliderManager>().w);
    }

    //setup collisions for this object
    public void SetCollisions()
    {
        HyperColliderManager[] hyperObjects; //all hyper objects in the world

        //hyperObjects = GameObject.FindGameObjectsWithTag("HyperColliderManager");
        hyperObjects = Object.FindObjectsOfType<HyperColliderManager>();

        if (GetComponent<Collider>())
        {
            foreach (var hypObj in hyperObjects)
            {
                if (hypObj)
                {
                    if (!CanCollide(hypObj.gameObject))
                    {
                        Physics.IgnoreCollision(GetComponent<Collider>(), hypObj.gameObject.GetComponent<Collider>(), true);
                    }
                    else
                    {
                        Physics.IgnoreCollision(GetComponent<Collider>(), hypObj.gameObject.GetComponent<Collider>(), false);
                    }
                }
            }
        }
    }
}
