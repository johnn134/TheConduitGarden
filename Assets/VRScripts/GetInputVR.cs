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

    public enum InteractionButton
    {
        TouchPad,
        Trigger
    }

    public int myIndex = -1;
    public GameObject holding = null;
    public bool griping = false;
    public bool blockMoveMethods = false;
    public bool callWMoveOnAllHyperScripts = false;
    public ControlType controlType;
    public InteractionButton interactionButton;
    HyperCreature player;


    void Start()
    {
        myIndex = (int)gameObject.GetComponent<SteamVR_TrackedObject>().index;
        player = HyperCreature.instance;
    }

    //this where objects can be grabbed and held
    void OnTriggerStay(Collider other)
    {
        if (griping && other.gameObject.GetComponent<HyperColliderManager>().isVisibleSolid(player.w) && (!holding || other.gameObject.Equals(holding.gameObject)) && other.gameObject.GetComponent<HyperColliderManager>().movable)
        {//if the grip buttons are depressed, the player is on the same w as the other object, if this controller is not already holding anything or the other object is still the object being held, is the other object movable
            holding = other.gameObject;

            //freeze the object's velocities so it doesnt move in your hand
            holding.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            holding.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0);

            //make the object a child of this controller so it moves and rotates with it and snap it to the controller's position
            holding.transform.parent = transform;
            holding.transform.position = transform.position;
            holding.transform.rotation = transform.rotation;

            //give the object velocities from the controller so when it is released it has physics instead of dropping to the ground
            holding.GetComponent<Rigidbody>().angularVelocity = 3 * SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).angularVelocity;
            holding.GetComponent<Rigidbody>().velocity = 2 * SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).velocity;
        }
        else
        {
            if (holding && other.gameObject.Equals(holding.gameObject))
            {//no longer gripping so only deparent the object that was being held
                holding.transform.parent = null;
                holding = null;
            }
        }
    }

    //this is needed for the case where the player moves the controller faster than the position update can move the object to the controller's position so this
    //will make the object cleanly slip out of the player's hand
    void OnTriggerExit(Collider other)
    {
        if (holding && other.gameObject.Equals(holding.gameObject))
        {
            holding.transform.parent = null;
            holding = null;
        }
    }

    //needed for scroll controls
    int tUp, tDown, tLeft, tRight = -1; //tocuhed up, down, left and right, the number is equal to the order they were touched from 1 to 2
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


        //pressing the touchpad interacts with the object held if it has a behavior
        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetPressDown(EVRButtonId.k_EButton_SteamVR_Touchpad) && interactionButton == InteractionButton.TouchPad)
        {
            if (holding)
                InteractWithHolding();
        }
        else if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger) && interactionButton == InteractionButton.Trigger)
        {
            if (holding)
                InteractWithHolding();
        }

        //grip buttons control grabbing
        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetPressDown(EVRButtonId.k_EButton_Grip))
        {
            griping = true;
        }
        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetPressUp(EVRButtonId.k_EButton_Grip))
        {
            griping = false;
        }
    }

    /*
    * InteractWithHolding
    *
    * Call a function on the object held by this controller depending on what scrip the object has on it
    */
    void InteractWithHolding()
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

    /*
    * ScrollControl
    *
    * Control scheme for moving on the w axis similar to early iPod controls by making circles on the touchpad
    */
    void ScrollControl()
    {
        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetTouch(EVRButtonId.k_EButton_SteamVR_Touchpad))
        {
            var axis = SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);

            //define areas for up, down, left and right
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
        { //if the players stops touching the touch pad, reset all the variables
            stage = 0;
            tUp = -1;
            tDown = -1;
            tLeft = -1;
            tRight = -1;
        }

        if (stage == 0)
        {//wait for the first touch, the variable will have a value of 1
            if (tUp == 0 || tDown == 0 || tLeft == 0 || tRight == 0)
                stage = 1;
        }
        else if (stage == 1)
        {//wait for acceptable second input, the second touch should be directly clockwise or counterclockwise from the first touch, the second touch will register as 1 here and then have a value of 2 in the next stage
            if ((tUp == 1 && tLeft == 1) ||
               (tUp == 1 && tRight == 1) ||
               (tDown == 1 && tRight == 1) ||
               (tDown == 1 && tLeft == 1))
                stage = 2;
            else if ((tUp == 1 && tLeft == -1 && tRight == -1 && tDown == -1) ||
                    (tUp == -1 && tLeft == 1 && tRight == -1 && tDown == -1) ||
                    (tUp == -1 && tLeft == -1 && tRight == 1 && tDown == -1) ||
                    (tUp == -1 && tLeft == -1 && tRight == -1 && tDown == 1))
            {//check if the touch is the same as the first touch to prevent the values from resetting
                stage = 1;
            }
            else
            {//if the second touch is invalid reset the variables
                stage = 0;
                tUp = -1;
                tDown = -1;
                tLeft = -1;
                tRight = -1;
            }
        }
        else if (stage == 2)
        {//depending on the order the first and second touches happened, detect if movement of the finger was clockwise or counter clockwise and move on the w axis appropriatly
            if ((tUp == 1 && tRight == 2) || (tRight == 1 && tDown == 2) || (tDown == 1 && tLeft == 2) || (tLeft == 1 && tUp == 2))
            {
                player.WMove(1);
                SlideAllHeldObjectsAlongW(1);
                callWMoveOnAllHyperScripts = true;
                SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
            }
            else if ((tUp == 2 && tRight == 1) || (tRight == 2 && tDown == 1) || (tDown == 2 && tLeft == 1) || (tLeft == 2 && tUp == 1))
            {
                player.WMove(-1);
                SlideAllHeldObjectsAlongW(-1);
                callWMoveOnAllHyperScripts = true;
                SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
            }

            //reset variables to setup for moving again
            stage = 0;
            tUp = -1;
            tDown = -1;
            tLeft = -1;
            tRight = -1;
        }
    }

    /*
    * VerticalSliderControl
    *
    * Moving along the w axis similar to sliding a slider along a vertical track
    */
    void VerticalSliderControl()
    {
        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetTouch(EVRButtonId.k_EButton_SteamVR_Touchpad))
        {
            var axis = SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
            if (axis.y <= .1 && axis.y > -.1)
            {//each w point has its own area on the y axis of the controller touchpad
                if (player.w - 1 == 3)
                {//only register if moving to values next to the current w position of the player
                    player.w = 3;
                    SlideAllHeldObjectsAlongW(-1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
                else if (player.w + 1 == 3)
                {
                    player.w = 3;
                    SlideAllHeldObjectsAlongW(1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
            }
            else if (axis.y <= .4 && axis.y > .1)
            {
                if (player.w - 1 == 4)
                {
                    player.w = 4;
                    SlideAllHeldObjectsAlongW(-1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
                else if (player.w + 1 == 4)
                {
                    player.w = 4;
                    SlideAllHeldObjectsAlongW(1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
            }
            else if (axis.y <= .7 && axis.y > .4)
            {
                if (player.w - 1 == 5)
                {
                    player.w = 5;
                    SlideAllHeldObjectsAlongW(-1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
                else if (player.w + 1 == 5)
                {
                    player.w = 5;
                    SlideAllHeldObjectsAlongW(1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
            }
            else if (axis.y <= 1.0 && axis.y > .7)
            {
                if (player.w + 1 == 6)
                {
                    player.w = 6;
                    SlideAllHeldObjectsAlongW(1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
            }
            else if (axis.y <= -.1 && axis.y > -.4)
            {
                if (player.w - 1 == 2)
                {
                    player.w = 2;
                    SlideAllHeldObjectsAlongW(-1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
                else if (player.w + 1 == 2)
                {
                    player.w = 2;
                    SlideAllHeldObjectsAlongW(1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
            }
            else if (axis.y <= -.4 && axis.y > -.7)
            {
                if (player.w - 1 == 1)
                {
                    player.w = 1;
                    SlideAllHeldObjectsAlongW(-1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
                else if (player.w + 1 == 1)
                {
                    player.w = 1;
                    SlideAllHeldObjectsAlongW(1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
            }
            else if (axis.y <= -.7 && axis.y > -1.0)
            {
                if (player.w - 1 == 0)
                {
                    player.w = 0;
                    SlideAllHeldObjectsAlongW(-1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
            }
        }
    }

    /*
   * HorizonalSliderControl
   *
   * Moving along the w axis similar to sliding a slider along a horizontal track
   * Code is the same as the vertical slider controls except all the y variables are replaced with x
   */
    void HorizontalSliderControl()
    {
        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetTouch(EVRButtonId.k_EButton_SteamVR_Touchpad))
        {
            var axis = SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetAxis(EVRButtonId.k_EButton_SteamVR_Touchpad);
            if (axis.x <= .1 && axis.x > -.1)
            {
                if (player.w - 1 == 3)
                {
                    player.w = 3;
                    SlideAllHeldObjectsAlongW(-1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
                else if (player.w + 1 == 3)
                {
                    player.w = 3;
                    SlideAllHeldObjectsAlongW(1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
            }
            else if (axis.x <= .4 && axis.x > .1)
            {
                if (player.w - 1 == 4)
                {
                    player.w = 4;
                    SlideAllHeldObjectsAlongW(-1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
                else if (player.w + 1 == 4)
                {
                    player.w = 4;
                    SlideAllHeldObjectsAlongW(1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
            }
            else if (axis.x <= .7 && axis.x > .4)
            {
                if (player.w - 1 == 5)
                {
                    player.w = 5;
                    SlideAllHeldObjectsAlongW(-1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
                else if (player.w + 1 == 5)
                {
                    player.w = 5;
                    SlideAllHeldObjectsAlongW(1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
            }
            else if (axis.x <= 1.0 && axis.x > .7)
            {
                if (player.w + 1 == 6)
                {
                    player.w = 6;
                    SlideAllHeldObjectsAlongW(1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
            }
            else if (axis.x <= -.1 && axis.x > -.4)
            {
                if (player.w - 1 == 2)
                {
                    player.w = 2;
                    SlideAllHeldObjectsAlongW(-1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
                else if (player.w + 1 == 2)
                {
                    player.w = 2;
                    SlideAllHeldObjectsAlongW(1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
            }
            else if (axis.x <= -.4 && axis.x > -.7)
            {
                if (player.w - 1 == 1)
                {
                    player.w = 1;
                    SlideAllHeldObjectsAlongW(-1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
                else if (player.w + 1 == 1)
                {
                    player.w = 1;
                    SlideAllHeldObjectsAlongW(1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
            }
            else if (axis.x <= -.7 && axis.x > -1.0)
            {
                if (player.w - 1 == 0)
                {
                    player.w = 0;
                    SlideAllHeldObjectsAlongW(-1);
                    callWMoveOnAllHyperScripts = true;
                    SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).TriggerHapticPulse(1000);
                }
            }
        }
    }

    /*
    * TriggerControl
    *
    * Move along the w axis by pressing the triggers on the controllers
    * The right trigger will move higher in the w axis and the left will move lower
    */
    void TriggerControl()
    {
        if (SteamVR_Controller.Input((int)gameObject.GetComponent<SteamVR_TrackedObject>().index).GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger))
        {
            if (gameObject.tag.Equals("RightControl"))
            {
                SlideAllHeldObjectsAlongW(1);
                player.WMove(1);
                callWMoveOnAllHyperScripts = true;
            }
            else {
                SlideAllHeldObjectsAlongW(-1);
                player.WMove(-1);
                callWMoveOnAllHyperScripts = true;
            }
        }
    }

    /*
	 * SlideAllHeldObjectsAlongW
	 * 
	 * Change the w position of all objects being held by either controller by deltaW
	 */
    void SlideAllHeldObjectsAlongW(int deltaW)
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
                otherControl.GetComponent<GetInputVR>().holding.GetComponent<HyperColliderManager>().SlideW(deltaW);
            }
        }
        else
            Debug.Log("CANNOT FIND OTHER CONTROLLER");

        if (holding)
            holding.GetComponent<HyperColliderManager>().SlideW(deltaW);

        callWMoveOnAllHyperScripts = true;
    }
}


