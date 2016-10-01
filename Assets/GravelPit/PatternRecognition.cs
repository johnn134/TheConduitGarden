using UnityEngine;
using System.Collections;

public class PatternRecognition : MonoBehaviour {
	private GameObject rake;
	public GameObject drawnLine;
	public GameObject originalPattern;
	public GameObject drawnPattern;
	public bool patternMatches;
	//private int[] bestLineMatches;
	public float linesOnDimension;
	public float rangeCheck;
	public int lineNumber;
	public int check;
	//private int index;
	//private int bestLine;
	public GravelShrine gravelShrine;
	private float similarity;

	// Use this for initialization
	void Start() {
		rake = GameObject.Find("Rake");
		gravelShrine = Object.FindObjectOfType<GravelShrine>();

		linesOnDimension = 0.0f;
		for (int i = 0; i < originalPattern.transform.childCount; i++) {
			if (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				linesOnDimension += 1.0f;
			}
		}

		check = 1;

		rangeCheck = 0.5f;

		patternMatches = false;

		/*bestLineMatches = new int[(int)linesOnDimension];

		for (int b = 0; b < (int)linesOnDimension; b++) {
			bestLineMatches[b] = 0;
		}*/

		calculateOriginalPatternVectors();
	}

	// Update is called once per frame
	void Update() {
		if (check == 0 && patternMatches == false) {
			//Debug.Log ("Check pattern = true");
			if (checkDrawnPattern() >= 0.80f) {
				patternMatches = true;
				//Debug.Log ("Current Plane: " + GetComponent<HyperColliderManager>().w);
				Debug.Log("Pattern matches.");
				gravelShrine.processPits();
				check = 1;
			}

			else {
				//Debug.Log ("Current Plane: " + GetComponent<HyperColliderManager>().w);
				Debug.Log("Pattern does not match.");
				patternMatches = false;
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
		//index = 0;
		for (int i = 0; i < originalPattern.transform.childCount; i++) {
			if (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternDimension == rake.GetComponent<HyperColliderManager>().w) {
				float highestCheckValue = 0.0f;

				for (int j = 0; j < drawnPattern.transform.childCount; j++) {

					float thisCheckValue = 0.0f;

					/*bool lineHasBeenMatched = false;

					foreach (int number in bestLineMatches) {
						if (drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnLineNumber == number) {
							lineHasBeenMatched = true;
						}
					}

					if (lineHasBeenMatched == true) {
						//Debug.Log("Already matched this line. Continuing.");
						continue;
					}*/

					//Debug.Log ("Line was not matched yet.");
					if (drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnDimension == rake.GetComponent<HyperColliderManager>().w) {
						float patternStartX = originalPattern.transform.GetChild(i).GetChild(0).transform.position.x;
						float patternStartZ = originalPattern.transform.GetChild(i).GetChild(0).transform.position.z;
						float patternVectorX = originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternVector.x;
						float patternVectorZ = originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternVector.z;
						float drawnStartX = drawnPattern.transform.GetChild(j).GetChild(0).transform.position.x;
						float drawnStartZ = drawnPattern.transform.GetChild(j).GetChild(0).transform.position.z;
						float drawnVectorX = drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnVector.x;
						float drawnVectorZ = drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnVector.z;
						//float section = (0.5f / (float)drawnPattern.transform.GetChild(j).transform.childCount);

						if (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().isVertical == true) {
							if (drawnStartX >= (patternStartX - rangeCheck) && drawnStartX <= (patternStartX + rangeCheck) && drawnStartZ >= (patternStartZ - rangeCheck) && drawnStartZ <= (patternStartZ + rangeCheck)) {
								if (Mathf.Abs(Mathf.Abs(patternVectorX) - Mathf.Abs(drawnVectorX)) <= rangeCheck && Mathf.Abs(Mathf.Abs(patternVectorZ) - Mathf.Abs(drawnVectorZ)) <= rangeCheck) {
									thisCheckValue += 1.0f;
									//Debug.Log("Vector matched.");
								}
							}
						}

						else if (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().isVertical == false) {
							if (drawnStartX >= (patternStartX - rangeCheck) && drawnStartX <= (patternStartX + rangeCheck) && drawnStartZ >= (patternStartZ - rangeCheck) && drawnStartZ <= (patternStartZ + rangeCheck)) {
								if (Mathf.Abs(Mathf.Abs(patternVectorX) - Mathf.Abs(drawnVectorX)) <= rangeCheck && Mathf.Abs(Mathf.Abs(patternVectorZ) - Mathf.Abs(drawnVectorZ)) <= rangeCheck) {
									thisCheckValue += 1.0f;
									//Debug.Log("Vector matched.");
								}
							}
						}
					}

					//Debug.Log("This Check Value: " + thisCheckValue);
					if (thisCheckValue > highestCheckValue) {
						highestCheckValue = thisCheckValue;
						//bestLine = drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnLineNumber;
					}
				}

				/*bestLineMatches[index] = bestLine;
				if (index < (linesOnDimension - 1)) {
					index++;
				}*/

				similarity += highestCheckValue;
				//Debug.Log("Current Similarity: " + similarity);
			}
		}

		float percent = (similarity / linesOnDimension);
		Debug.Log("Percent: " + percent);
		similarity = 0.0f;
		return percent;
	}
}

