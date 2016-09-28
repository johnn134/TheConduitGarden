using UnityEngine;
using System.Collections;

public class SDLeaf : MonoBehaviour {

	GameObject manager; //The tree's bonsai manager

	int age;            //The age of this leaf
	int w;              //The position of this leaf on the fourth dimension
	//int deathTime;      //The age at which this leaf died

	//int depth = 0;                  //The depth of this leaf on the tree
	int minAcceptableCoverage = 1;  //Number of leaves allowed to overshadow this leaf each growth cycle

	float DARKEN_VALUE = 0.1f;
	float DARKEN_ALPHA = 0.75f;

	bool zoneExtension;

	bool canSnip;       //Whether this leaf can be snipped or not
	bool isDead;       //Whether the leaf is alive or not

	static int ID = 0;

	void Awake() {
		age = 0;
		canSnip = true;
		isDead = false;

		zoneExtension = false;

		//name the leaf
		this.gameObject.name = "Leaf_" + ID;
		ID++;
	}

	// Use this for initialization
	void Start() {

	}

	void OnDestroy() {
		if (transform.parent != null)
		{
			if (transform.parent.GetComponent<SDBranch>() != null)
			{
				transform.parent.GetComponent<SDBranch>().registerLeafRemoved();
			}
		}
		if(manager != null) {
			manager.GetComponent<SDBonsaiManager>().removeLeaf();

			if(isDead) {
				manager.GetComponent<SDBonsaiManager>().removeDeadLeaf();
			}

			if(zoneExtension)
				manager.GetComponent<SDBonsaiManager>().registerRemovalOfZoneExtension();
		}
	}

	// Update is called once per frame
	void Update() {

	}

	//Snips this leaf when clicked
	void OnMouseDown() {
		if(canSnip) {
			Destroy(this.gameObject);
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.transform.parent != null) {
			if(other.transform.parent.GetComponent<Shears>() != null) {
				if(canSnip) {
					Destroy(this.gameObject);
				}
			}
		}
	}

	/*
	 * Ages this leaf
	 */
	public void processGrowthCycle() {
		age++;

		//Check for the leaf to die if alive
		checkForDeath();

		//Tell parent branch that growth is over
		if(transform.parent != null) {
			if(transform.parent.GetComponent<SDBranch>() != null) {
				transform.parent.GetComponent<SDBranch>().registerGrowthEnded();
			}
		}
	}

	/*
	 * Checks if the leaf will remain alive
	 * Leaf will live if one of these is satisfied:
	 * - it is on a branch tip
	 * - its parent branch is higher than its child branches and
	 *   it has an acceptable number of leaves hanging above it
	 * - it is facing above the horizon and
	 *   it has an acceptable number of leaves hanging above it
	 */
	void checkForDeath() {
		if(!isDead) {
			if(!(checkForBranchTip() || ((checkIsParentBranchHigher() || checkFacingAboveTheHorizon()) && checkOverhangingLeaves() <= minAcceptableCoverage))) {   //Dying
				makeLeafDead();
			}
		}
	}

	public void makeLeafDead() {
		//Set the death time
		//deathTime = age;

		//Darken the leaf to show it is dead
		//Color oldColor = transform.GetChild(0).GetComponent<MeshRenderer>().material.color;
		//setVisualColor(new Color (oldColor.r * DARKEN_VALUE, oldColor.g * DARKEN_VALUE, oldColor.b * DARKEN_VALUE, DARKEN_ALPHA));
		transform.GetChild(0).GetComponent<HyperObject>().dullCoef = 4;
		transform.GetChild(0).GetComponent<HyperObject>().WMove();

		isDead = true;
		manager.GetComponent<SDBonsaiManager>().addDeadLeaf();
	}

	/*
	 * Checks if the branch this leaf is on has no child branches
	 */
	bool checkForBranchTip() {
		return transform.parent.GetComponent<SDBranch>().getIsTip();
	}

	/*
	 * Checks if the branch this leaf is on is higher than its child branches
	 */
	bool checkIsParentBranchHigher() {
		return transform.parent.GetComponent<SDBranch>().getIsHigherThanChildren();
	}

