using UnityEngine;
using System.Collections.Generic;
using Valve.VR;

public class GetInputVR : MonoBehaviour
{
    public enum ControlType
    {
        Trigger,
        VSlide,
        HSlide,
        Scroll
    }

    public int myIndex = -1;
    public GameObject holding = null;
    public bool griping = false;
    public bool blockMoveMethods = false;
    public bool callWMoveOnAllHyperScripts = false;
    public ControlType controlType;
    HyperCreature player;


    void Start()
    {
        myIndex = (int)gameObject.GetComponent<SteamVR_TrackedObject>().index;
        player = HyperCreature.instance;
    }

    void OnTriggerStay(Collider other)
    {
        if (griping && other.gameObject.GetComponent<HyperColliderManager>().isVisibleSolid(player.w) && (!holding || other.gameObject.Equals(holding.gameObject)) && other.gameObject.GetComponent<HyperColliderManager>().movable)
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

    int tUp, tDown, tLeft, tRight = -1;
    int stage = 0;

    void Update()
    {
        callWMoveOnAllHyperScripts = false;
        if (!blockMoveMethods)
        {
            switch ((int)controlType)
            {
                case 0:
                    TriggerControl();
                    break;
                case 1:
                    VerticalSliderControl();
                    break;
                case 2:
                    HorizontalSliderControl();
                    break;
                case 3:
                    ScrollControl();
                    break;
            }
        }

        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetPressDown(EVRButtonId.k_EButton_SteamVR_Touchpad))
        {
            if (holding)
                InteractWithHolding(gameObject.tag);
        }

        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetPressDown(EVRButtonId.k_EButton_Grip))
        {
            griping = true;
        }
        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetPressUp(EVRButtonId.k_EButton_Grip))
        {
            griping = false;
        }
    }

    void InteractWithHolding(string controller)
    {
        if (holding.GetComponent<Shears>() != null)
        {
            holding.GetComponent<Shears>().snip();
        }
        if (holding.GetComponent<Insecticide>() != null)
        {
            holding.GetComponent<Insecticide>().startSpray();
        }
        
    }

    void ScrollControl()
    {
        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetTouch(EVRButtonId.k_EButton_SteamVR_Touchpad))
        {
            var axis = SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);

            if (axis.y > .2 && (axis.x > -.2 && axis.x <= .2))
                tUp = stage;
            if (axis.y < .2 && (axis.x >= -.2 && axis.x < .2))
                tDown = stage;
            if (axis.x > .2 && (axis.y >= -.2 && axis.y < .2))
                tRight = stage;
            if (axis.x < .2 && (axis.y > -.2 && axis.y <= .2))
                tLeft = stage;
        }
        else
        {
            stage = 0;
            tUp = -1;
            tDown = -1;
            tLeft = -1;
            tRight = -1;
        }

        if (stage == 0)
        {
            if (tUp == 0 || tDown == 0 || tLeft == 0 || tRight == 0)
                stage = 1;
        }
        else if (stage == 1)
        {
            if ((tUp == 1 && tLeft == 1) ||
               (tUp == 1 && tRight == 1) ||
               (tDown == 1 && tRight == 1) ||
               (tDown == 1 && tLeft == 1))
                stage = 2;
            else if ((tUp == 1 && tLeft == -1 && tRight == -1 && tDown == -1) ||
                    (tUp == -1 && tLeft == 1 && tRight == -1 && tDown == -1) ||
                    (tUp == -1 && tLeft == -1 && tRight == 1 && tDown == -1) ||
                    (tUp == -1 && tLeft == -1 && tRight == -1 && tDown == 1))
                stage = 1;
            else
            {
                stage = 0;
                tUp = -1;
                tDown = -1;
                tLeft = -1;
                tRight = -1;
            }
        }
        else if (stage == 2)
        {
            if ((tUp == 1 && tRight == 2) || (tRight == 1 && tDown == 2) || (tDown == 1 && tLeft == 2) || (tLeft == 1 && tUp == 2))
            {
                player.WMove(1);
                callWMoveOnAllHyperScripts = true;
                SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
            }
            else if ((tUp == 2 && tRight == 1) || (tRight == 2 && tDown == 1) || (tDown == 2 && tLeft == 1) || (tLeft == 2 && tUp == 1))
            {
                player.WMove(-1);
                callWMoveOnAllHyperScripts = true;
                SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
            }

            stage = 0;
            tUp = -1;
            tDown = -1;
            tLeft = -1;
            tRight = -1;
        }
    }

    void VerticalSliderControl()
    {
        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetTouch(EVRButtonId.k_EButton_SteamVR_Touchpad))
        {
            var axis = SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
            if (axis.y <= .1 && axis.y > -.1)
            {
                player.w = 3;
                callWMoveOnAllHyperScripts = true;
            }
            else if (axis.y <= .4 && axis.y > .1)
            {
                player.w = 4;
                callWMoveOnAllHyperScripts = true;
            }
            else if (axis.y <= .7 && axis.y > .4)
            {
                player.w = 5;
                callWMoveOnAllHyperScripts = true;
            }
            else if (axis.y <= 1.0 && axis.y > .7)
            {
                player.w = 6;
                callWMoveOnAllHyperScripts = true;
            }
            else if (axis.y <= -.1 && axis.y > -.4)
            {
                player.w = 2;
                callWMoveOnAllHyperScripts = true;
            }
            else if (axis.y <= -.4 && axis.y > -.7)
            {
                player.w = 1;
                callWMoveOnAllHyperScripts = true;
            }
            else if (axis.y <= -.7 && axis.y > -1.0)
            {
                player.w = 0;
                callWMoveOnAllHyperScripts = true;
            }
        }
    }

    void HorizontalSliderControl()
    {
        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetTouch(EVRButtonId.k_EButton_SteamVR_Touchpad))
        {
            var axis = SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
            if (axis.x <= .1 && axis.x > -.1)
            {
                player.w = 3;
                callWMoveOnAllHyperScripts = true;
            }
            else if (axis.x <= .4 && axis.x > .1)
            {
                player.w = 4;
                callWMoveOnAllHyperScripts = true;
            }
            else if (axis.x <= .7 && axis.x > .4)
            {
                player.w = 5;
                callWMoveOnAllHyperScripts = true;
            }
            else if (axis.x <= 1.0 && axis.x > .7)
            {
                player.w = 6;
                callWMoveOnAllHyperScripts = true;
            }
            else if (axis.x <= -.1 && axis.x > -.4)
            {
                player.w = 2;
                callWMoveOnAllHyperScripts = true;
            }
            else if (axis.x <= -.4 && axis.x > -.7)
            {
                player.w = 1;
                callWMoveOnAllHyperScripts = true;
            }
            else if (axis.x <= -.7 && axis.y > -1.0)
            {
                player.w = 0;
                callWMoveOnAllHyperScripts = true;
            }
        }
    }

    void TriggerControl()
    {
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
            }
            else
                Debug.Log("CANNOT FIND OTHER CONTROLLER");

            //check if the player is holding an object with this controller
            if (gameObject.tag.Equals("RightControl"))
            {
                if (holding)
                    holding.GetComponent<HyperColliderManager>().SlideW(1);
                player.WMove(1);
                callWMoveOnAllHyperScripts = true;
            }
            else {
                if (holding)
                    holding.GetComponent<HyperColliderManager>().SlideW(-1);
                player.WMove(-1);
                callWMoveOnAllHyperScripts = true;
            }


        }
    }
}


