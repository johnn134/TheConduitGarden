using UnityEngine;
using System.Collections;

public class PatternRecognition : MonoBehaviour {
	public GameObject rake;
	public GameObject tine1;
	public GameObject tine2;
	public GameObject tine3;
	public GameObject tine4;
	public GameObject drawnNode;
	public GameObject drawnLine;
	public GameObject originalPattern;
	public GameObject drawnPattern;
	public int frame;
	public float similarity;
	public bool patternMatches;
	public float nextActionTime1;
	public float nextActionTime2;
	public float nextActionTime3;
	public float nextActionTime4;
	public float nextPatternCheck;
	public GameObject lastTine1Node;
	public GameObject lastTine2Node;
	public GameObject lastTine3Node;
	public GameObject lastTine4Node;
	public GameObject currentTine1Line;
	public GameObject currentTine2Line;
	public GameObject currentTine3Line;
	public GameObject currentTine4Line;
	public GameObject empty;
	public int lineNumber;
	public int nodeNumber;
	public float rangeCheck;
	public float drawPeriod;
	public float checkPeriod;

	// Use this for initialization
	void Start() {
		frame = 1;
		drawPeriod = 3.0f;
		checkPeriod = 7.0f;
		lineNumber = 1;
		nodeNumber = 1;
		lastTine1Node = empty;
		lastTine2Node = empty;
		lastTine3Node = empty;
		lastTine4Node = empty;

		calculateOriginalPatternVectors();
	}