	/*
	 * Checks whether the leaf is facing above or below the horizon
	 */
	bool checkFacingAboveTheHorizon() {
		return Vector3.Dot(Vector3.up, transform.up) >= -0.05f;	//-0.05f allows for a slight amount of tilt down. Like 2.5 degrees
	}

	/*
	 * Checks if there are leaves above the face point on this leaf
	 */
	int checkOverhangingLeaves() {
		return Physics.RaycastAll(transform.GetChild(1).position, Vector3.up, 100.0f).Length;
	}

	/*
	 * Wrapper for checking if the leaf extends past a zone marker
	 * required of the contract
	 */
	public void checkIfLeafSatisfiesContract() {
		if(manager.GetComponent<SDBonsaiManager>() != null) {
			switch(manager.GetComponent<SDBonsaiManager>().levelType) {
				case SDBonsaiManager.CONTRACTLEVEL.NONE:

					break;
				case SDBonsaiManager.CONTRACTLEVEL.TOKYO:
					checkBoundsForTokyo();
					break;
				default:

					break;
			}
		}
	}

	/*
	 * Determines if the branch extends past the bounding zone for the Tokyo contract
	 */
	void checkBoundsForTokyo() {
		GameObject shrine = FindObjectOfType<SDBonsaiShrine>().gameObject;
		bool a = shrine.GetComponent<SDBonsaiShrine>().isPointInsideBoundingZone(transform.GetChild(1).position, manager);
		bool b = shrine.GetComponent<SDBonsaiShrine>().isPointInsideBoundingZone(transform.position, manager);
		if(!a || !b) {
			zoneExtension = true;
			//Debug.Log(gameObject.name + " extends past zone");
			if(manager.GetComponent<SDBonsaiManager>() != null)
				manager.GetComponent<SDBonsaiManager>().registerZoneExtension();
		}
	}

	/*
	 * Return isDead
	 */
	public bool getIsDead() {
		return isDead;
	}

	/*
	 * Sets whether this leaf can be snipped
	 */
	public void setcanSnip(bool canSnip) {
		this.canSnip = canSnip;
	}

	/*
	 * Sets the depth of this leaf
	 */
	//public void setDepth(int newDepth) {
	//	depth = newDepth;
	//}

	/*
	 * Sets the w position of this leaf and adjusts the color accordingly
	 */
	public void setWPosition(int newW) {
		w = newW;

		assignColorToWPosition();
	}

	/*
	 * Sets the visual color according to the w position
	 */
	void assignColorToWPosition() {
		float cModifier = 1.0f;
		float aModifier = 0.5f;

		if(isDead) {
			cModifier = DARKEN_VALUE;
			aModifier = DARKEN_ALPHA;
		}

		//Change Material value
		switch (w) {
			case 0:     //red
				setVisualColor(new Color (1.0f * cModifier, 0.0f, 0.0f, aModifier));
				break;
			case 1:     //orange
				setVisualColor(new Color (1.0f * cModifier, 0.5f * cModifier, 0.0f, aModifier));
				break;
			case 2:     //yellow
				setVisualColor(new Color (1.0f * cModifier, 1.0f * cModifier, 0.0f, aModifier));
				break;
			case 3:     //green
				setVisualColor(new Color (0.0f, 1.0f * cModifier, 0.0f, aModifier));
				break;
			case 4:     //blue
				setVisualColor(new Color (0.0f, 1.0f * cModifier, 1.0f * cModifier, aModifier));
				break;
			case 5:     //indigo
				setVisualColor(new Color (0.0f, 0.0f, 1.0f * cModifier, aModifier));
				break;
			case 6:     //violet
				setVisualColor(new Color (1.0f * cModifier, 0.0f, 1.0f * cModifier, aModifier));
				break;
		}
	}

	/*
	 * Changes the material color of the visual components of this leaf
	 */
	void setVisualColor(Color c) {
		transform.GetChild(0).GetComponent<MeshRenderer>().material.color = c;
	}

	/*
	 * Sets the bonsai manager this leaf answers to
	 */
	public void setManager(GameObject newManager) {
		manager = newManager;
	}
}
