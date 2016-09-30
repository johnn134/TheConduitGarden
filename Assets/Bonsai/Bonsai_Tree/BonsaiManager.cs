using UnityEngine;
using System.Collections;

public class BonsaiManager : MonoBehaviour {

	public int maxLeaves = 60;
	public int maxBranches = 45;

	public float growthCycleTime = 60.0f;
	public float treeSize = 0.25f;

	public CONTRACTLEVEL levelType = CONTRACTLEVEL.NONE;

	GameObject baseBranch;

	int numLeaves;
	int numBranches;
	int numDeadLeaves;
	int numDeadBranches;
	int numInfestedBranches;

	int zoneExtensions;			//The number of tree components extending past the contract bounding zone

	int[] requiredZonePasses;

	static int ID = 0;

	public enum CONTRACTLEVEL {
		NONE, TOKYO
	};

	// Use this for initialization
	void Start () {
		//initialize variables
		numLeaves = 0;
		numBranches = 1;	//1 for the base branch
		numDeadLeaves = 0;
		numDeadBranches = 0;
		numInfestedBranches = 0;

		zoneExtensions = 0;

		requiredZonePasses = new int[3];

		for(int i = 0; i < 3; i++) {
			requiredZonePasses[i] = 0;
		}

		//Name the tree
		this.gameObject.name = "BonsaiTree_" + ID;
		ID++;

		//Create the base branch
		baseBranch = Instantiate(Resources.Load("Bonsai/BranchPrefab"), transform) as GameObject;
		baseBranch.transform.localPosition = Vector3.zero;
		baseBranch.transform.localScale = new Vector3(treeSize, treeSize, treeSize);
		baseBranch.GetComponent<Branch>().setcanSnip(false);
		baseBranch.GetComponent<Branch>().setDepth(0);
		baseBranch.GetComponent<Branch>().setManager(this.gameObject);
		baseBranch.GetComponent<Branch>().checkIfBranchSatisfiesContract();

		baseBranch.GetComponent<HyperColliderManager>().setW(2);

		baseBranch.GetComponent<Branch>().setupTreeForLevel(levelType);

		InvokeRepeating("processGrowthCycle", growthCycleTime, growthCycleTime);
	}
	
	// Update is called once per frame
	void Update () {
		//Testing code for growing the tree with the spacebar
		if(Input.GetKeyDown(KeyCode.Space)) {
			processGrowthCycle();
		}
	}

	/*
	 * Initiate the growth cycle for the entire tree
	 */
	void processGrowthCycle() {
		baseBranch.GetComponent<Branch>().processGrowthCycle();
	}

	/*
	 * Increments the counter for the number of tree components
	 * extending past the bounding zone
	 */
	public void registerZoneExtension() {
		zoneExtensions += 1;
	}

	/*
	 * Decrements the counter for the number of tree components
	 * extending past the bounding zone
	 */
	public void registerRemovalOfZoneExtension() {
		zoneExtensions -= 1;
	}

	/*
	 * Increments the counter for the number of tree components
	 * passing through required zones
	 */
	public void registerReqZonePasses(int[] zones) {
		for(int i = 0; i < 3; i++) {
			requiredZonePasses[i] += zones[i];
		}
	}

	/*
	 * Decrements the counter for the number of tree components
	 * passing through required zones
	 */
	public void registerRemovalOfReqZonePasses(int[] zones) {
		for(int i = 0; i < 3; i++) {
			requiredZonePasses[i] -= zones[i];
		}
	}

	public void addLeaf() {
		numLeaves++;
	}

	public void removeLeaf() {
		numLeaves--;
	}

	public void addBranch() {
		numBranches++;
	}

	public void removeBranch() {
		numBranches--;
	}

	public void addDeadLeaf() {
		numDeadLeaves++;
	}

	public void removeDeadLeaf() {
		numDeadLeaves--;
	}

	public void addDeadBranch() {
		numDeadBranches++;
	}

	public void removeDeadBranch() {
		numDeadBranches--;
	}

	public void addInfestedBranch() {
		numInfestedBranches++;
	}

	public void removeInfestedBranch() {
		numInfestedBranches--;
	}

	public bool canMakeLeaf() {
		return numLeaves < maxLeaves;
	}

	public bool canMakeBranch() {
		return numBranches < maxBranches;
	}

	public int getNumLeaves() {
		return numLeaves;
	}

	public int getNumBranches() {
		return numBranches;
	}

	public int getNumInfestedBranches() {
		return numInfestedBranches;
	}

	public int getNumDeadLeaves() {
		return numDeadLeaves;
	}

	public int getNumDeadBranches() {
		return numDeadBranches;
	}

	public int getZoneExtensions() {
		return zoneExtensions;
	}

	public int[] getReqZonePasses() {
		return requiredZonePasses;
	}
}
