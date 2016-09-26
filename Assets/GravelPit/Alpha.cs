using UnityEngine;
using System.Collections;

public class Alpha : MonoBehaviour {
	public GameObject rake;
	public GameObject gravel;
	public GameObject tine1;
	public Vector3 tine1Position;
	public bool tine1Collision;
	public GameObject tine2;
	public Vector3 tine2Position;
	public bool tine2Collision;
	public GameObject tine3;
	public Vector3 tine3Position;
	public bool tine3Collision;
	public GameObject tine4;
	public Vector3 tine4Position;
	public bool tine4Collision;
	public Texture2D texture;
	public Color color;
	public Color originalColor;
	public float drawingSizeX;
	public float drawingSizeZ;
	public int[,] coordinates;
	public int raked;
	public bool isRakedEnough;
	public int originalThird;
	public int tine1Nodes;
	public int tine2Nodes;
	public int tine3Nodes;
	public int tine4Nodes;
	public GameObject drawnNode;
	public int nodeNumber;
	public GameObject currentTine1Line;
	public GameObject currentTine2Line;
	public GameObject currentTine3Line;
	public GameObject currentTine4Line;
	public GravelShrine gravelShrine;
	public HyperCreature player;
	public HyperObject myHyper;

	// Use this for initialization
	void Start() {
		player = HyperCreature.instance;
		myHyper = GetComponent<HyperObject>();
		rake = GameObject.FindGameObjectWithTag("Rake");
		tine1 = GameObject.FindGameObjectWithTag("Tine 1");
		tine2 = GameObject.FindGameObjectWithTag("Tine 2");
		tine3 = GameObject.FindGameObjectWithTag("Tine 3");
		tine4 = GameObject.FindGameObjectWithTag("Tine 4");
		gravel = GameObject.FindGameObjectWithTag ("Gravel");
		gravelShrine = Object.FindObjectOfType<GravelShrine>();

		GetComponent<Renderer>().material.mainTexture = texture;

		drawingSizeX = Mathf.Ceil(drawingSizeX * rake.GetComponent<Transform>().localScale.x / gravel.GetComponent<Transform>().localScale.x);
		drawingSizeZ = Mathf.Ceil(drawingSizeZ * rake.GetComponent<Transform>().localScale.z / gravel.GetComponent<Transform>().localScale.z);

		raked = 0;

		//color.r = 0.2f;
		//color.g = 0.2f;
		//color.b = 0.2f;
		//color.a = 1.0f;

		originalColor.r = 0.0f;
		originalColor.g = 0.0f;
		originalColor.b = 0.0f;
		originalColor.a = 0.0f;

		coordinates = new int[1024, 1024];

		originalThird = (int)Mathf.Ceil((float)texture.width / 3.0f);

		tine1Collision = false;
		tine2Collision = false;
		tine3Collision = false;
		tine4Collision = false;

		isRakedEnough = false;
	}

	// Update is called once per frame
	void Update() {
		if (player.w == myHyper.w) /*&& rake.transform.parent)*/ {
			if (tine1Collision || tine2Collision || tine3Collision || tine4Collision) {
				calculateTinePositions();

				if (Vector3.Dot(Vector3.up, transform.up) > 0) {
					if (tine1Collision == true) {
						draw(tine1Position.x, tine1Position.z);
					}

					if (tine2Collision == true) {
						draw(tine2Position.x, tine2Position.z);
					}

					if (tine3Collision == true) {
						draw(tine3Position.x, tine3Position.z);
					}

					if (tine4Collision == true) {
						draw(tine4Position.x, tine4Position.z);
					}
				}

				CalculateAmountRaked();
			}

			//raked = 0;
		}

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			clearPitSection1();
			raked = 0;
			foreach (int coordinate in coordinates) {
				if (coordinate == 1) {
					raked++;
				}
			}

			CalculateAmountRaked();
		}

		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			clearPitSection2();
			raked = 0;
			foreach (int coordinate in coordinates) {
				if (coordinate == 1) {
					raked++;
				}
			}

