using UnityEngine;
using System.Collections;

public class BonsaiShrine : MonoBehaviour {

	public GameObject LevelOneLights;
	public GameObject LevelTwoLights;
	public GameObject LevelThreeLights;
	public GameObject LevelFourLights;

	public GameObject[] trees;

    Transform shears;

	public CONTRACTLEVEL levelRequirement = CONTRACTLEVEL.NONE;

	int activationStage = 0;

	int[] issues;

	float checkTime;

	bool activated = false;
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

    KamiManager kamiManager;

    HyperCreature player;

    ParticleSystem particleObj;

    public enum CONTRACTLEVEL {
		NONE, TOKYO
	};

	void Awake() {
		linesActivated = true;

		checkTime = 2.0f;

		issues = new int[trees.Length];
		for(int i = 0; i < issues.Length; i++) {
			issues[i] = 0;
		}

		setActivationStage(0);

		initializeRequirementLines();
	}

	// Use this for initialization
	void Start () {
        kamiManager = KamiManager.instance;

        player = HyperCreature.instance;

        particleObj = GameObject.Find("BonsaiShrine/Particles").GetComponent<ParticleSystem>();

        shears = GameObject.Find("Shears").transform;

        var em = particleObj.emission;
        em.rate = 0;
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
        checkIfActivated(newStage);
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

    void checkIfActivated(int newStage){
        int oldStage = activationStage;
        activationStage = newStage;

        if (newStage > oldStage)
        {
            particleObj.Emit(20);

            for(int i = 0; i < newStage - oldStage; i++)
                Invoke("MakeKami", kamiManager.kamiArriveTime);
        }
        else if(newStage < oldStage)
        {
            for (int i = 0; i < oldStage - newStage; i++)
                ScareKami();
        }

        if (newStage == 4 && !activated)
        {
            activated = true;
            CancelInvoke();
            for (int i = 0; i < activationStage - kamiManager.NumberOfHappyKami(1); i++)
                Invoke("MakeKami", kamiManager.kamiArriveTime);
            var em = particleObj.emission;
            em.rate = 5;
            player.w_perif++;
            player.WMoveAllHyperObjects();
            InvokeRepeating("MakeKami", kamiManager.kamiArriveTime, kamiManager.kamiComeRate);
        }
        else if (newStage != 4 && activated)
        {
            activated = false;
            CancelInvoke();
            player.w_perif--;
            player.WMoveAllHyperObjects();
            InvokeRepeating("ScareKami", 0, kamiManager.kamiLeaveRate);
            var em = particleObj.emission;
            em.rate = 0;
        }
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
    bool checkContractProgress() {
		switch(levelRequirement) {
			case CONTRACTLEVEL.NONE:
				return true;
			case CONTRACTLEVEL.TOKYO:
				return checkTokyoContractProgress();
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
	bool checkTokyoContractProgress() {
		bool satisfied = true;

		foreach(GameObject tree in trees) {
			int[] reqZonePasses = tree.GetComponent<BonsaiManager>().getReqZonePasses();
			if(tree.GetComponent<BonsaiManager>().getZoneExtensions() > 0 ||
				reqZonePasses[0] <= 0 ||
				reqZonePasses[1] <= 0 ||
				reqZonePasses[2] <= 0) {
				satisfied = false;
			}
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
