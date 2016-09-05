using UnityEngine;
using System.Collections;

public class BonsaiManager : MonoBehaviour {

	public int maxLeaves = 60;
	public int maxBranches = 45;

	public float growthCycleTime = 60.0f;
	public float treeSize = 0.25f;

	GameObject baseBranch;

	public CONTRACTLEVEL levelType = CONTRACTLEVEL.NONE;

	int numLeaves;
	int numBranches;
	int numDeadLeaves;
	int numDeadBranches;
	int numInfestedBranches;

	int[] hitboxCollisions;

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

		hitboxCollisions = new int[6];

		for(int i = 0; i < 6; i++)
			hitboxCollisions[i] = 0;

		//Name the tree
		this.gameObject.name = "BonsaiTree_" + ID;
		ID++;

		//Create the base branch
		baseBranch = Instantiate(Resources.Load("Bonsai/BranchPrefab"), transform) as GameObject;
		baseBranch.transform.localPosition = Vector3.zero;
		baseBranch.transform.localScale = new Vector3(treeSize, treeSize, treeSize);
		baseBranch.GetComponent<Branch>().setcanSnip(false);
		baseBranch.GetComponent<Branch>().setDepth(0);

		//baseBranch.GetComponent<Branch>().setWPosition(3);
		baseBranch.GetComponent<HyperColliderManager>().setW(3);
		baseBranch.GetComponent<HyperColliderManager>().WMove();

		baseBranch.GetComponent<Branch>().setManager(this.gameObject);

		baseBranch.GetComponent<Branch>().setupTreeForLevel(levelType);

		InvokeRepeating("processGrowthCycle", growthCycleTime, growthCycleTime);
	}
	
	// Update is called once per frame
	void Update () {
		//Testing code for growing the tree with the spacebar
		if(Input.GetKeyDown(KeyCode.Space)) {
			processGrowthCycle();
		}

		/*
		//Grow tree on a timer
		if(Time.time % growthCycleTime == 0) {
			processGrowthCycle();
		}
		*/

		//Debug.Log("Branches: N-" + numBranches + ", D-" + numDeadBranches + ", I-" + numInfestedBranches
		//			+ "; Leaves: N-" + numLeaves + ", D-" + numDeadLeaves);
	}

	void processGrowthCycle() {
		baseBranch.GetComponent<Branch>().processGrowthCycle();
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

	public int[] getHitboxCollisions() {
		return hitboxCollisions;
	}

	public void registerHitboxCollision(string dir) {
		if(dir != "None")
			Debug.Log("Tree component collided with " + dir);
		switch(dir) {
			case "Top":
				hitboxCollisions[0] += 1;
				break;
			case "Bottom":
				hitboxCollisions[1] += 1;
				break;
			case "North":
				hitboxCollisions[2] += 1;
				break;
			case "South":
				hitboxCollisions[3] += 1;
				break;
			case "East":
				hitboxCollisions[4] += 1;
				break;
			case "West":
				hitboxCollisions[5] += 1;
				break;
			default:
				break;
		}
	}

	public void registerHitboxExit(string dir) {
		if(dir != "None")
			Debug.Log("Tree component left hitbox " + dir);
		switch(dir) {
			case "Top":
				hitboxCollisions[0] -= 1;
				break;
			case "Bottom":
				hitboxCollisions[1] -= 1;
				break;
			case "North":
				hitboxCollisions[2] -= 1;
				break;
			case "South":
				hitboxCollisions[3] -= 1;
				break;
			case "East":
				hitboxCollisions[4] -= 1;
				break;
			case "West":
				hitboxCollisions[5] -= 1;
				break;
			default:
				break;
		}
	}
}
