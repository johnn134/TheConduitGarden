using UnityEngine;
using System.Collections;

public class BonsaiShrine : MonoBehaviour {

	public GameObject[] LevelLights_A, LevelLights_B;

	public GameObject[] trees;

	public CONTRACTLEVEL levelRequirement = CONTRACTLEVEL.NONE;

	Transform shears;

	KamiManager kamiManager;

	ParticleSystem particleObj_LA, particleObj_LB, particleObj_RA, particleObj_RB;

	int activationStage;

	int[] issues;

	float checkTime;

	bool fullyActivated;
	bool linesActivated;

	const int MIN_LEAVES = 4;
	const int MIN_BRANCHES = 5;

	const float CHECK_DELAY = 1.0f;
	const float INACTIVE_COLOR_VALUE = 0.1f;
	const float ACTIVE_COLOR_VALUE = 1.0f;
	const float LINE_WIDTH = 0.0025f;

	const float TOKYO_REQ_RADIUS = 0.025f;

	const float TOKYO_ZONE_A_OFFSET_X = 0.025f;
	const float TOKYO_ZONE_A_OFFSET_Y = 0.07f;
	const float TOKYO_ZONE_A_OFFSET_Z = 0.06f;

	const float TOKYO_ZONE_B_OFFSET_X = -0.046f;
	const float TOKYO_ZONE_B_OFFSET_Y = 0.125f;
	const float TOKYO_ZONE_B_OFFSET_Z = -0.016f;

	const float TOKYO_ZONE_C_OFFSET_X = 0.008f;
	const float TOKYO_ZONE_C_OFFSET_Y = 0.119f;
	const float TOKYO_ZONE_C_OFFSET_Z = -0.052f;

    public enum CONTRACTLEVEL {
		NONE, TOKYO
	};

	void Awake() {
		fullyActivated = false;
		linesActivated = true;

		checkTime = 2.0f;

		activationStage = 0;

		issues = new int[trees.Length];
		for(int i = 0; i < issues.Length; i++) {
			issues[i] = 0;
		}

		//Cache particle systems
		particleObj_LA = transform.GetChild(1).GetComponent<ParticleSystem>();
		particleObj_LB = transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>();

		particleObj_RA = transform.GetChild(2).GetComponent<ParticleSystem>();
		particleObj_RB = transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>();

		//Initialize lights to off
		setActivationStage(0, 0);

		//Create Requirement lines on trees
		initializeRequirementLines();
	}

