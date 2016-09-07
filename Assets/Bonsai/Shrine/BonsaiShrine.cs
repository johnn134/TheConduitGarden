using UnityEngine;
using System.Collections;

public class BonsaiShrine : MonoBehaviour {

	public GameObject LevelOneLights;
	public GameObject LevelTwoLights;
	public GameObject LevelThreeLights;
	public GameObject LevelFourLights;

	public GameObject[] trees;

	public CONTRACTLEVEL levelRequirement = CONTRACTLEVEL.NONE;

	int activationStage;

	int[] issues;

	float checkTime;

	bool activated;

	const int MIN_LEAVES = 4;
	const int MIN_BRANCHES = 5;

	const float CHECK_DELAY = 1.0f;
	const float INACTIVE_COLOR_VALUE = 0.1f;
	const float ACTIVE_COLOR_VALUE = 1.0f;

	public enum CONTRACTLEVEL {
		NONE, TOKYO
	};

	// Use this for initialization
	void Start () {
		checkTime = 2.0f;

		issues = new int[trees.Length];
		for(int i = 0; i < issues.Length; i++) {
			issues[i] = 0;
		}

		setActivationStage(0);
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate() {
		//run tree check
		if(Time.time > checkTime) {
			checkTrees();
			checkTime = Time.time + CHECK_DELAY;
		}
	}

	/*
	 * Determines the state of the bonsai trees in the garden
	 * A tree is in poor health if:
	 * - Dead leaves or branches
	 * - Infested
	 * - Overgrown
	 * 
	 * Requirements for stages:
	 * 0 - Every tree must have at most 3 issues
	 * 1 - Every tree must have at most 2 issues
	 * 2 - Every tree must have at most 1 issue
	 * 3 - All trees have no issues
	 * 4 - All trees meet the specific requirement of the contract
	 */
	void checkTrees() {
		bool contractSatisfied = true;

		for(int i = 0; i < trees.Length; i++) {
			GameObject g = trees[i];
			issues[i] = 0;

			if(g.GetComponent<BonsaiManager>() != null) {
				//Overgrown check
				if(g.GetComponent<BonsaiManager>().getNumLeaves() >= g.GetComponent<BonsaiManager>().maxLeaves ||
				   g.GetComponent<BonsaiManager>().getNumBranches() >= g.GetComponent<BonsaiManager>().maxBranches) {
					issues[i] += 1;
				}

				//Infestation check
				if(g.GetComponent<BonsaiManager>().getNumInfestedBranches() > 0) {
					issues[i] += 1;
				}

				//Dead leaves and branches check
				if(g.GetComponent<BonsaiManager>().getNumDeadLeaves() > 0 || 
				   g.GetComponent<BonsaiManager>().getNumDeadBranches() > 0) {
					issues[i] += 1;
				}
					
				//Undergrowth check
				if(g.GetComponent<BonsaiManager>().getNumLeaves() < MIN_LEAVES ||
				   g.GetComponent<BonsaiManager>().getNumBranches() < MIN_BRANCHES) {
					issues[i] = 3;	//undergrowth turns off the shrine
				}

				//contract requirement check
				contractSatisfied = checkContractProgress();	/*** Change this to work with the required contract ***/
			}
		}

		int stage = 4;

		for(int i = 0; i < issues.Length; i++) {
			int val = 3 - issues[i];
			val += contractSatisfied ? 1 : 0;

			stage = Mathf.Min(val, stage);
		}

		setActivationStage(stage);
	}

	/*
	 * Sets the stage of activation for this shrine between 0 - 4 (inclusive)
	 */
	void setActivationStage(int newStage) {
		activateLights(newStage);
	}

	/*
	 * Activates the lights up to the given level
	 * i.e. lightLevel 2 activates 1 and 2
	 */
	void activateLights(int lightLevel) {
		Color active = Color.white * ACTIVE_COLOR_VALUE;
		Color inactive = Color.white * INACTIVE_COLOR_VALUE;

		LevelOneLights.GetComponent<Renderer>().material.color = lightLevel >= 1 ? active : inactive;
		LevelTwoLights.GetComponent<Renderer>().material.color = lightLevel >= 2 ? active : inactive;
		LevelThreeLights.GetComponent<Renderer>().material.color = lightLevel >= 3 ? active : inactive;
		LevelFourLights.GetComponent<Renderer>().material.color = lightLevel >= 4 ? active : inactive;
	}

	/*
	 * Wrapper for checking the progress of the bonsai trees in the contract level
	 */
	bool checkContractProgress() {
		switch(levelRequirement) {
			case CONTRACTLEVEL.NONE:
				return true;
				break;
			case CONTRACTLEVEL.TOKYO:
				return checkTokyoContractProgress();
				break;
			default:
				return false;
				break;
		}
	}

	/*
	 * Determines if the trees in the Tokyo contract level meet the requirements of the contract
	 * Requirements:
	 * - branches or leaves do NOT extend past the Top, Bottom, Lower or Upper bounds
	 */
	bool checkTokyoContractProgress() {
		bool satisfied = true;

		for(int i = 0; i < trees.Length; i++) {
			for(int j = 0; j < 10; j++) {
				if(trees[i].GetComponent<BonsaiManager>().getHitboxCollisions()[j] > 0) {
					satisfied = false;
				}
			}
		}

		return satisfied;
	}
}