	// Update is called once per frame
	void Update() {
		if (frame == 1) {
			if (GetComponent<Alpha>().tine1Collision == true) {
				if (Time.time > nextActionTime1) {
					nextActionTime1 = Time.time + drawPeriod;
					GameObject line;
					GameObject clone;
					if (GetComponent<Alpha>().tine1Nodes == 0) {
						line = Instantiate (drawnLine, new Vector3 (tine1.GetComponent<Transform>().position.x, 0, tine1.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
						clone = Instantiate (drawnNode, new Vector3 (tine1.GetComponent<Transform>().position.x, 0, tine1.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
						currentTine1Line = line; 
						line.GetComponent<DrawnLine>().drawnLineNumber = lineNumber;
						//Debug.Log ("Current Tine 1 line number: " + lineNumber);
						line.GetComponent<DrawnLine>().drawnDimension = rake.GetComponent<HyperColliderManager>().w;
						line.GetComponent<DrawnLine>().drawnStartNode = clone;
						clone.GetComponent<Node>().drawnNodeNumber = nodeNumber;
						//Debug.Log ("Tine 1 Line Start Node: Node " + line.GetComponent<DrawnLine>().drawnStartNode.GetComponent<Node>().drawnNodeNumber);
						//Debug.Log ("Tine 1 Line Node Number: Node " + clone.GetComponent<Node>().drawnNodeNumber);
						line.transform.parent = drawnPattern.transform;
						clone.transform.parent = line.transform;
						//Debug.Log ("Tine 1 node parent: Line " + clone.transform.parent.GetComponent<DrawnLine>().drawnLineNumber);
						GetComponent<Alpha>().tine1Nodes++;
						//Debug.Log ("Total Tine 1 nodes: " + GetComponent<Alpha>().tine1Nodes);
						//Debug.Log ("Total lines: " + lineNumber);
						//Debug.Log ("Total nodes: " + nodeNumber);
						lastTine1Node = clone;
						lineNumber++;
						nodeNumber++;
					} 

					else {
						clone = Instantiate (drawnNode, new Vector3 (tine1.GetComponent<Transform>().position.x, 0, tine1.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
						clone.GetComponent<Node>().drawnNodeNumber = nodeNumber;
						//Debug.Log ("Tine 1 Line Node Number: Node " + clone.GetComponent<Node>().drawnNodeNumber);
						clone.transform.parent = currentTine1Line.transform;
						//Debug.Log ("Tine 1 node parent: Line " + clone.transform.parent.GetComponent<DrawnLine>().drawnLineNumber);
						GetComponent<Alpha>().tine1Nodes++;
						//Debug.Log ("Total Tine 1 nodes: " + GetComponent<Alpha>().tine1Nodes);
						//Debug.Log ("Total nodes: " + nodeNumber);
						clone.GetComponent<Node>().previousNode = lastTine1Node;
						//Debug.Log ("Previous Tine 1 Node: Node " + clone.GetComponent<Node>().previousNode.GetComponent<Node>().drawnNodeNumber);
						lastTine1Node = clone;
						nodeNumber++;
					}
				}
			}
		}

		if (frame == 2) {
			if (GetComponent<Alpha>().tine2Collision == true) {
				if (Time.time > nextActionTime2) {
					nextActionTime2 = Time.time + drawPeriod;
					GameObject line;
					GameObject clone;
					if (GetComponent<Alpha>().tine2Nodes == 0) {
						line = Instantiate (drawnLine, new Vector3 (tine2.GetComponent<Transform>().position.x, 0, tine2.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
						clone = Instantiate (drawnNode, new Vector3 (tine2.GetComponent<Transform>().position.x, 0, tine2.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
						currentTine2Line = line; 
						line.GetComponent<DrawnLine>().drawnLineNumber = lineNumber;
						//Debug.Log ("Current Tine 2 line number: " + lineNumber);
						line.GetComponent<DrawnLine>().drawnDimension = rake.GetComponent<HyperColliderManager>().w;
						line.GetComponent<DrawnLine>().drawnStartNode = clone;
						clone.GetComponent<Node>().drawnNodeNumber = nodeNumber;
						//Debug.Log ("Tine 2 Line Start Node: Node " + line.GetComponent<DrawnLine>().drawnStartNode.GetComponent<Node>().drawnNodeNumber);
						//Debug.Log ("Tine 2 Line Node Number: Node " + clone.GetComponent<Node>().drawnNodeNumber);
						line.transform.parent = drawnPattern.transform;
						clone.transform.parent = line.transform;
						//Debug.Log ("Tine 2 node parent: Line " + clone.transform.parent.GetComponent<DrawnLine>().drawnLineNumber);
						GetComponent<Alpha>().tine2Nodes++;
						//Debug.Log ("Total Tine 2 nodes: " + GetComponent<Alpha>().tine2Nodes);
						//Debug.Log ("Total lines: " + lineNumber);
						//Debug.Log ("Total nodes: " + nodeNumber);
						lastTine2Node = clone;
						lineNumber++;
						nodeNumber++;
					} 

					else {
						clone = Instantiate (drawnNode, new Vector3 (tine2.GetComponent<Transform>().position.x, 0, tine2.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
						clone.GetComponent<Node>().drawnNodeNumber = nodeNumber;
						//Debug.Log ("Tine 2 Line Node Number: Node " + clone.GetComponent<Node>().drawnNodeNumber);
						clone.transform.parent = currentTine2Line.transform;
						//Debug.Log ("Tine 2 node parent: Line " + clone.transform.parent.GetComponent<DrawnLine>().drawnLineNumber);
						GetComponent<Alpha>().tine2Nodes++;
						//Debug.Log ("Total Tine 2 nodes: " + GetComponent<Alpha>().tine2Nodes);
						//Debug.Log ("Total nodes: " + nodeNumber);
						clone.GetComponent<Node>().previousNode = lastTine2Node;
						//Debug.Log ("Previous Tine 2 Node: Node " + clone.GetComponent<Node>().previousNode.GetComponent<Node>().drawnNodeNumber);
						lastTine2Node = clone;
						nodeNumber++;
					}
				}
			}
		}

		if (frame == 3) {
			if (GetComponent<Alpha>().tine3Collision == true) {
				if (Time.time > nextActionTime3) {
					nextActionTime3 = Time.time + drawPeriod;
					GameObject line;
					GameObject clone;
					if (GetComponent<Alpha>().tine3Nodes == 0) {
						line = Instantiate (drawnLine, new Vector3 (tine3.GetComponent<Transform>().position.x, 0, tine3.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
						clone = Instantiate (drawnNode, new Vector3 (tine3.GetComponent<Transform>().position.x, 0, tine3.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
						currentTine3Line = line; 
						line.GetComponent<DrawnLine>().drawnLineNumber = lineNumber;
						//Debug.Log ("Current Tine 3 line number: " + lineNumber);
						line.GetComponent<DrawnLine>().drawnDimension = rake.GetComponent<HyperColliderManager>().w;
						line.GetComponent<DrawnLine>().drawnStartNode = clone;
						clone.GetComponent<Node>().drawnNodeNumber = nodeNumber;
						//Debug.Log ("Tine 3 Line Node Number: Node " + clone.GetComponent<Node>().drawnNodeNumber);
						line.transform.parent = drawnPattern.transform;
						clone.transform.parent = line.transform;
						//Debug.Log ("Tine 3 node parent: Line " + clone.transform.parent.GetComponent<DrawnLine>().drawnLineNumber);
						GetComponent<Alpha>().tine3Nodes++;
						//Debug.Log ("Total Tine 3 nodes: " + GetComponent<Alpha>().tine3Nodes);
						//Debug.Log ("Total lines: " + lineNumber);
						//Debug.Log ("Total nodes: " + nodeNumber);
						lastTine3Node = clone;
						lineNumber++;
						nodeNumber++;
					} 

					else {
						clone = Instantiate (drawnNode, new Vector3 (tine3.GetComponent<Transform>().position.x, 0, tine3.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
						clone.GetComponent<Node>().drawnNodeNumber = nodeNumber;
						//Debug.Log ("Tine 3 Line Node Number: Node " + clone.GetComponent<Node>().drawnNodeNumber);
						clone.transform.parent = currentTine3Line.transform;
						//Debug.Log ("Tine 3 node parent: Line " + clone.transform.parent.GetComponent<DrawnLine>().drawnLineNumber);
						GetComponent<Alpha>().tine3Nodes++;
						//Debug.Log ("Total Tine 3 nodes: " + GetComponent<Alpha>().tine3Nodes);
						//Debug.Log ("Total nodes: " + nodeNumber);
						clone.GetComponent<Node>().previousNode = lastTine3Node;
						//Debug.Log ("Previous Tine 3 Node: Node " + clone.GetComponent<Node>().previousNode.GetComponent<Node>().drawnNodeNumber);
						lastTine3Node = clone;
						nodeNumber++;
					}
				}
			}
		}

		if (frame == 4) {
			if (GetComponent<Alpha>().tine4Collision == true) {
				if (Time.time > nextActionTime4) {
					nextActionTime4 = Time.time + drawPeriod;
					GameObject line;
					GameObject clone;
					if (GetComponent<Alpha>().tine4Nodes == 0) {
						line = Instantiate (drawnLine, new Vector3 (tine4.GetComponent<Transform>().position.x, 0, tine4.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
						clone = Instantiate (drawnNode, new Vector3 (tine4.GetComponent<Transform>().position.x, 0, tine4.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
						currentTine4Line = line; 
						line.GetComponent<DrawnLine>().drawnLineNumber = lineNumber;
						//Debug.Log ("Current Tine 4 line number: " + lineNumber);
						line.GetComponent<DrawnLine>().drawnDimension = rake.GetComponent<HyperColliderManager>().w;
						line.GetComponent<DrawnLine>().drawnStartNode = clone;
						clone.GetComponent<Node>().drawnNodeNumber = nodeNumber;
						//Debug.Log ("Tine 4 Line Node Number: Node " + clone.GetComponent<Node>().drawnNodeNumber);
						line.transform.parent = drawnPattern.transform;
						clone.transform.parent = line.transform;
						//Debug.Log ("Tine 4 node parent: Line " + clone.transform.parent.GetComponent<DrawnLine>().drawnLineNumber);
						GetComponent<Alpha>().tine4Nodes++;
						//Debug.Log ("Total Tine 4 nodes: " + GetComponent<Alpha>().tine4Nodes);
						//Debug.Log ("Total lines: " + lineNumber);
						//Debug.Log ("Total nodes: " + nodeNumber);
						lastTine4Node = clone;
						lineNumber++;
						nodeNumber++;
					} 

					else {
						clone = Instantiate (drawnNode, new Vector3 (tine4.GetComponent<Transform>().position.x, 0, tine4.GetComponent<Transform>().position.z), Quaternion.identity) as GameObject;
						clone.GetComponent<Node>().drawnNodeNumber = nodeNumber;
						//Debug.Log ("Tine 4 Line Node Number: Node " + clone.GetComponent<Node>().drawnNodeNumber);
						clone.transform.parent = currentTine4Line.transform;
						//Debug.Log ("Tine 4 node parent: Line " + clone.transform.parent.GetComponent<DrawnLine>().drawnLineNumber);
						GetComponent<Alpha>().tine4Nodes++;
						//Debug.Log ("Total Tine 4 nodes: " + GetComponent<Alpha>().tine4Nodes);
						//Debug.Log ("Total nodes: " + nodeNumber);
						clone.GetComponent<Node>().previousNode = lastTine4Node;
						//Debug.Log ("Previous Tine 4 Node: Node " + clone.GetComponent<Node>().previousNode.GetComponent<Node>().drawnNodeNumber);
						lastTine4Node = clone;
						nodeNumber++;
					}
				}
			}

			frame = 0;
		}

		if (Time.time > nextPatternCheck) {
			nextPatternCheck = Time.time + checkPeriod;

			if (checkDrawnPattern() >= 0.80f) {
				patternMatches = true;
				Debug.Log("Pattern matches.");
			} 

			else {
				Debug.Log("Pattern does not match.");
			}
		}

		frame++;
	}

	void calculateOriginalPatternVectors() {
		Debug.Log ("Alpha Dimension: " + GetComponent<HyperObject>().w);
		for (int i = 0; i <= originalPattern.transform.childCount; i++) {
			if (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternVectorFromStart = (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternEndNode.transform.position - originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternStartNode.transform.position);
				Debug.Log ("Pattern Line " + originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternLineNumber + " Vector From Start: " + originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternVectorFromStart);
				originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternVectorFromEnd = (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternStartNode.transform.position - originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternEndNode.transform.position);
				Debug.Log ("Pattern Line " + originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternLineNumber + " Vector From End: " + originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternVectorFromEnd);
			}
		}
	}

	float checkDrawnPattern() {
		float linesOnDimension = 0.0f;
		for (int i = 0; i < originalPattern.transform.childCount; i++) {
			if (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				linesOnDimension += 1.0f;
				float patternStartX = originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternVectorFromStart.x;
				float patternStartZ = originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternVectorFromStart.z;
				float patternEndX = originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternVectorFromEnd.x;
				float patternEndZ = originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternVectorFromEnd.z;
				float highestCheckValue = 0.0f;

				for (int j = 0; j < drawnPattern.transform.childCount; j++) {
					float thisCheckValue = 0.0f;
					if (drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnDimension == GetComponent<HyperObject>().w) {
						float drawnStartX = drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnVectorFromStart.x;
						float drawnStartZ = drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnVectorFromStart.z;
						float drawnEndX = drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnVectorFromEnd.x;
						float drawnEndZ = drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnVectorFromEnd.z;

						if ((Mathf.Abs(patternStartX - drawnStartX) <= rangeCheck || Mathf.Abs(patternStartX - drawnEndX) <= rangeCheck || Mathf.Abs(patternEndX - drawnStartX) <= rangeCheck || Mathf.Abs(patternEndX - drawnEndX) <= rangeCheck) && (Mathf.Abs(patternStartZ - drawnStartZ) <= rangeCheck || Mathf.Abs(patternStartZ - drawnEndZ) <= rangeCheck|| Mathf.Abs(patternEndZ - drawnStartZ) <= rangeCheck|| Mathf.Abs(patternEndZ - drawnEndZ) <= rangeCheck)) {
							Debug.Log("Line within range.");
							thisCheckValue += 0.5f;
							Debug.Log(thisCheckValue);
						}

						float section = (0.5f/(float)drawnPattern.transform.GetChild(j).transform.childCount);
						Debug.Log ("Section: " + section);

						if (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().isVertical == true) {
							for (int k = 0; k < drawnPattern.transform.GetChild(j).transform.childCount; k++) {
								if (Mathf.Abs(patternStartX - drawnPattern.transform.GetChild(j).GetChild(k).transform.position.x) <= rangeCheck) {
									Debug.Log("Node within range.");
									thisCheckValue += section;
									Debug.Log(thisCheckValue);
								}
							}
						} 

						else if (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().isVertical == false) {
							for (int k = 0; k < drawnPattern.transform.GetChild(j).transform.childCount; k++) {
								if (Mathf.Abs(patternStartZ - drawnPattern.transform.GetChild(j).GetChild(k).transform.position.z) <= rangeCheck) {
									Debug.Log("Node within range.");
									thisCheckValue += section;
									Debug.Log(thisCheckValue);
								}
							}
						}

						if (thisCheckValue > highestCheckValue) {
							Debug.Log (thisCheckValue + " is higher than " + highestCheckValue);
							highestCheckValue = thisCheckValue;
						}
					}
				}

				similarity += highestCheckValue;
			}
		}

		Debug.Log("Lines on dimension: " + linesOnDimension);

		float percent = similarity/linesOnDimension;
		similarity = 0.0f;
		Debug.Log("Pattern drawn by player is " + (percent * 100.0f) + "% similar to the designed pattern."); 

		return percent;
	}
}