	// Use this for initialization
	void Start () {
        kamiManager = KamiManager.instance;

        shears = GameObject.Find("Shears").transform;
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.L)) {
			if(linesActivated)
				deactivateBoundLines();
			else
				activateBoundLines();
		}

        if (shears.parent && !linesActivated)
            activateBoundLines();
        else if (!shears.parent && linesActivated)
            deactivateBoundLines();
	}

	void FixedUpdate() {
		//run tree check
		if(Time.time > checkTime) {
			checkTrees();
			checkTime = Time.time + CHECK_DELAY;
		}
	}

	/*
	 * Adds a linerenderer to the given object with the given positions
	 */
	void addBoundLine(GameObject obj, Vector3[] pos) {
		GameObject newLine = new GameObject("BoundLine");
		newLine.transform.position = obj.transform.position;
		newLine.transform.parent = obj.transform;
		newLine.AddComponent<LineRenderer>().SetVertexCount(pos.Length);
		newLine.GetComponent<LineRenderer>().material = Resources.Load("BonsaiShrine/BoundsMat") as Material;
		newLine.GetComponent<LineRenderer>().SetWidth(LINE_WIDTH, LINE_WIDTH);
		newLine.GetComponent<LineRenderer>().SetPositions(pos);
	}

	/*
	 * Set up the visual outline of the required zones
	 */
	void initializeRequirementLines() {
		foreach(GameObject tree in trees) {
			GameObject requiredLines = tree.transform.GetChild(1).gameObject;
			Vector3[] circlePos = new Vector3[17];
			Vector3 offset = tree.transform.position;

			//Zone A
			for(int i = 0; i < 17; i++) {
				float x = Mathf.Cos(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_REQ_RADIUS;
				float y = Mathf.Sin(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_REQ_RADIUS;

				circlePos[i] = new Vector3(x + offset.x + TOKYO_ZONE_A_OFFSET_X, y + offset.y + TOKYO_ZONE_A_OFFSET_Y, offset.z + TOKYO_ZONE_A_OFFSET_Z);
			}

			addRequiredLine(requiredLines, circlePos);

			//Zone B
			for(int i = 0; i < 17; i++) {
				float z = Mathf.Cos(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_REQ_RADIUS;
				float y = Mathf.Sin(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_REQ_RADIUS;

				circlePos[i] = new Vector3(offset.x + TOKYO_ZONE_B_OFFSET_X, y + offset.y + TOKYO_ZONE_B_OFFSET_Y, z + offset.z + TOKYO_ZONE_B_OFFSET_Z);
			}

			addRequiredLine(requiredLines, circlePos);

			//Zone C
			for(int i = 0; i < 17; i++) {
				float z = Mathf.Cos(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_REQ_RADIUS;
				float y = Mathf.Sin(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_REQ_RADIUS;

				circlePos[i] = new Vector3(offset.x + TOKYO_ZONE_C_OFFSET_X, y + offset.y + TOKYO_ZONE_C_OFFSET_Y, z + offset.z + TOKYO_ZONE_C_OFFSET_Z);
			}

			addRequiredLine(requiredLines, circlePos);

		}
	}

	void addRequiredLine(GameObject obj, Vector3[] pos) {
		GameObject newLine = new GameObject("RequirementLine");
		newLine.transform.position = obj.transform.position;
		newLine.transform.parent = obj.transform;
		newLine.AddComponent<LineRenderer>().SetVertexCount(pos.Length);
		newLine.GetComponent<LineRenderer>().material = Resources.Load("BonsaiShrine/RequiredMat") as Material;
		newLine.GetComponent<LineRenderer>().SetWidth(LINE_WIDTH, LINE_WIDTH);
		newLine.GetComponent<LineRenderer>().SetPositions(pos);
	}

	/*
	 * Sets the bounding zone lines for all trees to be enabled
	 */
	void activateBoundLines() {
		foreach(GameObject tree in trees) {
			foreach(Transform line in tree.transform.GetChild(0)) {
				line.GetComponent<LineRenderer>().enabled = true;
			}
			foreach(Transform line in tree.transform.GetChild(1)) {
				line.GetComponent<LineRenderer>().enabled = true;
			}
		}

		linesActivated = true;
	}

	/*
	 * Sets the bounding zone lines for all trees to be disabled
	 */
	void deactivateBoundLines() {
		foreach(GameObject tree in trees) {
			foreach(Transform line in tree.transform.GetChild(0)) {
				line.GetComponent<LineRenderer>().enabled = false;
			}
			foreach(Transform line in tree.transform.GetChild(1)) {
				line.GetComponent<LineRenderer>().enabled = false;
			}
		}

		linesActivated = false;
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
		bool contractSatisfiedA = true;
		bool contractSatisfiedB = true;

		//contract requirement check
		contractSatisfiedA = checkContractProgress(trees[0]);
		contractSatisfiedB = checkContractProgress(trees[1]);

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
			}
		}

		int stageA = (3 - issues[0]) + (contractSatisfiedA ? 1 : 0);
		int stageB = (3 - issues[1]) + (contractSatisfiedB ? 1 : 0);

		setActivationStage(stageA, stageB);
	}

	/*
	 * Sets the stage of activation for this shrine between 0 - 4 (inclusive)
	 */
	void setActivationStage(int newStageA, int newStageB) {
		activateLights(newStageA, LevelLights_A);
		activateLights(newStageB, LevelLights_B);
		checkForStageChange((int)((newStageA + newStageB) / 2));
	}

	/*
	 * Activates the lights up to the given level
	 * i.e. lightLevel 2 activates 1 and 2
	 */
	void activateLights(int lightLevel, GameObject[] lights) {
		Color active = Color.white * ACTIVE_COLOR_VALUE;
		Color inactive = Color.white * INACTIVE_COLOR_VALUE;

		lights[0].GetComponent<Renderer>().material.color = lightLevel >= 1 ? active : inactive;
		lights[1].GetComponent<Renderer>().material.color = lightLevel >= 2 ? active : inactive;
		lights[2].GetComponent<Renderer>().material.color = lightLevel >= 3 ? active : inactive;
		lights[3].GetComponent<Renderer>().material.color = lightLevel >= 4 ? active : inactive;
	}

	/*
	 * Provide feedback for reaching a higher or lower stage of activation
	 */
	void checkForStageChange(int newStage){
		//Check for new stage
		if (newStage > activationStage) {	//increase activation level
			particleObj_LA.Play();
			particleObj_LB.Play();
			particleObj_RA.Play();
			particleObj_RB.Play();

			for(int i = 0; i < newStage - activationStage; i++)
                Invoke("MakeKami", kamiManager.kamiArriveTime);
        }
		else if(newStage < activationStage) {	//decrease activation level
			for (int i = 0; i < activationStage - newStage; i++)
                ScareKami();
        }

		//Check for full activation
		if (newStage == 4 && !fullyActivated) {	//Turn on full activation
            CancelInvoke();	//clear invoked methods

			//Create remaining minimum Kami for final stage
			for (int i = 0; i < newStage - kamiManager.NumberOfHappyKami(1); i++)
				Invoke("MakeKami", kamiManager.kamiArriveTime);

			//Start repeating Kami creation
			InvokeRepeating("MakeKami", kamiManager.kamiArriveTime, kamiManager.kamiComeRate);

			//Start emmitting particles
			particleObj_LA.loop = true;
			particleObj_LB.loop = true;
			particleObj_RA.loop = true;
			particleObj_RB.loop = true;

			//Increase Peripheral vision

			fullyActivated = true;
        }
		else if (newStage != 4 && fullyActivated) {	//Turn of full activation
			CancelInvoke();	//clear invoked methods

			//Start repeating Kami Exits
			InvokeRepeating("ScareKami", 0, kamiManager.kamiLeaveRate);

			//Stop emmitting particles
			particleObj_LA.loop = false;
			particleObj_LB.loop = false;
			particleObj_RA.loop = false;
			particleObj_RB.loop = false;

			//Decrease Peripheral vision

			fullyActivated = false;
        }

		activationStage = newStage;
    }

    void MakeKami()
    {
        kamiManager.MakeKami(kamiManager.transform.position, transform.rotation, Random.Range(0, 7), 1);
    }

    void ScareKami()
    {
        //check if there are kami to scare away
        if (kamiManager.NumberOfHappyKami(1) > activationStage)
            kamiManager.MakeKamiFlee(1);
        else
        {
            //cancel all invoked make kami scrips and restart the ones that would not have been scared away
            CancelInvoke();

            for (int i = 0; i < activationStage - kamiManager.NumberOfHappyKami(1); i++)
                Invoke("MakeKami", kamiManager.kamiArriveTime);
        }
    }

    /*
	 * Wrapper for checking the progress of the bonsai trees in the contract level
	 */
	bool checkContractProgress(GameObject tree) {
		switch(levelRequirement) {
			case CONTRACTLEVEL.NONE:
				return true;
			case CONTRACTLEVEL.TOKYO:
				return checkTokyoContractProgress(tree);
			default:
				return false;
		}
	}

	/*
	 * Determines if the trees in the Tokyo contract level meet the requirements of the contract
	 * Requirements:
	 * - branches or leaves do NOT extend past the Top, Bottom, Lower or Upper bounds
	 * - there is at least 1 branch through each of the 5 required zones
	 */
	bool checkTokyoContractProgress(GameObject tree) {
		bool satisfied = true;

		int[] reqZonePasses = tree.GetComponent<BonsaiManager>().getReqZonePasses();
		if(tree.GetComponent<BonsaiManager>().getZoneExtensions() > 0 ||
			reqZonePasses[0] <= 0 ||
			reqZonePasses[1] <= 0 ||
			reqZonePasses[2] <= 0) {
			satisfied = false;
		}
		
		return satisfied;
	}

	/*
	 * returns true if the point is within the contract's allowed growing zone
	 */
	public bool isPointInsideBoundingZone(Vector3 point, GameObject tree) {
		bool inZone = true;

		return inZone;
	}

	/*
	 * returns true if the line between the given points
	 * intersects with required Zone A
	 */
	public bool passesThroughReqZoneA(Vector3 start, Vector3 end, GameObject tree) {
		float slopeXZ = (end.z - start.z) / (end.x - start.x);
		float slopeYZ = (end.z - start.z) / (end.y - start.y);

		float x = start.x + ((tree.transform.position.z + TOKYO_ZONE_A_OFFSET_Z - start.z) / slopeXZ);
		float y = start.y + ((tree.transform.position.z + TOKYO_ZONE_A_OFFSET_Z - start.z) / slopeYZ);
		float distance = Mathf.Sqrt(Mathf.Pow(x - (tree.transform.position.x + TOKYO_ZONE_A_OFFSET_X), 2) + Mathf.Pow(y - (tree.transform.position.y + TOKYO_ZONE_A_OFFSET_Y), 2));

		return distance < TOKYO_REQ_RADIUS && isPointBetween(tree.transform.position.z + TOKYO_ZONE_A_OFFSET_Z, start.z, end.z);
	}

	/*
	 * returns true if the line between the given points
	 * intersects with required Zone B
	 */
	public bool passesThroughReqZoneB(Vector3 start, Vector3 end, GameObject tree) {
		float slopeZX = (end.x - start.x) / (end.z - start.z);
		float slopeYX = (end.x - start.x) / (end.y - start.y);

		float z = start.z + ((tree.transform.position.x + TOKYO_ZONE_B_OFFSET_X - start.x) / slopeZX);
		float y = start.y + ((tree.transform.position.x + TOKYO_ZONE_B_OFFSET_X - start.x) / slopeYX);
		float distance = Mathf.Sqrt(Mathf.Pow(z - (tree.transform.position.z + TOKYO_ZONE_B_OFFSET_Z), 2) + Mathf.Pow(y - (tree.transform.position.y + TOKYO_ZONE_B_OFFSET_Y), 2));

		return distance < TOKYO_REQ_RADIUS && isPointBetween(tree.transform.position.x + TOKYO_ZONE_B_OFFSET_X, start.x, end.x);
	}

	/*
	 * returns true if the line between the given points
	 * intersects with required Zone C
	 */
	public bool passesThroughReqZoneC(Vector3 start, Vector3 end, GameObject tree) {
		float slopeZX = (end.x - start.x) / (end.z - start.z);
		float slopeYX = (end.x - start.x) / (end.y - start.y);

		float z = start.z + ((tree.transform.position.x + TOKYO_ZONE_C_OFFSET_X - start.x) / slopeZX);
		float y = start.y + ((tree.transform.position.x + TOKYO_ZONE_C_OFFSET_X - start.x) / slopeYX);
		float distance = Mathf.Sqrt(Mathf.Pow(z - (tree.transform.position.z + TOKYO_ZONE_C_OFFSET_Z), 2) + Mathf.Pow(y - (tree.transform.position.y + TOKYO_ZONE_C_OFFSET_Y), 2));

		return distance < TOKYO_REQ_RADIUS && isPointBetween(tree.transform.position.x + TOKYO_ZONE_C_OFFSET_X, start.x, end.x);
	}

	/*
	 * Returns true if the given value is between the start and end points
	 */
	bool isPointBetween(float val, float start, float end) {
		return (val < start && val > end) || (val > start && val < end);
	}
}