			CalculateAmountRaked();
		}

		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			clearPitSection3();
			raked = 0;
			foreach (int coordinate in coordinates) {
				if (coordinate == 1) {
					raked++;
				}
			}

			CalculateAmountRaked();
		}

		texture.Apply();
	}

	void OnDestroy() {
		clearPitSection1();
		clearPitSection2();
		clearPitSection3();
	}

	void CalculateAmountRaked() {
		if (raked >= (originalThird * texture.height) && !isRakedEnough) {
			isRakedEnough = true;
			Debug.Log("Raked enough.");
			gravelShrine.processPits();
		}

		else if (raked < (originalThird * texture.height) && isRakedEnough) {
			isRakedEnough = false;
			Debug.Log("Not Raked enough.");
			gravelShrine.processPits();
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.name == "Tine 1") {
			tine1Collision = true;
		}

		if (col.gameObject.name == "Tine 2") {
			tine2Collision = true;
		}

		if (col.gameObject.name == "Tine 3") {
			tine3Collision = true;
		}

		if (col.gameObject.name == "Tine 4") {
			tine4Collision = true;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.transform.name == "Tine 1") {
			tine1Collision = false;
			GameObject clone;
			clone = Instantiate (drawnNode, new Vector3 (tine1.GetComponent<Transform>().position.x, 0, tine1.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
			clone.GetComponent<Node>().drawnNodeNumber = GetComponent<PatternRecognition>().nodeNumber;
			//Debug.Log ("Tine 1 Line Node Number: Node " + clone.GetComponent<Node>().drawnNodeNumber);
			clone.transform.parent = GetComponent<PatternRecognition>().currentTine1Line.transform;
			//Debug.Log ("Tine 1 node parent: Line " + clone.transform.parent.GetComponent<DrawnLine>().drawnLineNumber);
			GetComponent<Alpha>().tine1Nodes++;
			//Debug.Log ("Total Tine 1 nodes: " + GetComponent<Alpha>().tine1Nodes);
			//Debug.Log ("Total nodes: " + nodeNumber);
			clone.GetComponent<Node>().previousNode = GetComponent<PatternRecognition>().lastTine1Node;
			//Debug.Log ("Previous Tine 1 Node: Node " + clone.GetComponent<Node>().previousNode.GetComponent<Node>().drawnNodeNumber);
			GetComponent<PatternRecognition>().lastTine1Node = clone;
			nodeNumber++;

			//Debug.Log ("Last Current Tine 1 Node: Node " + GetComponent<PatternRecognition>().lastTine1Node.GetComponent<Node>().drawnNodeNumber);
			GetComponent<PatternRecognition>().currentTine1Line.GetComponent<DrawnLine>().drawnEndNode = GetComponent<PatternRecognition>().lastTine1Node;
			//Debug.Log ("Current Tine 1 Line End Node: Node " + GetComponent<PatternRecognition>().currentTine1Line.GetComponent<DrawnLine>().drawnEndNode.GetComponent<Node>().drawnNodeNumber);
			GetComponent<PatternRecognition>().currentTine1Line.GetComponent<DrawnLine>().drawnVectorFromStart = (GetComponent<PatternRecognition>().currentTine1Line.GetComponent<DrawnLine>().drawnEndNode.transform.position - GetComponent<PatternRecognition>().currentTine1Line.GetComponent<DrawnLine>().drawnStartNode.transform.position);
			//Debug.Log ("Current Tine 1 Line Vector From Start: " + GetComponent<PatternRecognition>().currentTine1Line.GetComponent<DrawnLine>().drawnVectorFromStart);
			GetComponent<PatternRecognition>().currentTine1Line.GetComponent<DrawnLine>().drawnVectorFromEnd = (GetComponent<PatternRecognition>().currentTine1Line.GetComponent<DrawnLine>().drawnStartNode.transform.position - GetComponent<PatternRecognition>().currentTine1Line.GetComponent<DrawnLine>().drawnEndNode.transform.position);
			//Debug.Log ("Current Tine 1 Line Vector From End: " + GetComponent<PatternRecognition>().currentTine1Line.GetComponent<DrawnLine>().drawnVectorFromEnd);
			tine1Nodes = 0;
		}

		if (col.transform.name == "Tine 2") {
			tine2Collision = false;
			GameObject clone;
			clone = Instantiate (drawnNode, new Vector3 (tine2.GetComponent<Transform>().position.x, 0, tine2.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
			clone.GetComponent<Node>().drawnNodeNumber = GetComponent<PatternRecognition>().nodeNumber;
			//Debug.Log ("Tine 2 Line Node Number: Node " + clone.GetComponent<Node>().drawnNodeNumber);
			clone.transform.parent = GetComponent<PatternRecognition>().currentTine2Line.transform;
			//Debug.Log ("Tine 2 node parent: Line " + clone.transform.parent.GetComponent<DrawnLine>().drawnLineNumber);
			GetComponent<Alpha>().tine2Nodes++;
			//Debug.Log ("Total Tine 2 nodes: " + GetComponent<Alpha>().tine2Nodes);
			//Debug.Log ("Total nodes: " + nodeNumber);
			clone.GetComponent<Node>().previousNode = GetComponent<PatternRecognition>().lastTine2Node;
			//Debug.Log ("Previous Tine 2 Node: Node " + clone.GetComponent<Node>().previousNode.GetComponent<Node>().drawnNodeNumber);
			GetComponent<PatternRecognition>().lastTine2Node = clone;
			nodeNumber++;

			//Debug.Log ("Last Current Tine 2 Node: Node " + GetComponent<PatternRecognition>().lastTine2Node.GetComponent<Node>().drawnNodeNumber);
			GetComponent<PatternRecognition>().currentTine2Line.GetComponent<DrawnLine>().drawnEndNode = GetComponent<PatternRecognition>().lastTine2Node;
			//Debug.Log ("Current Tine 2 Line End Node: Node " + GetComponent<PatternRecognition>().currentTine2Line.GetComponent<DrawnLine>().drawnEndNode.GetComponent<Node>().drawnNodeNumber);
			GetComponent<PatternRecognition>().currentTine2Line.GetComponent<DrawnLine>().drawnVectorFromStart = (GetComponent<PatternRecognition>().currentTine2Line.GetComponent<DrawnLine>().drawnEndNode.transform.position - GetComponent<PatternRecognition>().currentTine2Line.GetComponent<DrawnLine>().drawnStartNode.transform.position);
			//Debug.Log ("Current Tine 2 Line Vector From Start: " + GetComponent<PatternRecognition>().currentTine2Line.GetComponent<DrawnLine>().drawnVectorFromStart);
			GetComponent<PatternRecognition>().currentTine2Line.GetComponent<DrawnLine>().drawnVectorFromEnd = (GetComponent<PatternRecognition>().currentTine2Line.GetComponent<DrawnLine>().drawnStartNode.transform.position - GetComponent<PatternRecognition>().currentTine2Line.GetComponent<DrawnLine>().drawnEndNode.transform.position);
			//Debug.Log ("Current Tine 2 Line Vector From End: " + GetComponent<PatternRecognition>().currentTine2Line.GetComponent<DrawnLine>().drawnVectorFromEnd);
			tine2Nodes = 0;
		}

		if (col.transform.name == "Tine 3") {
			tine3Collision = false;
			GameObject clone;
			clone = Instantiate (drawnNode, new Vector3 (tine3.GetComponent<Transform>().position.x, 0, tine3.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
			clone.GetComponent<Node>().drawnNodeNumber = GetComponent<PatternRecognition>().nodeNumber;
			//Debug.Log ("Tine 3 Line Node Number: Node " + clone.GetComponent<Node>().drawnNodeNumber);
			clone.transform.parent = GetComponent<PatternRecognition>().currentTine3Line.transform;
			//Debug.Log ("Tine 3 node parent: Line " + clone.transform.parent.GetComponent<DrawnLine>().drawnLineNumber);
			GetComponent<Alpha>().tine3Nodes++;
			//Debug.Log ("Total Tine 3 nodes: " + GetComponent<Alpha>().tine3Nodes);
			//Debug.Log ("Total nodes: " + nodeNumber);
			clone.GetComponent<Node>().previousNode = GetComponent<PatternRecognition>().lastTine3Node;
			//Debug.Log ("Previous Tine 3 Node: Node " + clone.GetComponent<Node>().previousNode.GetComponent<Node>().drawnNodeNumber);
			GetComponent<PatternRecognition>().lastTine3Node = clone;
			nodeNumber++;

			//Debug.Log ("Last Current Tine 3 Node: Node " + GetComponent<PatternRecognition>().lastTine3Node.GetComponent<Node>().drawnNodeNumber);
			GetComponent<PatternRecognition>().currentTine3Line.GetComponent<DrawnLine>().drawnEndNode = GetComponent<PatternRecognition>().lastTine3Node;
			//Debug.Log ("Current Tine 3 Line End Node: Node " + GetComponent<PatternRecognition>().currentTine3Line.GetComponent<DrawnLine>().drawnEndNode.GetComponent<Node>().drawnNodeNumber);
			GetComponent<PatternRecognition>().currentTine3Line.GetComponent<DrawnLine>().drawnVectorFromStart = (GetComponent<PatternRecognition>().currentTine3Line.GetComponent<DrawnLine>().drawnEndNode.transform.position - GetComponent<PatternRecognition>().currentTine3Line.GetComponent<DrawnLine>().drawnStartNode.transform.position);
			//Debug.Log ("Current Tine 3 Line Vector From Start: " + GetComponent<PatternRecognition>().currentTine3Line.GetComponent<DrawnLine>().drawnVectorFromStart);
			GetComponent<PatternRecognition>().currentTine3Line.GetComponent<DrawnLine>().drawnVectorFromEnd = (GetComponent<PatternRecognition>().currentTine3Line.GetComponent<DrawnLine>().drawnStartNode.transform.position - GetComponent<PatternRecognition>().currentTine3Line.GetComponent<DrawnLine>().drawnEndNode.transform.position);
			//Debug.Log ("Current Tine 3 Line Vector From End: " + GetComponent<PatternRecognition>().currentTine3Line.GetComponent<DrawnLine>().drawnVectorFromEnd);
			tine3Nodes = 0;
		}

		if (col.transform.name == "Tine 4") {
			tine4Collision = false;
			GameObject clone;
			clone = Instantiate (drawnNode, new Vector3 (tine4.GetComponent<Transform>().position.x, 0, tine4.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
			clone.GetComponent<Node>().drawnNodeNumber = GetComponent<PatternRecognition>().nodeNumber;
			//Debug.Log ("Tine 4 Line Node Number: Node " + clone.GetComponent<Node>().drawnNodeNumber);
			clone.transform.parent = GetComponent<PatternRecognition>().currentTine4Line.transform;
			//Debug.Log ("Tine 4 node parent: Line " + clone.transform.parent.GetComponent<DrawnLine>().drawnLineNumber);
			GetComponent<Alpha>().tine4Nodes++;
			//Debug.Log ("Total Tine 4 nodes: " + GetComponent<Alpha>().tine4Nodes);
			//Debug.Log ("Total nodes: " + nodeNumber);
			clone.GetComponent<Node>().previousNode = GetComponent<PatternRecognition>().lastTine4Node;
			//Debug.Log ("Previous Tine 4 Node: Node " + clone.GetComponent<Node>().previousNode.GetComponent<Node>().drawnNodeNumber);
			GetComponent<PatternRecognition>().lastTine4Node = clone;
			nodeNumber++;

			//Debug.Log ("Last Current Tine 4 Node: Node " + GetComponent<PatternRecognition>().lastTine4Node.GetComponent<Node>().drawnNodeNumber);
			GetComponent<PatternRecognition>().currentTine4Line.GetComponent<DrawnLine>().drawnEndNode = GetComponent<PatternRecognition>().lastTine4Node;
			//Debug.Log ("Current Tine 4 Line End Node: Node " + GetComponent<PatternRecognition>().currentTine4Line.GetComponent<DrawnLine>().drawnEndNode.GetComponent<Node>().drawnNodeNumber);
			GetComponent<PatternRecognition>().currentTine4Line.GetComponent<DrawnLine>().drawnVectorFromStart = (GetComponent<PatternRecognition>().currentTine4Line.GetComponent<DrawnLine>().drawnEndNode.transform.position - GetComponent<PatternRecognition>().currentTine4Line.GetComponent<DrawnLine>().drawnStartNode.transform.position);
			//Debug.Log ("Current Tine 4 Line Vector From Start: " + GetComponent<PatternRecognition>().currentTine4Line.GetComponent<DrawnLine>().drawnVectorFromStart);
			GetComponent<PatternRecognition>().currentTine4Line.GetComponent<DrawnLine>().drawnVectorFromEnd = (GetComponent<PatternRecognition>().currentTine4Line.GetComponent<DrawnLine>().drawnStartNode.transform.position - GetComponent<PatternRecognition>().currentTine4Line.GetComponent<DrawnLine>().drawnEndNode.transform.position);
			//Debug.Log ("Current Tine 4 Line Vector From End: " + GetComponent<PatternRecognition>().currentTine4Line.GetComponent<DrawnLine>().drawnVectorFromEnd);
			tine4Nodes = 0;
		}
	}

	void calculateTinePositions() {
		if (gravel.GetComponent<Transform>().position.x > 0) {
			tine1Position.x = Mathf.Ceil((tine1.GetComponent<Transform>().position.x - gravel.GetComponent<Transform>().position.x + (5.0f * gravel.GetComponent<Transform>().localScale.x)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.x)));
			tine2Position.x = Mathf.Ceil((tine2.GetComponent<Transform>().position.x - gravel.GetComponent<Transform>().position.x + (5.0f * gravel.GetComponent<Transform>().localScale.x)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.x)));
			tine3Position.x = Mathf.Ceil((tine3.GetComponent<Transform>().position.x - gravel.GetComponent<Transform>().position.x + (5.0f * gravel.GetComponent<Transform>().localScale.x)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.x)));
			tine4Position.x = Mathf.Ceil((tine4.GetComponent<Transform>().position.x - gravel.GetComponent<Transform>().position.x + (5.0f * gravel.GetComponent<Transform>().localScale.x)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.x)));
		}

		else if (gravel.GetComponent<Transform>().position.x < 0) {
			tine1Position.x = Mathf.Ceil((tine1.GetComponent<Transform>().position.x + gravel.GetComponent<Transform>().position.x + (5.0f * gravel.GetComponent<Transform>().localScale.x)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.x)));
			tine2Position.x = Mathf.Ceil((tine2.GetComponent<Transform>().position.x + gravel.GetComponent<Transform>().position.x + (5.0f * gravel.GetComponent<Transform>().localScale.x)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.x)));
			tine3Position.x = Mathf.Ceil((tine3.GetComponent<Transform>().position.x + gravel.GetComponent<Transform>().position.x + (5.0f * gravel.GetComponent<Transform>().localScale.x)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.x)));
			tine4Position.x = Mathf.Ceil((tine4.GetComponent<Transform>().position.x + gravel.GetComponent<Transform>().position.x + (5.0f * gravel.GetComponent<Transform>().localScale.x)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.x)));
		}

		else if (gravel.GetComponent<Transform>().position.x == 0) {
			tine1Position.x = Mathf.Ceil((tine1.GetComponent<Transform>().position.x + (5.0f * gravel.GetComponent<Transform>().localScale.x)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.x)));
			tine2Position.x = Mathf.Ceil((tine2.GetComponent<Transform>().position.x + (5.0f * gravel.GetComponent<Transform>().localScale.x)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.x)));
			tine3Position.x = Mathf.Ceil((tine3.GetComponent<Transform>().position.x + (5.0f * gravel.GetComponent<Transform>().localScale.x)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.x)));
			tine4Position.x = Mathf.Ceil((tine4.GetComponent<Transform>().position.x + (5.0f * gravel.GetComponent<Transform>().localScale.x)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.x)));
		}

		if (gravel.GetComponent<Transform> ().position.z > 0) {
			tine1Position.z = Mathf.Ceil((tine1.GetComponent<Transform>().position.z - gravel.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
			tine2Position.z = Mathf.Ceil((tine2.GetComponent<Transform>().position.z - gravel.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
			tine3Position.z = Mathf.Ceil((tine3.GetComponent<Transform>().position.z - gravel.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
			tine4Position.z = Mathf.Ceil((tine4.GetComponent<Transform>().position.z - gravel.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
		}

		else if (gravel.GetComponent<Transform> ().position.z < 0) {
			tine1Position.z = Mathf.Ceil((tine1.GetComponent<Transform>().position.z + gravel.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
			tine2Position.z = Mathf.Ceil((tine2.GetComponent<Transform>().position.z + gravel.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
			tine3Position.z = Mathf.Ceil((tine3.GetComponent<Transform>().position.z + gravel.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
			tine4Position.z = Mathf.Ceil((tine4.GetComponent<Transform>().position.z + gravel.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
		}

		else if (gravel.GetComponent<Transform> ().position.z == 0) {
			tine1Position.z = Mathf.Ceil((tine1.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
			tine2Position.z = Mathf.Ceil((tine2.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
			tine3Position.z = Mathf.Ceil((tine3.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
			tine4Position.z = Mathf.Ceil((tine4.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
		}
	}

	void draw(float tineXPosition, float tineZPosition) {
		for (int x = ((int)tineXPosition - (int)drawingSizeX); x <= ((int)tineXPosition + (int)drawingSizeX); x++) {
			for (int z = ((int)tineZPosition - (int)drawingSizeZ); z <= ((int)tineZPosition + (int)drawingSizeZ); z++) {
				if (x < 0 || x > 1023 || z < 0 || z > 1023) {
					break;
				}

				if (coordinates [x, z] == 1) {
					break;
				} 

				else {
					texture.SetPixel(x, z, color);
					coordinates [x, z] = 1;
					raked++;
				}
			}
		}
	}

	void clearPitSection1() {
		for (int i = 1; i <= originalThird; i++) {
			for (int j = 1; j <= texture.height; j++) {
				texture.SetPixel((i - 1), (j - 1), originalColor);
				coordinates [(i - 1), (j - 1)] = 0;
			}
		}
	}

	void clearPitSection2() {
		for (int i = (originalThird + 1); i <= (originalThird * 2); i++) {
			for (int j = 1; j <= texture.height; j++) {
				texture.SetPixel((i - 1), (j - 1), originalColor);
				coordinates [(i - 1), (j - 1)] = 0;
			}
		}
	}

	void clearPitSection3() {
		for (int i = ((originalThird * 2) + 1); i <= texture.width; i++) {
			for (int j = 1; j <= texture.height; j++) {
				texture.SetPixel((i - 1), (j - 1), originalColor);
				coordinates [(i - 1), (j - 1)] = 0;
			}
		}
	}
}
