using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;

public class HyperObject : MonoBehaviour {

	public int w;                                   //point on the w axis
    public int w_depth;                             //how far along the w axis this object extends

    public float dullCoef = 1.0f;                   //how much to dull the color of the object by

    public bool isParent = false;                   //is this object the parent?
    //NOTE: Enable even if no children and if has both HyperObject and HyperColliderManager enable this as parent and not the other

    public bool staticRenderMode = false;      //enable if alpha should be 0 at all times if not visible from the player's w point

    FourthDManager IVDManager;                      //the 4D manager

    SteamVR_ControllerManager controllerManager;    //The steam controller manager that holds the controller indices

	Renderer _cachedRenderer;						//The renderer for this object

	HyperCreature hypPlayer;						//Reference to the player

    const int TRANSPARENT_QUEUE_ORDER = 3000;

	void Awake(){
		//locate the 4Dmanager
		IVDManager = Object.FindObjectOfType<FourthDManager>();

		controllerManager = Object.FindObjectOfType<SteamVR_ControllerManager>();

		_cachedRenderer = GetComponent<Renderer>();

		hypPlayer = Object.FindObjectOfType<HyperCreature>();
    }

    void Start()
    {
        //if this is the parent perform all the initialization for this object
        if (isParent)
            setW(w);
    }

    //have the objects detect the vive controller inputs to move themselves
    void LateUpdate()
    {
        for (int i = 0; i < controllerManager.indices.Length; i++)
        {
            if (controllerManager.indices[i] != OpenVR.k_unTrackedDeviceIndexInvalid)
            {
                if (SteamVR_Controller.Input((int)controllerManager.indices[i]).GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger))
                {
                    WMove();
                }
            }
        }
    }

    //the player has moved to a new w point, remove once 4D shader is implemented
    public void WMove()
    {
		int newW = hypPlayer.w;
        if (isVisibleSolid(newW))
        {//this object is on the player's w point or is wide enough to be seen and touched
            StartCoroutine(ColorTrans(newW, 1.0f));
        }
		else{
            float targA = PeripheralAlpha(newW);
            /*if (vanishWhenTransparent)
                targA = 0.0f;*/

            if (!staticRenderMode)
            {
                _cachedRenderer.material.SetFloat("_Mode", 2);
                _cachedRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                _cachedRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                _cachedRenderer.material.SetInt("_ZWrite", 0);
                _cachedRenderer.material.DisableKeyword("_ALPHATEST_ON");
                _cachedRenderer.material.EnableKeyword("_ALPHABLEND_ON");
                _cachedRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                _cachedRenderer.material.renderQueue = 3000;
            }

            //if got -1 for target alpha then already proper alpha so skip the color lerp
            if (targA != -1.0f)
            {
                _cachedRenderer.material.SetFloat("_Mode", 2);

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
        }

        //GetComponent<MeshRenderer>().material.SetFloat("_WPos", (float)w);
        //GetComponent<MeshRenderer>().material.renderQueue = TRANSPARENT_QUEUE_ORDER + getNewOrder(newW);
    }

    public void setW(int newW)
    {
        w = newW;
        WMove();

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

    float PeripheralAlpha(int newW)
    {
        if (Mathf.Abs(newW - w) <= hypPlayer.w_perif * 2 || Mathf.Abs(newW - (w + w_depth)) <= hypPlayer.w_perif * 2)
        {
            if (_cachedRenderer.material.color.a == .2f)
                return -1.0f;

            return .2f;
        }

        if (_cachedRenderer.material.color.a == 0.0f)
            return -1.0f;

        return 0.0f;
    }

    //smoothly change the color of this object, rmove once 4D shader is implemented
    IEnumerator ColorTrans(int newW, float targetA){
        Color targetColor;
		
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
		
		for(float i = 0.0f; i <= 1.0f; i += .1f){
            
			_cachedRenderer.material.color = Color.Lerp(_cachedRenderer.material.color, targetColor, .2f);

			yield return null;
		}

        _cachedRenderer.material.color = targetColor;

        if(targetA == 1f && !staticRenderMode && isVisibleSolid(hypPlayer.w))
        {
            _cachedRenderer.material.SetFloat("_Mode", 0);
            _cachedRenderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            _cachedRenderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            _cachedRenderer.material.SetInt("_ZWrite", 1);
            _cachedRenderer.material.DisableKeyword("_ALPHATEST_ON");
            _cachedRenderer.material.DisableKeyword("_ALPHABLEND_ON");
            _cachedRenderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            _cachedRenderer.material.renderQueue = -1;
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
