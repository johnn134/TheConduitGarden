using UnityEngine;
using System.Collections.Generic;
using Valve.VR;

public class GetInputVR : MonoBehaviour
{
    public int myIndex = -1;
    public GameObject holding = null;
    public bool griping = false;
    //public GameObject controlInfo = null;


    void Start()
    {
        myIndex = (int)gameObject.GetComponent<SteamVR_TrackedObject>().index;
    }

    void OnTriggerStay(Collider other)
    {
        /*if(griping && !holding && other.gameObject.GetComponent<HyperColliderManager>().isVisibleSolid(gameObject.transform.parent.GetComponent<HyperCreature>().w) && other.gameObject.GetComponent<HyperColliderManager>().movable)
        {
            holding = other.gameObject;
            holding.transform.parent = transform;
        }*/
        if (griping && other.gameObject.GetComponent<HyperColliderManager>().isVisibleSolid(gameObject.transform.parent.GetComponent<HyperCreature>().w) && (!holding || other.gameObject.Equals(holding.gameObject)) && other.gameObject.GetComponent<HyperColliderManager>().movable)
        {
            holding = other.gameObject;
            holding.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            holding.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
            holding.transform.parent = transform;
            holding.transform.position = transform.position;
            holding.transform.rotation = transform.rotation;
            holding.GetComponent<Rigidbody>().angularVelocity = 3 * SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).angularVelocity;
            holding.GetComponent<Rigidbody>().velocity = 2 * SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).velocity;
        }
        else
        {
            if (holding && other.gameObject.Equals(holding.gameObject))
            {
                holding.transform.parent = null;
                holding = null;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (holding && other.gameObject.Equals(holding.gameObject))
        {
            holding.transform.parent = null;
            holding = null;
        }
    }

    EVRButtonId[] buttonIds = new EVRButtonId[] {
        EVRButtonId.k_EButton_ApplicationMenu,
        EVRButtonId.k_EButton_Grip,
        EVRButtonId.k_EButton_SteamVR_Touchpad,
        EVRButtonId.k_EButton_SteamVR_Trigger
    };

    EVRButtonId[] axisIds = new EVRButtonId[] {
        EVRButtonId.k_EButton_SteamVR_Touchpad,
        EVRButtonId.k_EButton_SteamVR_Trigger
    };

    public Transform point, pointer;

    bool tUp, tDown, tLeft, tRight = false;
    int stage = 0;

    void Update()
    {
        /*if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetTouch(EVRButtonId.k_EButton_SteamVR_Touchpad) && tag.Equals("RightControl"))
        {
            var axis = SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);

            if (axis.y > .2 && (axis.x > -.2 && axis.x <= .2))
                tUp = true;
            if (axis.y < .2 && (axis.x >= -.2 && axis.x < .2))
                tDown = true;
            if (axis.x > .2 && (axis.y >= -.2 && axis.y < .2))
                tRight = true;
            if (axis.x < .2 && (axis.y > -.2 && axis.y <= .2))
                tLeft = true;
        }
        else
        {
            stage = 0;
            tUp = false;
            tDown = false;
            tLeft = false;
            tRight = false;
        }

        if(stage == 0)
        {
            if (tUp || tDown || tLeft || tRight)
                stage = 1;
        }
        else if(stage == 1)
        {
            
        }

        if (tUp && tDown && tLeft && tRight)
        {
            Debug.Log("ALL");
            stage = 0;
            tUp = false;
            tDown = false;
            tLeft = false;
            tRight = false;
        }*/

        /*if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetTouch(EVRButtonId.k_EButton_SteamVR_Touchpad) && tag.Equals("RightControl")){
            var axis = SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
            if(axis.y <= .1 && axis.y > -.1){
                gameObject.transform.parent.GetComponent<HyperCreature>().w = 3;
                gameObject.transform.parent.GetComponent<HyperCreature>().WMove();
            }
            else if(axis.y <= .4 && axis.y > .1){
                gameObject.transform.parent.GetComponent<HyperCreature>().w = 4;
                gameObject.transform.parent.GetComponent<HyperCreature>().WMove();
            }
            else if(axis.y <= .7 && axis.y > .4){
                gameObject.transform.parent.GetComponent<HyperCreature>().w = 5;
                gameObject.transform.parent.GetComponent<HyperCreature>().WMove();
            }
            else if(axis.y <= 1.0 && axis.y > .7){
                gameObject.transform.parent.GetComponent<HyperCreature>().w = 6;
                gameObject.transform.parent.GetComponent<HyperCreature>().WMove();
            }
            else if(axis.y <= -.1 && axis.y > -.4){
                gameObject.transform.parent.GetComponent<HyperCreature>().w = 2;
                gameObject.transform.parent.GetComponent<HyperCreature>().WMove();
            }
            else if(axis.y <= -.4 && axis.y > -.7){
                gameObject.transform.parent.GetComponent<HyperCreature>().w = 1;
                gameObject.transform.parent.GetComponent<HyperCreature>().WMove();
            }
            else if(axis.y <= -.7 && axis.y > -1.0){
                gameObject.transform.parent.GetComponent<HyperCreature>().w = 0;
                gameObject.transform.parent.GetComponent<HyperCreature>().WMove();
            }
        }*/

        /*if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetPressDown(EVRButtonId.k_EButton_SteamVR_Touchpad) && gameObject.tag.Equals("RightControl")){
            gameObject.transform.parent.transform.position = GameObject.Find("OriginBox").transform.position;
        }*/

        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetPressDown(EVRButtonId.k_EButton_SteamVR_Touchpad))
        {
            if (holding)
                InteractWithHolding(gameObject.tag);
        }

        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger))
        {
            //check the other controller if it has an object as the w movement affects that object too
            GameObject otherControl;
            if (tag.Equals("RightControl"))
                otherControl = GameObject.FindGameObjectWithTag("LeftControl");
            else
                otherControl = GameObject.FindGameObjectWithTag("RightControl");

            //make sure we found the other controller
            if (otherControl)
            {
                if (otherControl.GetComponent<GetInputVR>().holding)
                {
                    if (gameObject.tag.Equals("RightControl"))
                        otherControl.GetComponent<GetInputVR>().holding.GetComponent<HyperColliderManager>().SlideW(1);
                    else
                        otherControl.GetComponent<GetInputVR>().holding.GetComponent<HyperColliderManager>().SlideW(-1);
                }
                /*else{
					if(holding){
						otherControl.GetComponent<GetInputVR>().holding.GetComponent<HyperObject>().SlideW(1);
					}
				}*/
            }
            else
                Debug.Log("CANNOT FIND OTHER CONTROLLER");

            //check if the player is holding an object with this controller
            if (gameObject.tag.Equals("RightControl"))
            {
                if (holding)
                    holding.GetComponent<HyperColliderManager>().SlideW(1);
                gameObject.transform.parent.GetComponent<HyperCreature>().WMove(1);
            }
            else {
                if (holding)
                    holding.GetComponent<HyperColliderManager>().SlideW(-1);
                gameObject.transform.parent.GetComponent<HyperCreature>().WMove(-1);
            }


        }

        /*if (griping && other.gameObject.GetComponent<HyperColliderManager>().isVisibleSolid(gameObject.transform.parent.GetComponent<HyperCreature>().w) && (!holding || other.gameObject.Equals(holding.gameObject)) && other.gameObject.GetComponent<HyperColliderManager>().movable)
        {
            holding = other.gameObject;
            holding.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            holding.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);
            holding.transform.parent = transform;
            holding.transform.position = transform.position;
            holding.transform.rotation = transform.rotation;
            holding.GetComponent<Rigidbody>().angularVelocity = 3 * SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).angularVelocity;
            holding.GetComponent<Rigidbody>().velocity = 2 * SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).velocity;
        }
        else
        {
            if (holding && other.gameObject.Equals(holding.gameObject))
            {
                holding.transform.parent = null;
                holding = null;
            }
        }*/

        /*if (holding)
        {
            holding.GetComponent<Rigidbody>().velocity = Vector3.zero;
            holding.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            holding.transform.position = transform.position;
            holding.transform.rotation = transform.rotation;

        }

        if (griping && !holding)
            griping = false;*/

        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetPressDown(EVRButtonId.k_EButton_Grip))
        {
            /*griping = !griping;

            if (holding && !griping)
            {
                holding.GetComponent<Rigidbody>().angularVelocity = 3 * SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).angularVelocity;
                holding.GetComponent<Rigidbody>().velocity = 2 * SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).velocity;
                holding.transform.parent = null;
                holding = null;
            }*/
            griping = true;
        }
        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetPressUp(EVRButtonId.k_EButton_Grip))
        {
            griping = false;
        }

            /*foreach (var index in controllerIndices)
            {
                var overlay = SteamVR_Overlay.instance;
                if (overlay && point && pointer)
                {
                    var t = SteamVR_Controller.Input(index).transform;
                    pointer.transform.localPosition = t.pos;
                    pointer.transform.localRotation = t.rot;

                    var results = new SteamVR_Overlay.IntersectionResults();
                    var hit = overlay.ComputeIntersection(t.pos, t.rot * Vector3.forward, ref results);
                    if (hit)
                    {
                        point.transform.localPosition = results.point;
                        point.transform.localRotation = Quaternion.LookRotation(results.normal);
                    }

                    continue;
                }
            }*/
        }

    void InteractWithHolding(string controller)
    {
        /*
		if (holding.tag.Equals("Chime"))
        {
            holding.GetComponent<Chime>().IncreasePitch();
        }
		*/
        if (holding.GetComponent<Shears>() != null)
        {
            holding.GetComponent<Shears>().snip();
        }
        if (holding.GetComponent<Insecticide>() != null)
        {
            holding.GetComponent<Insecticide>().startSpray();
        }
        
    }

    /*public void DetermineLayer(int newW)
    {
        switch (newW)
        {
            case 0:
                gameObject.layer = 8;
                break;
            case 1:
                gameObject.layer = 9;
                break;
            case 2:
                gameObject.layer = 10;
                break;
            case 3:
                gameObject.layer = 11;
                break;
            case 4:
                gameObject.layer = 12;
                break;
            case 5:
                gameObject.layer = 13;
                break;
            case 6:
                gameObject.layer = 14;
                break;
            default:
                gameObject.layer = 0;
                break;
        }
    }*/
}


