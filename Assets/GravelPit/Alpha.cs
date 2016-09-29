using UnityEngine;
using System.Collections;

public class Alpha : MonoBehaviour {
	public GameObject rake;
	public GameObject gravel;
	public GameObject pit;
	public GameObject drawnPattern;
	public GameObject originalPattern;
	public GameObject drawnLine;
	public GameObject drawnNode;
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
	public int frame;
	public float drawPeriod;
	public float nextActionTime1;
	public float nextActionTime2;
	public float nextActionTime3;
	public float nextActionTime4;
	public int raked;
	public bool isRakedEnough;
	public int originalThird;
	public bool checkPattern;
	public int tine1Nodes;
	public int tine2Nodes;
	public int tine3Nodes;
	public int tine4Nodes;
	public float tine1Lines;
	public float tine2Lines;
	public float tine3Lines;
	public float tine4Lines;
	public GameObject lastTine1Node;
	public GameObject lastTine2Node;
	public GameObject lastTine3Node;
	public GameObject lastTine4Node;
	public GameObject currentTine1Line;
	public GameObject currentTine2Line;
	public GameObject currentTine3Line;
	public GameObject currentTine4Line;
	public int lineNumber;
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

		originalColor.r = 0.0f;
		originalColor.g = 0.0f;
		originalColor.b = 0.0f;
		originalColor.a = 0.0f;

		coordinates = new int[1024, 1024];

		originalThird = (int)Mathf.Ceil((float)texture.width/3.0f);

		tine1Collision = false;
		tine2Collision = false;
		tine3Collision = false;
		tine4Collision = false;

		isRakedEnough = false;

		lastTine1Node = null;
		lastTine2Node = null;
		lastTine3Node = null;
		lastTine4Node = null;
		lineNumber = 1;

		drawPeriod = 3.0f;
		frame = 1;
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

