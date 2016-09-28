using UnityEngine;
using System.Collections;

public class PatternRecognition : MonoBehaviour {
	public GameObject drawnLine;
	public GameObject originalPattern;
	public GameObject drawnPattern;
	public float similarity;
	public bool patternMatches;
	public int[] bestLineMatches;
	public float linesOnDimension;
	public float rangeCheck;
	public int lineNumber;
	public int check;
	public int index;

	// Use this for initialization
	void Start() {
		linesOnDimension = 0.0f;
		for (int i = 0; i < originalPattern.transform.childCount; i++) {
			if (originalPattern.transform.GetChild (i).GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				linesOnDimension += 1.0f;
			}
		}

		check = 0;

		bestLineMatches = new int[(int)linesOnDimension];

		calculateOriginalPatternVectors();
	}

	// Update is called once per frame
	void Update() {
		if (GetComponent<Alpha>().checkPattern == true && check == 0) {
			Debug.Log ("Check pattern = true");
			if (checkDrawnPattern() >= 0.80f) {
				patternMatches = true;
				Debug.Log ("Current Plane: " + GetComponent<HyperColliderManager>().w);
				Debug.Log ("Pattern matches.");
				check = 1;
			} 

			else {
				Debug.Log ("Current Plane: " + GetComponent<HyperColliderManager>().w);
				Debug.Log ("Pattern does not match.");
				check = 1;
			}
		}
	}

	void calculateOriginalPatternVectors() {
		//Debug.Log ("Alpha Dimension: " + GetComponent<HyperObject>().w);
		for (int i = 0; i < originalPattern.transform.childCount; i++) {
			if (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternVector = (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternEndNode.transform.position - originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternStartNode.transform.position);
			}
		}
	}

	public float checkDrawnPattern() {
		index = 0;
		for (int i = 0; i < originalPattern.transform.childCount; i++) {
			if (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				float patternVectorX = originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternVector.x;
				float patternVectorZ = originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternVector.z;
				float highestCheckValue = 0.0f;
				int bestLine = 0;

				for (int j = 0; j < drawnPattern.transform.childCount; j++) {
					float thisCheckValue = 0.0f;
					if (drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnDimension == GetComponent<HyperObject>().w) {
						bool lineHasBeenMatched = false;

						foreach (int number in bestLineMatches) {
							if (drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnLineNumber == number) {
								lineHasBeenMatched = true;
							}
						}

						if (lineHasBeenMatched == true) {
							continue;
						}

						float drawnVectorX = drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnVector.x;
						float drawnVectorZ = drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnVector.z;

						if (Mathf.Abs(patternVectorX - drawnVectorX) <= rangeCheck && Mathf.Abs(patternVectorZ - drawnVectorZ) <= rangeCheck) {
							//Debug.Log("Line within range.");
							thisCheckValue += 0.5f;
							//Debug.Log(thisCheckValue);
						}

						float section = (0.5f/(float)drawnPattern.transform.GetChild(j).transform.childCount);
						//Debug.Log ("Section: " + section);

						if (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().isVertical == true) {
							for (int k = 0; k < drawnPattern.transform.GetChild(j).transform.childCount; k++) {
								if (Mathf.Abs(originalPattern.transform.GetChild(i).transform.position.x - drawnPattern.transform.GetChild(j).GetChild(k).transform.position.x) <= rangeCheck) {
									//Debug.Log("Node within range.");
									thisCheckValue += section;
									//Debug.Log(thisCheckValue);
								}
							}
						} 

						else if (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().isVertical == false) {
							for (int k = 0; k < drawnPattern.transform.GetChild(j).transform.childCount; k++) {
								if (Mathf.Abs(originalPattern.transform.GetChild(i).transform.position.z - drawnPattern.transform.GetChild(j).GetChild(k).transform.position.z) <= rangeCheck) {
									//Debug.Log("Node within range.");
									thisCheckValue += section;
									//Debug.Log(thisCheckValue);
								}
							}
						}

						if (thisCheckValue > highestCheckValue) {
							//Debug.Log (thisCheckValue + " is higher than " + highestCheckValue);
							highestCheckValue = thisCheckValue;
							//bestLine = drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnLineNumber;
						}
					}
				}

				bestLineMatches[index] = bestLine;
				if (index < (linesOnDimension - 1)) {
					index++;
				}

				similarity += highestCheckValue;
			}
		}

		//Debug.Log("Lines on dimension: " + linesOnDimension);

		float percent = similarity/linesOnDimension;
		similarity = 0.0f;
		Debug.Log("Pattern drawn by player is " + (percent * 100.0f) + "% similar to the designed pattern."); 

		return percent;
	}
}
