using UnityEngine;
using System.Collections;

public class BonsaiShrine : MonoBehaviour {

	public GameObject LevelOneLights;
	public GameObject LevelTwoLights;
	public GameObject LevelThreeLights;
	public GameObject LevelFourLights;

	public GameObject[] trees;

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
	const float TOKYO_LOWEST_HEIGHT = -0.01f;
	const float TOKYO_BASE_RADIUS = 0.1f;
	const float TOKYO_BASE_HEIGHT = 0.1f;
	const float TOKYO_TOP_RADIUS = 0.05f;
	const float TOKYO_TOP_HEIGHT = 0.25f;
	const float TOKYO_REQ_TOP_HEIGHT = 0.2f;
	const float TOKYO_REQ_TOP_RADIUS = 0.025f;
	const float TOKYO_REQ_BASE_HEIGHT = 0.05f;
	const float TOKYO_REQ_BASE_RADIUS = 0.025f;
	const float TOKYO_REQ_BASE_OFFSET = 0.05f;

    KamiManager kamiManager;

    HyperCreature player;

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

		initializeBoundingLines();
		initializeRequirementLines();
	}

	// Use this for initialization
	void Start () {
        kamiManager = KamiManager.instance;

        player = HyperCreature.instance;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.L)) {
			if(linesActivated)
				deactivateBoundLines();
			else
				activateBoundLines();
		}
	}

	void FixedUpdate() {
		//run tree check
		if(Time.time > checkTime) {
			checkTrees();
			checkTime = Time.time + CHECK_DELAY;
		}
	}

	/*
	 * Set up the visual outline of the bounding zone
	 */
	void initializeBoundingLines() {
		foreach(GameObject tree in trees) {
			GameObject boundLines = tree.transform.GetChild(0).gameObject;
			Vector3[] circlePos = new Vector3[17];
			Vector3[] linePos = new Vector3[2];
			Vector3 offset = tree.transform.position;

			//Base Circle
			for(int i = 0; i < 17; i++) {
				float x = Mathf.Cos(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_BASE_RADIUS;
				float z = Mathf.Sin(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_BASE_RADIUS;

				circlePos[i] = new Vector3(x + offset.x, TOKYO_LOWEST_HEIGHT + offset.y, z + offset.z);
			}

			addBoundLine(boundLines, circlePos);

			//Bottom Lines
			for(int i = 0; i < 8; i++) {
				float x = Mathf.Cos(Mathf.Deg2Rad * ((float)i * (360f / 8f))) * TOKYO_BASE_RADIUS;
				float z = Mathf.Sin(Mathf.Deg2Rad * ((float)i * (360f / 8f))) * TOKYO_BASE_RADIUS;

				linePos[0] = new Vector3(x + offset.x, TOKYO_LOWEST_HEIGHT + offset.y, z + offset.z);
				linePos[1] = new Vector3(x + offset.x, TOKYO_BASE_HEIGHT + offset.y, z + offset.z);

				addBoundLine(boundLines, linePos);
			}

			//Middle Circle
			for(int i = 0; i < 17; i++) {
				float x = Mathf.Cos(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_BASE_RADIUS;
				float z = Mathf.Sin(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_BASE_RADIUS;

				circlePos[i] = new Vector3(x + offset.x, TOKYO_BASE_HEIGHT + offset.y, z + offset.z);
			}

			addBoundLine(boundLines, circlePos);

			//Top Lines
			for(int i = 0; i < 8; i++) {
				float x1 = Mathf.Cos(Mathf.Deg2Rad * ((float)i * (360f / 8f))) * TOKYO_BASE_RADIUS;
				float z1 = Mathf.Sin(Mathf.Deg2Rad * ((float)i * (360f / 8f))) * TOKYO_BASE_RADIUS;

				float x2 = Mathf.Cos(Mathf.Deg2Rad * ((float)i * (360f / 8f))) * TOKYO_TOP_RADIUS;
				float z2 = Mathf.Sin(Mathf.Deg2Rad * ((float)i * (360f / 8f))) * TOKYO_TOP_RADIUS;

				linePos[0] = new Vector3(x1 + offset.x, TOKYO_BASE_HEIGHT + offset.y, z1 + offset.z);
				linePos[1] = new Vector3(x2 + offset.x, TOKYO_TOP_HEIGHT + offset.y, z2 + offset.z);

				addBoundLine(boundLines, linePos);
			}

			//Top Circle
			for(int i = 0; i < 17; i++) {
				float x = Mathf.Cos(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_TOP_RADIUS;
				float z = Mathf.Sin(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_TOP_RADIUS;

				circlePos[i] = new Vector3(x + offset.x, TOKYO_TOP_HEIGHT + offset.y, z + offset.z);
			}

			addBoundLine(boundLines, circlePos);
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
			Vector3[] linePos = new Vector3[2];
			Vector3 offset = tree.transform.position;

			//Top Zone
			for(int i = 0; i < 17; i++) {
				float x = Mathf.Cos(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_REQ_TOP_RADIUS;
				float z = Mathf.Sin(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_REQ_TOP_RADIUS;

				circlePos[i] = new Vector3(x + offset.x, TOKYO_REQ_TOP_HEIGHT + offset.y, z + offset.z);
			}

			addRequiredLine(requiredLines, circlePos);

			//North Zone - north is +z
			for(int i = 0; i < 17; i++) {
				float x = Mathf.Cos(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_REQ_BASE_RADIUS;
				float y = Mathf.Sin(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_REQ_BASE_RADIUS;

				circlePos[i] = new Vector3(x + offset.x, TOKYO_REQ_BASE_HEIGHT + y + offset.y, TOKYO_REQ_BASE_OFFSET + offset.z);
			}

			addRequiredLine(requiredLines, circlePos);


			//East Zone - east is +x
			for(int i = 0; i < 17; i++) {
				float z = Mathf.Cos(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_REQ_BASE_RADIUS;
				float y = Mathf.Sin(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_REQ_BASE_RADIUS;

				circlePos[i] = new Vector3(TOKYO_REQ_BASE_OFFSET + offset.x, TOKYO_REQ_BASE_HEIGHT + y + offset.y, z + offset.z);
			}

			addRequiredLine(requiredLines, circlePos);


			//South Zone - south is -z
			for(int i = 0; i < 17; i++) {
				float x = Mathf.Cos(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_REQ_BASE_RADIUS;
				float y = Mathf.Sin(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_REQ_BASE_RADIUS;

				circlePos[i] = new Vector3(x + offset.x, TOKYO_REQ_BASE_HEIGHT + y + offset.y, -TOKYO_REQ_BASE_OFFSET + offset.z);
			}

			addRequiredLine(requiredLines, circlePos);


			//West Zone - west is -x
			for(int i = 0; i < 17; i++) {
				float z = Mathf.Cos(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_REQ_BASE_RADIUS;
				float y = Mathf.Sin(Mathf.Deg2Rad * ((float)i * (360f / 16f))) * TOKYO_REQ_BASE_RADIUS;

				circlePos[i] = new Vector3(-TOKYO_REQ_BASE_OFFSET + offset.x, TOKYO_REQ_BASE_HEIGHT + y + offset.y, z + offset.z);
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
            //particleObj.Emit(20);

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
            //var em = particleObj.emission;
            //em.rate = 5;
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
            //var em = particleObj.emission;
            //em.rate = 0;
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
				reqZonePasses[2] <= 0 ||
				reqZonePasses[3] <= 0 ||
				reqZonePasses[4] <= 0) {
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

		if(point.y < tree.transform.position.y + TOKYO_LOWEST_HEIGHT ||
			point.y > tree.transform.position.y + TOKYO_TOP_HEIGHT) {	//Above or below
			inZone = false;
			//Debug.Log("height issue");
		}
		else {
			Vector3 pointPos = new Vector3(point.x, 0.0f, point.z);
			Vector3 treePos = new Vector3(tree.transform.position.x, 0.0f, tree.transform.position.z);

			if(point.y < tree.transform.position.y + TOKYO_BASE_HEIGHT) {	//Below Median
				if(Vector3.Distance(pointPos, treePos) > TOKYO_BASE_RADIUS) {
					inZone = false;
					//Debug.Log("base issue");
				}
			}
			else {	//Above Median
				float angle = Mathf.Atan((TOKYO_TOP_HEIGHT - TOKYO_BASE_HEIGHT) /
										 (TOKYO_BASE_RADIUS - TOKYO_TOP_RADIUS));
				float coneHeight = Mathf.Tan(angle) * TOKYO_BASE_RADIUS;
				float pointRadius = (coneHeight - (point.y - (tree.transform.position.y + TOKYO_BASE_HEIGHT))) / Mathf.Tan(angle);

				if(Vector3.Distance(pointPos, treePos) > pointRadius) {
					inZone = false;
					//Debug.Log("top issue");
				}
			}
		}

		return inZone;
	}

	/*
	 * returns true if the line between the given points
	 * intersects with the required top zone
	 * Top = +y
	 */
	public bool passesThroughReqTopZone(Vector3 start, Vector3 end, GameObject tree) {
		float slopeXY = (end.y - start.y) / (end.x - start.x);
		float slopeZY = (end.y - start.y) / (end.z - start.z);

		float x = start.x - ((tree.transform.position.y + TOKYO_REQ_TOP_HEIGHT - start.y) / slopeXY);
		float z = start.z - ((tree.transform.position.y + TOKYO_REQ_TOP_HEIGHT - start.y) / slopeZY);
		float distance = Mathf.Sqrt(Mathf.Pow(x - tree.transform.position.x, 2) + Mathf.Pow(z - tree.transform.position.z, 2));

		return distance < TOKYO_REQ_TOP_RADIUS && isPointBetween(tree.transform.position.y + TOKYO_REQ_TOP_HEIGHT, start.y, end.y);
	}

	/*
	 * returns true if the line between the given points
	 * intersects with the required north zone
	 * North = +z
	 */
	public bool passesThroughReqNorthZone(Vector3 start, Vector3 end, GameObject tree) {
		float slopeXZ = (end.z - start.z) / (end.x - start.x);
		float slopeYZ = (end.z - start.z) / (end.y - start.y);

		float x = start.x - ((tree.transform.position.z + TOKYO_REQ_BASE_OFFSET - start.z) / slopeXZ);
		float y = start.y - ((tree.transform.position.z + TOKYO_REQ_BASE_OFFSET - start.z) / slopeYZ);
		float distance = Mathf.Sqrt(Mathf.Pow(x - tree.transform.position.x, 2) + Mathf.Pow(y - (tree.transform.position.y + TOKYO_REQ_BASE_HEIGHT), 2));

		return distance < TOKYO_REQ_BASE_RADIUS && isPointBetween(tree.transform.position.z + TOKYO_REQ_BASE_OFFSET, start.z, end.z);
	}

	/*
	 * returns true if the line between the given points
	 * intersects with the required east zone
	 * East = +x
	 */
	public bool passesThroughReqEastZone(Vector3 start, Vector3 end, GameObject tree) {
		float slopeXY = (end.x - start.x) / (end.y - start.y);
		float slopeXZ = (end.x - start.x) / (end.z - start.z);

		float y = start.y - ((tree.transform.position.x + TOKYO_REQ_BASE_OFFSET - start.x) / slopeXY);
		float z = start.z - ((tree.transform.position.x + TOKYO_REQ_BASE_OFFSET - start.x) / slopeXZ);
		float distance = Mathf.Sqrt(Mathf.Pow(z - tree.transform.position.z, 2) + Mathf.Pow(y - (tree.transform.position.y + TOKYO_REQ_BASE_HEIGHT), 2));

		return distance < TOKYO_REQ_BASE_RADIUS && isPointBetween(tree.transform.position.x + TOKYO_REQ_BASE_OFFSET, start.x, end.x);
	}

	/*
	 * returns true if the line between the given points
	 * intersects with the required south zone
	 * South = -z
	 */
	public bool passesThroughReqSouthZone(Vector3 start, Vector3 end, GameObject tree) {
		float slopeXZ = (end.z - start.z) / (end.x - start.x);
		float slopeYZ = (end.z - start.z) / (end.y - start.y);

		float x = start.x - ((tree.transform.position.z - TOKYO_REQ_BASE_OFFSET - start.z) / slopeXZ);
		float y = start.y - ((tree.transform.position.z - TOKYO_REQ_BASE_OFFSET - start.z) / slopeYZ);
		float distance = Mathf.Sqrt(Mathf.Pow(x - tree.transform.position.x, 2) + Mathf.Pow(y - (tree.transform.position.y + TOKYO_REQ_BASE_HEIGHT), 2));

		return distance < TOKYO_REQ_BASE_RADIUS && isPointBetween(tree.transform.position.z - TOKYO_REQ_BASE_OFFSET, start.z, end.z);
	}

	/*
	 * returns true if the line between the given points
	 * intersects with the required west zone
	 * West = -x
	 */
	public bool passesThroughReqWestZone(Vector3 start, Vector3 end, GameObject tree) {
		float slopeXY = (end.x - start.x) / (end.y - start.y);
		float slopeXZ = (end.x - start.x) / (end.z - start.z);

		float y = start.y - ((tree.transform.position.x - TOKYO_REQ_BASE_OFFSET - start.x) / slopeXY);
		float z = start.z - ((tree.transform.position.x - TOKYO_REQ_BASE_OFFSET - start.x) / slopeXZ);
		float distance = Mathf.Sqrt(Mathf.Pow(z - tree.transform.position.z, 2) + Mathf.Pow(y - (tree.transform.position.y + TOKYO_REQ_BASE_HEIGHT), 2));

		return distance < TOKYO_REQ_BASE_RADIUS && isPointBetween(tree.transform.position.x - TOKYO_REQ_BASE_OFFSET, start.x, end.x);
	}

	/*
	 * Returns true if the given value is between the start and end points
	 */
	bool isPointBetween(float val, float start, float end) {
		return (val < start && val > end) || (val > start && val < end);
	}
}