		if (frame == 1) {
			if (tine1Collision == true) {
				if (tine1Nodes == 0) {
					GameObject line;
					GameObject clone;
					line = Instantiate (drawnLine, new Vector3 (tine1.GetComponent<Transform>().position.x, 0, tine1.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
					clone = Instantiate (drawnNode, new Vector3 (tine1.GetComponent<Transform>().position.x, 0, tine1.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
					currentTine1Line = line; 
					line.GetComponent<DrawnLine>().tineNumber = 1;
					line.GetComponent<DrawnLine>().drawnLineNumber = lineNumber;
					line.GetComponent<DrawnLine>().drawnDimension = rake.GetComponent<HyperColliderManager>().w;
					line.GetComponent<DrawnLine>().drawnStartNode = clone;
					line.transform.parent = drawnPattern.transform;
					clone.transform.parent = line.transform;
					tine1Nodes++;
					//Debug.Log ("Total Tine 1 nodes: " + tine1Nodes);
					lastTine1Node = clone;
					lineNumber++;
				}

				if (Time.time > nextActionTime1) {
					nextActionTime1 = Time.time + drawPeriod;
					if (tine1Nodes != 0) {
						GameObject clone;
						clone = Instantiate (drawnNode, new Vector3 (tine1.GetComponent<Transform>().position.x, 0, tine1.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
						clone.transform.parent = currentTine1Line.transform;
						tine1Nodes++;
						//Debug.Log ("Total Tine 1 nodes: " + tine1Nodes);
						lastTine1Node = clone;
					}
				}
			}
		}

		if (frame == 2) {
			if (tine2Collision == true) {
				if (tine2Nodes == 0) {
					GameObject line;
					GameObject clone;
					line = Instantiate (drawnLine, new Vector3 (tine2.GetComponent<Transform>().position.x, 0, tine2.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
					clone = Instantiate (drawnNode, new Vector3 (tine2.GetComponent<Transform>().position.x, 0, tine2.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
					currentTine2Line = line; 
					line.GetComponent<DrawnLine>().tineNumber = 2;
					line.GetComponent<DrawnLine>().drawnLineNumber = lineNumber;
					line.GetComponent<DrawnLine>().drawnDimension = rake.GetComponent<HyperColliderManager>().w;
					line.GetComponent<DrawnLine>().drawnStartNode = clone;
					line.transform.parent = drawnPattern.transform;
					clone.transform.parent = line.transform;
					tine2Nodes++;
					//Debug.Log ("Total Tine 2 nodes: " + tine2Nodes);
					lastTine2Node = clone;
					lineNumber++;
				}

				if (Time.time > nextActionTime2) {
					nextActionTime2 = Time.time + drawPeriod;
					if (tine2Nodes != 0) {
						GameObject clone;
						clone = Instantiate (drawnNode, new Vector3 (tine2.GetComponent<Transform>().position.x, 0, tine2.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
						clone.transform.parent = currentTine2Line.transform;
						tine2Nodes++;
						//Debug.Log ("Total Tine 2 nodes: " + tine2Nodes);
						lastTine2Node = clone;
					}
				}
			}
		}

		if (frame == 3) {
			if (tine3Collision == true) {
				if (tine3Nodes == 0) {
					GameObject line;
					GameObject clone;
					line = Instantiate (drawnLine, new Vector3 (tine3.GetComponent<Transform>().position.x, 0, tine3.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
					clone = Instantiate (drawnNode, new Vector3 (tine3.GetComponent<Transform>().position.x, 0, tine3.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
					currentTine3Line = line; 
					line.GetComponent<DrawnLine>().tineNumber = 3;
					line.GetComponent<DrawnLine>().drawnLineNumber = lineNumber;
					line.GetComponent<DrawnLine>().drawnDimension = rake.GetComponent<HyperColliderManager>().w;
					line.GetComponent<DrawnLine>().drawnStartNode = clone;
					line.transform.parent = drawnPattern.transform;
					clone.transform.parent = line.transform;
					tine3Nodes++;
					//Debug.Log ("Total Tine 3 nodes: " + tine3Nodes);
					lastTine3Node = clone;
					lineNumber++;
				}

				if (Time.time > nextActionTime3) {
					nextActionTime3 = Time.time + drawPeriod;
					if (tine3Nodes != 0) {
						GameObject clone;
						clone = Instantiate (drawnNode, new Vector3 (tine3.GetComponent<Transform>().position.x, 0, tine3.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
						clone.transform.parent = currentTine3Line.transform;
						tine3Nodes++;
						//Debug.Log ("Total Tine 3 nodes: " + tine3Nodes);
						lastTine3Node = clone;
					}
				}
			}
		}

		if (frame == 4) {
			if (tine4Collision == true) {
				if (tine4Nodes == 0) {
					GameObject line;
					GameObject clone;
					line = Instantiate (drawnLine, new Vector3 (tine4.GetComponent<Transform>().position.x, 0, tine4.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
					clone = Instantiate (drawnNode, new Vector3 (tine4.GetComponent<Transform>().position.x, 0, tine4.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
					currentTine4Line = line; 
					line.GetComponent<DrawnLine>().tineNumber = 4;
					line.GetComponent<DrawnLine>().drawnLineNumber = lineNumber;
					line.GetComponent<DrawnLine>().drawnDimension = rake.GetComponent<HyperColliderManager>().w;
					line.GetComponent<DrawnLine>().drawnStartNode = clone;
					line.transform.parent = drawnPattern.transform;
					clone.transform.parent = line.transform;
					tine4Nodes++;
					//Debug.Log ("Total Tine 4 nodes: " + tine4Nodes);
					lastTine4Node = clone;
					lineNumber++;
				}

				if (Time.time > nextActionTime4) {
					nextActionTime4 = Time.time + drawPeriod;
					if (tine4Nodes != 0) {
						GameObject clone;
						clone = Instantiate (drawnNode, new Vector3 (tine4.GetComponent<Transform>().position.x, 0, tine4.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
						clone.transform.parent = currentTine4Line.transform;
						tine4Nodes++;
						//Debug.Log ("Total Tine 4 nodes: " + tine4Nodes);
						lastTine4Node = clone;
					}
				}
			}

			frame = 0;
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
			gravelShrine.processPits();
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
			gravelShrine.processPits();
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
			gravelShrine.processPits();
		}

		frame++;

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
			//Debug.Log("Raked enough.");
			gravelShrine.processPits();
		}

		else if (raked < (originalThird * texture.height) && isRakedEnough) {
			isRakedEnough = false;
			//Debug.Log("Not Raked enough.");
			gravelShrine.processPits();
		}
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.name == "Tine 1") {
			tine1Collision = true;

			GameObject line;
			GameObject clone;
			line = Instantiate (drawnLine, new Vector3 (tine1.GetComponent<Transform>().position.x, 0, tine1.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
			clone = Instantiate (drawnNode, new Vector3 (tine1.GetComponent<Transform>().position.x, 0, tine1.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
			currentTine1Line = line; 
			line.GetComponent<DrawnLine>().tineNumber = 1;
			line.GetComponent<DrawnLine>().drawnLineNumber = lineNumber;
			line.GetComponent<DrawnLine>().drawnDimension = rake.GetComponent<HyperColliderManager>().w;
			line.GetComponent<DrawnLine>().drawnStartNode = clone;
			line.transform.parent = drawnPattern.transform;
			clone.transform.parent = line.transform;
			tine1Nodes++;
			//Debug.Log ("Total Tine 1 nodes: " + tine1Nodes);
			lastTine1Node = clone;
			lineNumber++;
		}

		if (col.gameObject.name == "Tine 2") {
			tine2Collision = true;

			GameObject line;
			GameObject clone;
			line = Instantiate (drawnLine, new Vector3 (tine2.GetComponent<Transform>().position.x, 0, tine2.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
			clone = Instantiate (drawnNode, new Vector3 (tine2.GetComponent<Transform>().position.x, 0, tine2.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
			currentTine2Line = line; 
			line.GetComponent<DrawnLine>().tineNumber = 2;
			line.GetComponent<DrawnLine>().drawnLineNumber = lineNumber;
			line.GetComponent<DrawnLine>().drawnDimension = rake.GetComponent<HyperColliderManager>().w;
			line.GetComponent<DrawnLine>().drawnStartNode = clone;
			line.transform.parent = drawnPattern.transform;
			clone.transform.parent = line.transform;
			tine2Nodes++;
			//Debug.Log ("Total Tine 2 nodes: " + tine2Nodes);
			lastTine2Node = clone;
			lineNumber++;
		}

		if (col.gameObject.name == "Tine 3") {
			tine3Collision = true;

			GameObject line;
			GameObject clone;
			line = Instantiate (drawnLine, new Vector3 (tine3.GetComponent<Transform>().position.x, 0, tine3.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
			clone = Instantiate (drawnNode, new Vector3 (tine3.GetComponent<Transform>().position.x, 0, tine3.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
			currentTine3Line = line; 
			line.GetComponent<DrawnLine>().tineNumber = 3;
			line.GetComponent<DrawnLine>().drawnLineNumber = lineNumber;
			line.GetComponent<DrawnLine>().drawnDimension = rake.GetComponent<HyperColliderManager>().w;
			line.GetComponent<DrawnLine>().drawnStartNode = clone;
			line.transform.parent = drawnPattern.transform;
			clone.transform.parent = line.transform;
			tine3Nodes++;
			//Debug.Log ("Total Tine 3 nodes: " + tine3Nodes);
			lastTine3Node = clone;
			lineNumber++;
		}

		if (col.gameObject.name == "Tine 4") {
			tine4Collision = true;

			GameObject line;
			GameObject clone;
			line = Instantiate (drawnLine, new Vector3 (tine4.GetComponent<Transform>().position.x, 0, tine4.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
			clone = Instantiate (drawnNode, new Vector3 (tine4.GetComponent<Transform>().position.x, 0, tine4.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
			currentTine4Line = line; 
			line.GetComponent<DrawnLine>().tineNumber = 4;
			line.GetComponent<DrawnLine>().drawnLineNumber = lineNumber;
			line.GetComponent<DrawnLine>().drawnDimension = rake.GetComponent<HyperColliderManager>().w;
			line.GetComponent<DrawnLine>().drawnStartNode = clone;
			line.transform.parent = drawnPattern.transform;
			clone.transform.parent = line.transform;
			tine4Nodes++;
			//Debug.Log ("Total Tine 4 nodes: " + tine4Nodes);
			lastTine1Node = clone;
			lineNumber++;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.transform.name == "Tine 1") {
			tine1Collision = false;

			GameObject clone;
			clone = Instantiate (drawnNode, new Vector3 (tine1.GetComponent<Transform>().position.x, 0, tine1.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
			clone.transform.parent = currentTine1Line.transform;
			tine1Nodes++;
			//Debug.Log ("Total Tine 1 nodes: " + tine1Nodes);
			lastTine1Node = clone;

			currentTine1Line.GetComponent<DrawnLine>().drawnEndNode = lastTine1Node;
			currentTine1Line.GetComponent<DrawnLine>().drawnVector = (currentTine1Line.GetComponent<DrawnLine>().drawnEndNode.transform.position - currentTine1Line.GetComponent<DrawnLine>().drawnStartNode.transform.position);
			//Debug.Log ("Current Tine 1 Line Vector: " + currentTine1Line.GetComponent<DrawnLine>().drawnVector);
			tine1Nodes = 0;
			tine1Lines += 1.0f;
		}

		if (col.transform.name == "Tine 2") {
			tine2Collision = false;

			GameObject clone;
			clone = Instantiate (drawnNode, new Vector3 (tine2.GetComponent<Transform>().position.x, 0, tine2.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
			clone.transform.parent = currentTine2Line.transform;
			tine2Nodes++;
			//Debug.Log ("Total Tine 2 nodes: " + tine2Nodes);
			lastTine2Node = clone;

			currentTine2Line.GetComponent<DrawnLine>().drawnEndNode = lastTine2Node;
			currentTine2Line.GetComponent<DrawnLine>().drawnVector = (currentTine2Line.GetComponent<DrawnLine>().drawnEndNode.transform.position - currentTine2Line.GetComponent<DrawnLine>().drawnStartNode.transform.position);
			//Debug.Log ("Current Tine 2 Line Vector: " + currentTine2Line.GetComponent<DrawnLine>().drawnVector);
			tine2Nodes = 0;
			tine2Lines += 1.0f;
		}

		if (col.transform.name == "Tine 3") {
			tine3Collision = false;

			GameObject clone;
			clone = Instantiate (drawnNode, new Vector3 (tine3.GetComponent<Transform>().position.x, 0, tine3.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
			clone.transform.parent = currentTine3Line.transform;
			tine3Nodes++;
			//Debug.Log ("Total Tine 3 nodes: " + tine3Nodes);
			lastTine3Node = clone;

			currentTine3Line.GetComponent<DrawnLine>().drawnEndNode = lastTine3Node;
			currentTine3Line.GetComponent<DrawnLine>().drawnVector = (currentTine3Line.GetComponent<DrawnLine>().drawnEndNode.transform.position - currentTine3Line.GetComponent<DrawnLine>().drawnStartNode.transform.position);
			//Debug.Log ("Current Tine 3 Line Vector: " + currentTine3Line.GetComponent<DrawnLine>().drawnVector);
			tine3Nodes = 0;
			tine3Lines += 1.0f;
		}

		if (col.transform.name == "Tine 4") {
			tine4Collision = false;

			GameObject clone;
			clone = Instantiate (drawnNode, new Vector3 (tine4.GetComponent<Transform>().position.x, 0, tine4.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
			clone.transform.parent = currentTine4Line.transform;
			tine4Nodes++;
			//Debug.Log ("Total Tine 4 nodes: " + tine4Nodes);
			lastTine4Node = clone;

			currentTine4Line.GetComponent<DrawnLine>().drawnEndNode = lastTine4Node;
			currentTine4Line.GetComponent<DrawnLine>().drawnVector = (currentTine4Line.GetComponent<DrawnLine>().drawnEndNode.transform.position - currentTine4Line.GetComponent<DrawnLine>().drawnStartNode.transform.position);
			//Debug.Log ("Current Tine 4 Line Vector: " + currentTine4Line.GetComponent<DrawnLine>().drawnVector);
			tine4Nodes = 0;
			tine4Lines += 1.0f;
		}

		if (((tine1Lines + tine2Lines + tine3Lines + tine4Lines)/GetComponent<PatternRecognition>().linesOnDimension) >= 0.80f) {
			if (GetComponent<PatternRecognition>().patternMatches == false) {
				//Debug.Log ("Pattern matches = false");
				checkPattern = true;
				GetComponent<PatternRecognition>().check = 0;
			}	
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

		if (gravel.GetComponent<Transform>().position.z > 0) {
			tine1Position.z = Mathf.Ceil((tine1.GetComponent<Transform>().position.z - gravel.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
			tine2Position.z = Mathf.Ceil((tine2.GetComponent<Transform>().position.z - gravel.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
			tine3Position.z = Mathf.Ceil((tine3.GetComponent<Transform>().position.z - gravel.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
			tine4Position.z = Mathf.Ceil((tine4.GetComponent<Transform>().position.z - gravel.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
		}

		else if (gravel.GetComponent<Transform>().position.z < 0) {
			tine1Position.z = Mathf.Ceil((tine1.GetComponent<Transform>().position.z + gravel.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
			tine2Position.z = Mathf.Ceil((tine2.GetComponent<Transform>().position.z + gravel.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
			tine3Position.z = Mathf.Ceil((tine3.GetComponent<Transform>().position.z + gravel.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
			tine4Position.z = Mathf.Ceil((tine4.GetComponent<Transform>().position.z + gravel.GetComponent<Transform>().position.z + (5.0f * gravel.GetComponent<Transform>().localScale.z)) * ((float)texture.width / (10.0f * gravel.GetComponent<Transform>().localScale.z)));
		}

		else if (gravel.GetComponent<Transform>().position.z == 0) {
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

		Mesh mesh = pit.GetComponent<MeshFilter>().mesh;
		Bounds bounds = mesh.bounds;

		if (tine1Collision == true) {
			float xPosition = currentTine1Line.GetComponent<DrawnLine>().drawnStartNode.transform.position.x;
			float leftBound = (-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f));
			float rightBound = ((-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f)) + ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			if (xPosition >= leftBound && xPosition <= rightBound) {
				currentTine1Line.GetComponent<DrawnLine>().drawnEndNode = lastTine1Node;
				currentTine1Line.GetComponent<DrawnLine>().drawnVector = (currentTine1Line.GetComponent<DrawnLine>().drawnEndNode.transform.position - currentTine1Line.GetComponent<DrawnLine>().drawnStartNode.transform.position);
				//Debug.Log ("Current Tine 1 Line Vector: " + currentTine1Line.GetComponent<DrawnLine>().drawnVector);
				tine1Nodes = 0;
				tine1Lines += 1.0f;
			}
		}

		if (tine2Collision == true) {
			float xPosition = currentTine2Line.GetComponent<DrawnLine>().drawnStartNode.transform.position.x;
			float leftBound = (-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f));
			float rightBound = ((-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f)) + ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			if (xPosition >= leftBound && xPosition <= rightBound) {
				currentTine2Line.GetComponent<DrawnLine> ().drawnEndNode = lastTine2Node;
				currentTine2Line.GetComponent<DrawnLine> ().drawnVector = (currentTine2Line.GetComponent<DrawnLine> ().drawnEndNode.transform.position - currentTine2Line.GetComponent<DrawnLine> ().drawnStartNode.transform.position);
				//Debug.Log ("Current Tine 2 Line Vector: " + currentTine2Line.GetComponent<DrawnLine>().drawnVector);
				tine2Nodes = 0;
				tine2Lines += 1.0f;
			}
		}

		if (tine3Collision == true) {
			float xPosition = currentTine3Line.GetComponent<DrawnLine>().drawnStartNode.transform.position.x;
			float leftBound = (-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f));
			float rightBound = ((-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f)) + ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			if (xPosition >= leftBound && xPosition <= rightBound) {
				currentTine3Line.GetComponent<DrawnLine> ().drawnEndNode = lastTine3Node;
				currentTine3Line.GetComponent<DrawnLine> ().drawnVector = (currentTine3Line.GetComponent<DrawnLine> ().drawnEndNode.transform.position - currentTine3Line.GetComponent<DrawnLine> ().drawnStartNode.transform.position);
				//Debug.Log ("Current Tine 3 Line Vector: " + currentTine3Line.GetComponent<DrawnLine>().drawnVector);
				tine3Nodes = 0;
				tine3Lines += 1.0f;
			}
		}

		if (tine4Collision == true) {
			float xPosition = currentTine4Line.GetComponent<DrawnLine>().drawnStartNode.transform.position.x;
			float leftBound = (-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f));
			float rightBound = ((-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f)) + ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			if (xPosition >= leftBound && xPosition <= rightBound) {
				currentTine4Line.GetComponent<DrawnLine> ().drawnEndNode = lastTine4Node;
				currentTine4Line.GetComponent<DrawnLine> ().drawnVector = (currentTine4Line.GetComponent<DrawnLine> ().drawnEndNode.transform.position - currentTine4Line.GetComponent<DrawnLine> ().drawnStartNode.transform.position);
				//Debug.Log ("Current Tine 4 Line Vector: " + currentTine4Line.GetComponent<DrawnLine>().drawnVector);
				tine4Nodes = 0;
				tine4Lines += 1.0f;
			}
		}

		foreach (Transform child in drawnPattern.transform) {
			float xPosition = child.transform.position.x;
			float leftBound = (-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f));
			float rightBound = ((-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f)) + ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			if (xPosition >= leftBound && xPosition <= rightBound && child.GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				//testPattern.transform.GetChild(k).gameObject.SetActive(false);
				Destroy(child.gameObject);
			}
		}

		bool patternInSection = false;
		foreach (Transform child in originalPattern.transform) {
			float startNode = child.GetChild(0).transform.position.x;
			float endNode = child.GetChild(1).transform.position.x;
			float leftBound = (-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f));
			float rightBound = ((-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f)) + ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			if (startNode >= leftBound && startNode <= rightBound && child.GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				patternInSection = true;
				//Debug.Log ("Pattern line in this section.");
				break;
			}
			if (endNode >= leftBound && endNode <= rightBound && child.GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				patternInSection = true;
				//Debug.Log ("Pattern line in this section.");
				break;
			}
		}

		if (patternInSection == true) {
			GetComponent<PatternRecognition>().patternMatches = false;
		}
	}

	void clearPitSection2() {
		for (int i = (originalThird + 1); i <= (originalThird * 2); i++) {
			for (int j = 1; j <= texture.height; j++) {
				texture.SetPixel((i - 1), (j - 1), originalColor);
				coordinates [(i - 1), (j - 1)] = 0;
			}
		}

		Mesh mesh = pit.GetComponent<MeshFilter>().mesh;
		Bounds bounds = mesh.bounds;

		if (tine1Collision == true) {
			float xPosition = currentTine1Line.GetComponent<DrawnLine>().drawnStartNode.transform.position.x;
			float leftBound = ((-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f)) + ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			float rightBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x/2.0f) - ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			if (xPosition >= leftBound && xPosition <= rightBound) {
				currentTine1Line.GetComponent<DrawnLine> ().drawnEndNode = lastTine1Node;
				currentTine1Line.GetComponent<DrawnLine> ().drawnVector = (currentTine1Line.GetComponent<DrawnLine> ().drawnEndNode.transform.position - currentTine1Line.GetComponent<DrawnLine> ().drawnStartNode.transform.position);
				//Debug.Log ("Current Tine 1 Line Vector: " + currentTine1Line.GetComponent<DrawnLine>().drawnVector);
				tine1Nodes = 0;
				tine1Lines += 1.0f;
			}
		}

		if (tine2Collision == true) {
			float xPosition = currentTine2Line.GetComponent<DrawnLine>().drawnStartNode.transform.position.x;
			float leftBound = ((-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f)) + ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			float rightBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x/2.0f) - ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			if (xPosition >= leftBound && xPosition <= rightBound) {
				currentTine2Line.GetComponent<DrawnLine> ().drawnEndNode = lastTine2Node;
				currentTine2Line.GetComponent<DrawnLine> ().drawnVector = (currentTine2Line.GetComponent<DrawnLine> ().drawnEndNode.transform.position - currentTine2Line.GetComponent<DrawnLine> ().drawnStartNode.transform.position);
				//Debug.Log ("Current Tine 2 Line Vector: " + currentTine2Line.GetComponent<DrawnLine>().drawnVector);
				tine2Nodes = 0;
				tine2Lines += 1.0f;
			}
		}

		if (tine3Collision == true) {
			float xPosition = currentTine3Line.GetComponent<DrawnLine>().drawnStartNode.transform.position.x;
			float leftBound = ((-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f)) + ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			float rightBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x/2.0f) - ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			if (xPosition >= leftBound && xPosition <= rightBound) {
				currentTine3Line.GetComponent<DrawnLine> ().drawnEndNode = lastTine3Node;
				currentTine3Line.GetComponent<DrawnLine> ().drawnVector = (currentTine3Line.GetComponent<DrawnLine> ().drawnEndNode.transform.position - currentTine3Line.GetComponent<DrawnLine> ().drawnStartNode.transform.position);
				//Debug.Log ("Current Tine 3 Line Vector: " + currentTine3Line.GetComponent<DrawnLine>().drawnVector);
				tine3Nodes = 0;
				tine3Lines += 1.0f;
			}
		}

		if (tine4Collision == true) {
			float xPosition = currentTine4Line.GetComponent<DrawnLine>().drawnStartNode.transform.position.x;
			float leftBound = ((-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f)) + ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			float rightBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x/2.0f) - ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			if (xPosition >= leftBound && xPosition <= rightBound) {
				currentTine4Line.GetComponent<DrawnLine> ().drawnEndNode = lastTine4Node;
				currentTine4Line.GetComponent<DrawnLine> ().drawnVector = (currentTine4Line.GetComponent<DrawnLine> ().drawnEndNode.transform.position - currentTine4Line.GetComponent<DrawnLine> ().drawnStartNode.transform.position);
				//Debug.Log ("Current Tine 4 Line Vector: " + currentTine4Line.GetComponent<DrawnLine>().drawnVector);
				tine4Nodes = 0;
				tine4Lines += 1.0f;
			}
		}

		foreach (Transform child in drawnPattern.transform) {
			float xPosition = child.transform.position.x;
			float leftBound = ((-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f)) + ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			float rightBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x/2.0f) - ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			if (xPosition >= leftBound && xPosition <= rightBound && child.GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				//testPattern.transform.GetChild(k).gameObject.SetActive(false);
				Destroy(child.gameObject);
			}
		}

		bool patternInSection = false;
		foreach (Transform child in originalPattern.transform) {
			float startNode = child.GetChild(0).transform.position.x;
			float endNode = child.GetChild(1).transform.position.x;
			float leftBound = ((-1.0f * ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f)) + ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			float rightBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x/2.0f) - ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			if (startNode >= leftBound && startNode <= rightBound && child.GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				patternInSection = true;
				//Debug.Log ("Pattern line in this section.");
				break;
			}
			if (endNode >= leftBound && endNode <= rightBound && child.GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				patternInSection = true;
				//Debug.Log ("Pattern line in this section.");
				break;
			}
		}

		if (patternInSection == true) {
			GetComponent<PatternRecognition>().patternMatches = false;
		}
	}

	void clearPitSection3() {
		for (int i = ((originalThird * 2) + 1); i <= texture.width; i++) {
			for (int j = 1; j <= texture.height; j++) {
				texture.SetPixel((i - 1), (j - 1), originalColor);
				coordinates [(i - 1), (j - 1)] = 0;
			}
		}

		Mesh mesh = pit.GetComponent<MeshFilter>().mesh;
		Bounds bounds = mesh.bounds;

		if (tine1Collision == true) {
			float xPosition = currentTine1Line.GetComponent<DrawnLine>().drawnStartNode.transform.position.x;
			float leftBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x/2.0f) - ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			float rightBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f);
			if (xPosition >= leftBound && xPosition <= rightBound) {
				currentTine1Line.GetComponent<DrawnLine> ().drawnEndNode = lastTine1Node;
				currentTine1Line.GetComponent<DrawnLine> ().drawnVector = (currentTine1Line.GetComponent<DrawnLine> ().drawnEndNode.transform.position - currentTine1Line.GetComponent<DrawnLine> ().drawnStartNode.transform.position);
				//Debug.Log ("Current Tine 1 Line Vector: " + currentTine1Line.GetComponent<DrawnLine>().drawnVector);
				tine1Nodes = 0;
				tine1Lines += 1.0f;
			}
		}

		if (tine2Collision == true) {
			float xPosition = currentTine2Line.GetComponent<DrawnLine>().drawnStartNode.transform.position.x;
			float leftBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x/2.0f) - ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			float rightBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f);
			if (xPosition >= leftBound && xPosition <= rightBound) {
				currentTine2Line.GetComponent<DrawnLine> ().drawnEndNode = lastTine2Node;
				currentTine2Line.GetComponent<DrawnLine> ().drawnVector = (currentTine2Line.GetComponent<DrawnLine> ().drawnEndNode.transform.position - currentTine2Line.GetComponent<DrawnLine> ().drawnStartNode.transform.position);
				//Debug.Log ("Current Tine 2 Line Vector: " + currentTine2Line.GetComponent<DrawnLine>().drawnVector);
				tine2Nodes = 0;
				tine2Lines += 1.0f;
			}
		}

		if (tine3Collision == true) {
			float xPosition = currentTine3Line.GetComponent<DrawnLine>().drawnStartNode.transform.position.x;
			float leftBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x/2.0f) - ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			float rightBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f);
			if (xPosition >= leftBound && xPosition <= rightBound) {
				currentTine3Line.GetComponent<DrawnLine> ().drawnEndNode = lastTine3Node;
				currentTine3Line.GetComponent<DrawnLine> ().drawnVector = (currentTine3Line.GetComponent<DrawnLine> ().drawnEndNode.transform.position - currentTine3Line.GetComponent<DrawnLine> ().drawnStartNode.transform.position);
				//Debug.Log ("Current Tine 3 Line Vector: " + currentTine3Line.GetComponent<DrawnLine>().drawnVector);
				tine3Nodes = 0;
				tine3Lines += 1.0f;
			}
		}

		if (tine4Collision == true) {
			float xPosition = currentTine4Line.GetComponent<DrawnLine>().drawnStartNode.transform.position.x;
			float leftBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x/2.0f) - ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			float rightBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f);
			if (xPosition >= leftBound && xPosition <= rightBound) {
				currentTine4Line.GetComponent<DrawnLine> ().drawnEndNode = lastTine4Node;
				currentTine4Line.GetComponent<DrawnLine> ().drawnVector = (currentTine4Line.GetComponent<DrawnLine> ().drawnEndNode.transform.position - currentTine4Line.GetComponent<DrawnLine> ().drawnStartNode.transform.position);
				//Debug.Log ("Current Tine 4 Line Vector: " + currentTine4Line.GetComponent<DrawnLine>().drawnVector);
				tine4Nodes = 0;
				tine4Lines += 1.0f;
			}
		}

		foreach (Transform child in drawnPattern.transform) {
			float xPosition = child.transform.position.x;
			float leftBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x/2.0f) - ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			float rightBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f);
			if (xPosition >= leftBound && xPosition <= rightBound && child.GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				//testPattern.transform.GetChild(k).gameObject.SetActive(false);
				Destroy(child.gameObject);
			}
		}

		bool patternInSection = false;
		foreach (Transform child in originalPattern.transform) {
			float startNode = child.GetChild(0).transform.position.x;
			float endNode = child.GetChild(1).transform.position.x;
			float leftBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x/2.0f) - ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/3.0f));
			float rightBound = ((bounds.size.x * gravel.GetComponent<Transform>().localScale.x)/2.0f);
			if (startNode >= leftBound && startNode <= rightBound && child.GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				patternInSection = true;
				//Debug.Log ("Pattern line in this section.");
				break;
			}
			if (endNode >= leftBound && endNode <= rightBound && child.GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				patternInSection = true;
				//Debug.Log ("Pattern line in this section.");
				break;
			}
		}

		if (patternInSection == true) {
			GetComponent<PatternRecognition>().patternMatches = false;
		}
	}
}
