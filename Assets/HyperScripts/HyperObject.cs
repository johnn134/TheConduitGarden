using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;

public class HyperObject : MonoBehaviour {

	public int w;                                   //point on the w axis
    public int w_depth;                             //how far along the w axis this object extends

    public float dullCoef = 1.0f;                   //how much to dull the color of the object by

	public Texture texture;

    public bool isParent = false;                   //is this object the parent?
    //NOTE: Enable even if no children and if has both HyperObject and HyperColliderManager enable this as parent and not the other

    public bool staticRenderMode = false;      		//whether the shader on this object can change

    SteamVR_ControllerManager controllerManager;    //The steam controller manager that holds the controller indices

	Renderer _cachedRenderer;						//The renderer for this object

	HyperCreature hypPlayer;						//Reference to the player

	#region Callbacks

	void Awake(){
		controllerManager = SteamVR_ControllerManager.instance;

		_cachedRenderer = GetComponent<Renderer>();

        hypPlayer = HyperCreature.instance;
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

	#endregion
	#region Movement Functions

    //the player has moved to a new w point, remove once 4D shader is implemented
    public void WMove()
    {
		int playerW = hypPlayer.w;

		if (isVisibleSolid(playerW)) {//this object is on the player's w point or is wide enough to be seen and touched
			StartCoroutine(ColorTrans(playerW, 1.0f));
        }
		else {
			float targA = PeripheralAlpha(playerW);

            if (!staticRenderMode)
            {
				setTransparentShader();
            }

            //if got -1 for target alpha then already proper alpha so skip the color lerp
            if (targA != -1.0f)
            {
                //fade out if not on player's w point
                if (w_depth > 0)
                {
					if (w > playerW)
                        StartCoroutine(ColorTrans(w, targA));
                    else
                        StartCoroutine(ColorTrans(w + w_depth, targA));
                }
                else
                {
					if (w < playerW)
                        StartCoroutine(ColorTrans(w, targA));
                    else
                        StartCoroutine(ColorTrans(w + w_depth, targA));
                }
            }
        }
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

	#endregion
	#region Utility Functions

	//smoothly change the color of this object, rmove once 4D shader is implemented
	IEnumerator ColorTrans(int newW, float targetA){
		Color targetColor = Color.white;

		//deturmine the target color based on w point
		switch(newW) {
			case 0:
				targetColor = Color.red;
				break;
			case 1:
				targetColor = new Color(1, .45f, 0);
				break;
			case 2:
				targetColor = Color.yellow;
				break;
			case 3:
				targetColor = Color.green;
				break;
			case 4:
				targetColor = Color.cyan;
				break;
			case 5:
				targetColor = Color.blue;
				break;
			case 6:
				targetColor = Color.magenta;
				break;
		}

		targetColor.a = targetA;
		targetColor.r /= dullCoef;
		targetColor.g /= dullCoef;
		targetColor.b /= dullCoef;

		for(float i = 0.0f; i <= 1.0f; i += .1f){

			_cachedRenderer.material.SetColor("_Color", Color.Lerp(_cachedRenderer.material.GetColor("_Color"), targetColor, .2f));

			yield return null;
		}

		_cachedRenderer.material.SetColor("_Color", targetColor);

		if(targetA == 1.0f && !staticRenderMode && isVisibleSolid(hypPlayer.w))
		{
			setOpaqueShader();
		}
	}

	/*
	 * PeripheralAlpha
	 * 
	 * Determines the alpha of this object depending on its position
	 * compared to the player's peripheral vision
	 * 
	 * If returning -1, skip the colortrans coroutine
	 * .2 - within player's vision
	 * 0 - not in player's vision
	 * 
	 * @param newW - player's w position
	 * @return float - alpha of this object's material
	 */
	float PeripheralAlpha(int newW)
	{
		if (Mathf.Abs(newW - w) <= hypPlayer.w_perif * 2 || Mathf.Abs(newW - (w + w_depth)) <= hypPlayer.w_perif * 2) {	//This object is in the player's peripheral range
			if (_cachedRenderer.material.color.a == .2f)
				return -1.0f;

			return .2f;
		}

		if (_cachedRenderer.material.color.a == 0.0f)
			return -1.0f;

		return 0.0f;
	}

    //determine if this can be seen from the other w as a solid object
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

	/*
	 * Swaps the shader on this object's material to the fourth dimension opaque shader
	 */
	void setOpaqueShader() {
		Color temp = _cachedRenderer.material.GetColor("_Color");
		_cachedRenderer.material.shader = Shader.Find("FourthDimension/FourthDimensionOpaqueShader");
		_cachedRenderer.material.SetTexture("_MainTex", texture);
		_cachedRenderer.material.SetColor("_Color", temp);
	}

	/*
	 * Swaps the shader on this object's material to the fourth dimension transparent shader
	 */
	void setTransparentShader() {
		Color temp = _cachedRenderer.material.GetColor("_Color");
		_cachedRenderer.material.shader = Shader.Find("FourthDimension/FourthDimensionTransparentShader");
		_cachedRenderer.material.SetTexture("_MainTex", texture);
		_cachedRenderer.material.SetColor("_Color", temp);
	}

	#endregion
}
