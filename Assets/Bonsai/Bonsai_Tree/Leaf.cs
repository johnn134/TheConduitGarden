using UnityEngine;
using System.Collections;

public class Leaf : MonoBehaviour {

	GameObject manager; //The tree's bonsai manager

	int age;            //The age of this leaf

	bool zoneExtension;

	bool canSnip;       //Whether this leaf can be snipped or not
	bool isDead;       //Whether the leaf is alive or not

	const int minAcceptableCoverage = 1;  //Number of leaves allowed to overshadow this leaf each growth cycle 

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

	void OnDestroy() {
		if (transform.parent != null)
		{
			if (transform.parent.GetComponent<Branch>() != null)
			{
				transform.parent.GetComponent<Branch>().registerLeafRemoved();
			}
		}
		if(manager != null) {
			manager.GetComponent<BonsaiManager>().removeLeaf();

			if(isDead) {
				manager.GetComponent<BonsaiManager>().removeDeadLeaf();
			}

			if(zoneExtension)
				manager.GetComponent<BonsaiManager>().registerRemovalOfZoneExtension();
		}
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
			if(transform.parent.GetComponent<Branch>() != null) {
				transform.parent.GetComponent<Branch>().registerGrowthEnded();
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
		//Darken the leaf to show it is dead
		transform.GetChild(0).GetComponent<HyperObject>().dullCoef = 4;
		transform.GetChild(0).GetComponent<HyperObject>().WMove();

		isDead = true;
		manager.GetComponent<BonsaiManager>().addDeadLeaf();
	}

	/*
	 * Checks if the branch this leaf is on has no child branches
	 */
	bool checkForBranchTip() {
		return transform.parent.GetComponent<Branch>().getIsTip();
	}

	/*
	 * Checks if the branch this leaf is on is higher than its child branches
	 */
	bool checkIsParentBranchHigher() {
		return transform.parent.GetComponent<Branch>().getIsHigherThanChildren();
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
		RaycastHit[] rh = Physics.RaycastAll(transform.GetChild(1).position, Vector3.up, 100.0f);
		int counter = 0;
		foreach(RaycastHit r in rh) {
			if(r.transform.GetComponent<Leaf>() != null)
				counter++;
		}
		return counter;
	}

	/*
	 * Wrapper for checking if the leaf extends past a zone marker
	 * required of the contract
	 */
	public void checkIfLeafSatisfiesContract() {
		if(manager.GetComponent<BonsaiManager>() != null) {
			switch(manager.GetComponent<BonsaiManager>().levelType) {
				case BonsaiManager.CONTRACTLEVEL.NONE:

					break;
				case BonsaiManager.CONTRACTLEVEL.TOKYO:
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
		/*
		GameObject shrine = FindObjectOfType<BonsaiShrine>().gameObject;
		bool a = shrine.GetComponent<BonsaiShrine>().isPointInsideBoundingZone(transform.GetChild(1).position, manager);
		bool b = shrine.GetComponent<BonsaiShrine>().isPointInsideBoundingZone(transform.position, manager);
		if(!a || !b) {
			zoneExtension = true;
			if(manager.GetComponent<BonsaiManager>() != null)
				manager.GetComponent<BonsaiManager>().registerZoneExtension();
		}
		*/
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
	 * Sets the bonsai manager this leaf answers to
	 */
	public void setManager(GameObject newManager) {
		manager = newManager;
	}
}
