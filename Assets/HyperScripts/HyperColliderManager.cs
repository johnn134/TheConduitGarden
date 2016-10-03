using UnityEngine;
using System.Collections;
using Valve.VR;

public class HyperColliderManager : MonoBehaviour {

    public int w;                           		//point on the w axis
    public int w_depth;                     		//how far along the w axis this object extends

    public bool movable = true;            			//is the object movable?

    public bool isParent = false;           		//is this object the parent?
    //NOTE: Enable even if no children OR if has both HyperObject and HyperColliderManager 
	//		DO NOT enable this as parent and enable on the other

    FourthDManager hyperManager;

	void Awake(){
        hyperManager = FourthDManager.instance;
	}

    void Start()
    {
        //if this is the parent perform all the initialization for this object
        if (isParent)
            setW(w);

		hyperManager.AddToList(gameObject);
        SetCollisions();
    }

    void OnDestroy()
    {
        hyperManager.RemoveFromList(gameObject);
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
		if ((deltaW > 0 && w != HyperObject.W_RANGE && w + w_depth != HyperObject.W_RANGE)
			|| (deltaW < 0 && w != 0 && w + w_depth != 0))
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

    //determine if this can be seen from the other w as a solid object, remove once 4D shader is implemented
    public bool isVisibleSolid(int otherW)
    {
		bool positiveDepth = 	(w_depth > 0 && otherW <= w + w_depth && otherW >= w);
		bool negativeDepth = 	(w_depth < 0 && otherW >= w + w_depth && otherW <= w);
		bool noDepth =			(w_depth == 0 && w == otherW);

		return positiveDepth || negativeDepth || noDepth;
    }

    bool CanCollide(GameObject other)
    {
        if (w < other.GetComponent<HyperColliderManager>().w)
        {
            if (other.GetComponent<HyperColliderManager>().w_depth >= 0)
                return (w + w_depth >= other.GetComponent<HyperColliderManager>().w);
            else
                return (w + w_depth >= other.GetComponent<HyperColliderManager>().w
										+ other.GetComponent<HyperColliderManager>().w_depth);
        }
        else if (w > other.GetComponent<HyperColliderManager>().w)
        {
            if (other.GetComponent<HyperColliderManager>().w_depth > 0)
                return (w + w_depth <= other.GetComponent<HyperColliderManager>().w
										+ other.GetComponent<HyperColliderManager>().w_depth);
            else
                return (w + w_depth <= other.GetComponent<HyperColliderManager>().w);
        }
        else
            return (w == other.GetComponent<HyperColliderManager>().w);
    }

    //setup collisions for this object
    public void SetCollisions()
    {
        foreach (var hypObj in hyperManager.GetList())
        {
            if (hypObj)
				Physics.IgnoreCollision(GetComponent<Collider>(), 
										hypObj.GetComponent<Collider>(), 
										!CanCollide(hypObj));
        }
    }
}
