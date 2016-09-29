using UnityEngine;
using System.Collections;

public class ScrollDrag : MonoBehaviour {

	public GameObject target;

	public SCROLLAXIS scrollType;

	bool grabbed;

	const float UPPER_LIMIT = 0.9f;
	const float LOWER_LIMIT = -0.95f;

	public enum SCROLLAXIS {
		X, Y, Z
	};

	// Use this for initialization
	void Start () {
		grabbed = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(grabbed)
			dragScroll(target);
	}

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.StartsWith("Controller"))
        {
            GetInputVR controller = other.GetComponent<GetInputVR>();

            if (controller.griping)
            {
                if(!grabbed)
                    grabScroll(controller.gameObject);
            }
            else if(grabbed)
                dropScroll();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.StartsWith("Controller"))
        {
            if(grabbed)
                dropScroll();
        }
    }

	/*
	 * Assigns the scroll to be grabbed by the player,
	 * if the scroll is currently active
	 * Inactive scrolls should remain closed and locked
	 */
	public bool grabScroll(GameObject newTarget) {
		//if(transform.parent.GetComponent<ContractScroll>().isActive) {
			grabbed = true;

			target = newTarget;

			return true;
		//}
		//return false;
	}

	/*
	 * Stops tracking the player's controller and closes the scroll
	 */
	public void dropScroll() {
		grabbed = false;

		if(transform.parent.GetComponent<ContractScroll>().isActive)
			transform.parent.GetComponent<ContractScroll>().closeScroll();
		else
			transform.parent.GetComponent<ContractScroll>().openScroll();
	}

	/*
	 * When the player grabs the scroll handle ring,
	 * the scroll will follow the y position of the controller within
	 * a limited range of movement
	 */
	void dragScroll(GameObject other) {
		if(!transform.parent.GetComponent<ContractScroll>().getIsScrolling()) {
			//Get a few values for ease of reading
			Transform visualBottom = transform.parent.GetChild(0).GetChild(1);
			Vector3 visualTopPos = transform.parent.GetChild(0).GetChild(0).position;
			Vector3 otherPos = other.transform.position;
			float offset;

			switch(scrollType) {
				case SCROLLAXIS.Y:
					offset = visualBottom.position.y - transform.position.y;	//between bottom visual and drag point

					//reposition the visual of the bottom of the scroll object
					visualBottom.position = new Vector3(visualBottom.position.x, 
						Mathf.Clamp(otherPos.y + offset, 
							visualTopPos.y + LOWER_LIMIT * transform.parent.lossyScale.y, 
							visualTopPos.y + UPPER_LIMIT * transform.parent.lossyScale.y), 
						visualBottom.position.z);
					
					//reposition this point
					transform.position = new Vector3(transform.position.x, 
						Mathf.Clamp(otherPos.y, 
							visualTopPos.y + LOWER_LIMIT * transform.parent.lossyScale.y - offset, 
							visualTopPos.y + UPPER_LIMIT * transform.parent.lossyScale.y - offset), 
						transform.position.z);
					
					break;
				case SCROLLAXIS.X:
					offset = visualBottom.position.x - transform.position.x;	//between bottom visual and drag point

					//reposition the visual of the bottom of the scroll object
					visualBottom.position = new Vector3(Mathf.Clamp(otherPos.x + offset, 
							visualTopPos.x + LOWER_LIMIT * transform.parent.lossyScale.y, 
							visualTopPos.x + UPPER_LIMIT * transform.parent.lossyScale.y), 
						visualBottom.position.y, 
						visualBottom.position.z);

					//reposition this point
					transform.position = new Vector3(Mathf.Clamp(otherPos.x, 
							visualTopPos.x + LOWER_LIMIT * transform.parent.lossyScale.y - offset, 
							visualTopPos.x + UPPER_LIMIT * transform.parent.lossyScale.y - offset),
						transform.position.y,
						transform.position.z);

					break;
				case SCROLLAXIS.Z:
					offset = visualBottom.position.z - transform.position.z;	//between bottom visual and drag point

					//reposition the visual of the bottom of the scroll object
					visualBottom.position = new Vector3(visualBottom.position.x, 
						visualBottom.position.y,
						Mathf.Clamp(otherPos.x + offset, 
							visualTopPos.z + LOWER_LIMIT * transform.parent.lossyScale.y, 
							visualTopPos.z + UPPER_LIMIT * transform.parent.lossyScale.y));

					//reposition this point
					transform.position = new Vector3(visualBottom.position.x, 
						visualBottom.position.y,
						Mathf.Clamp(otherPos.x + offset, 
							visualTopPos.z + LOWER_LIMIT * transform.parent.lossyScale.y - offset, 
							visualTopPos.z + UPPER_LIMIT * transform.parent.lossyScale.y - offset));

					break;
			}
		}								 
	}
}
