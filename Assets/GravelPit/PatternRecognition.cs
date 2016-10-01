using UnityEngine;
using System.Collections;

public class PatternRecognition : MonoBehaviour {
	public GameObject drawnLine;
	public GameObject originalPattern;
	public GameObject drawnPattern;
	public float similarity;
	public bool patternMatches;
	//public int[] bestLineMatches;
	public float linesOnDimension;
	public float rangeCheck;
	public int lineNumber;
	public int check;
	//public int index;
	public GravelShrine gravelShrine;

	// Use this for initialization
	void Start() {
		gravelShrine = Object.FindObjectOfType<GravelShrine>();

		linesOnDimension = 0.0f;
		for (int i = 0; i < originalPattern.transform.childCount; i++) {
			if (originalPattern.transform.GetChild (i).GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				linesOnDimension += 1.0f;
			}
		}

		check = 1;

		patternMatches = false;

		//bestLineMatches = new int[(int)linesOnDimension];

		calculateOriginalPatternVectors();
	}

	// Update is called once per frame
	void Update() {
		if (check == 0 && patternMatches == false) {
			//Debug.Log ("Check pattern = true");
			if (checkDrawnPattern() >= 0.80f) {
				patternMatches = true;
				//Debug.Log ("Current Plane: " + GetComponent<HyperColliderManager>().w);
				Debug.Log ("Pattern matches.");
				gravelShrine.processPits();
				check = 1;
			} 

			else {
				//Debug.Log ("Current Plane: " + GetComponent<HyperColliderManager>().w);
				Debug.Log ("Pattern does not match.");
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
			if (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternDimension == GetComponent<HyperObject>().w) {
				//Debug.Log("Pattern line on dimension " + GetComponent<HyperObject>().w);
				float patternVectorX = originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternVector.x;
				float patternVectorZ = originalPattern.transform.GetChild(i).GetComponent<PatternLine>().patternVector.z;
				float highestCheckValue = 0.0f;
				//int bestLine = 0;

				for (int j = 0; j < drawnPattern.transform.childCount; j++) {
					float thisCheckValue = 0.0f;
					if (drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnDimension == GetComponent<HyperObject>().w) {
						//Debug.Log("Drawn line on dimension " + GetComponent<HyperObject>().w);
						/*bool lineHasBeenMatched = false;

						foreach (int number in bestLineMatches) {
							if (drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnLineNumber == number) {
								lineHasBeenMatched = true;
							}
						}

						if (lineHasBeenMatched == true) {
							continue;
						}*/

						float drawnVectorX = drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnVector.x;
						float drawnVectorZ = drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnVector.z;

						float section = (0.5f/(float)drawnPattern.transform.GetChild(j).transform.childCount);
						//Debug.Log ("Section: " + section);

						if (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().isVertical == true) {
							int checkedVector = 0;
							for (int k = 0; k < drawnPattern.transform.GetChild(j).transform.childCount; k++) {
								float patternXPosition = originalPattern.transform.GetChild(i).transform.position.x;
								float patternStartZ = originalPattern.transform.GetChild(i).GetChild(0).transform.position.z;
								float patternEndZ = originalPattern.transform.GetChild(i).GetChild(1).transform.position.z;
								float drawnXPosition = drawnPattern.transform.GetChild(j).GetChild(k).transform.position.x;
								float drawnZPosition = drawnPattern.transform.GetChild(j).GetChild(k).transform.position.z;
								if (drawnXPosition >= 0) {
									if (Mathf.Abs(patternXPosition - drawnXPosition) <= rangeCheck) {
										if (drawnZPosition >= (patternStartZ - rangeCheck) && drawnZPosition <= (patternEndZ + rangeCheck)) {
											//Debug.Log("Drawn node's X within range.");
											//Debug.Log("Node within range.");
											thisCheckValue += section;
											//Debug.Log(thisCheckValue);
										}

										if (checkedVector == 0) {
											if (Mathf.Abs(Mathf.Abs(patternVectorX) - Mathf.Abs(drawnVectorX)) <= rangeCheck && Mathf.Abs(Mathf.Abs(patternVectorZ) - Mathf.Abs(drawnVectorZ)) <= rangeCheck) {
												//Debug.Log("Drawn vector within range.");
												//Debug.Log("Line within range.");
												thisCheckValue += 0.5f;
												//Debug.Log(thisCheckValue);
											}
											checkedVector = 1;
										}
									}
								}

								else if (drawnXPosition < 0) {
									if (Mathf.Abs(drawnXPosition - patternXPosition) <= rangeCheck) {
										if (drawnZPosition >= (patternStartZ - rangeCheck) && drawnZPosition <= (patternEndZ + rangeCheck)) {
											//Debug.Log("Drawn node's X within range.");
											//Debug.Log("Node within range.");
											thisCheckValue += section;
											//Debug.Log(thisCheckValue);
										}

										if (checkedVector == 0) {
											if (Mathf.Abs(Mathf.Abs(patternVectorX) - Mathf.Abs(drawnVectorX)) <= rangeCheck && Mathf.Abs(Mathf.Abs(patternVectorZ) - Mathf.Abs(drawnVectorZ)) <= rangeCheck) {
												//Debug.Log("Drawn vector within range.");
												//Debug.Log("Line within range.");
												thisCheckValue += 0.5f;
												//Debug.Log(thisCheckValue);
											}
											checkedVector = 1;
										}
									}
								}
							}
						} 

						else if (originalPattern.transform.GetChild(i).GetComponent<PatternLine>().isVertical == false) {
							int checkedVector = 0;
							for (int k = 0; k < drawnPattern.transform.GetChild(j).transform.childCount; k++) {
								float patternZPosition = originalPattern.transform.GetChild(i).transform.position.z;
								float patternStartX = originalPattern.transform.GetChild(i).GetChild(0).transform.position.x;
								float patternEndX = originalPattern.transform.GetChild(i).GetChild(1).transform.position.x;
								float drawnZPosition = drawnPattern.transform.GetChild(j).GetChild(k).transform.position.z;
								float drawnXPosition = drawnPattern.transform.GetChild(j).GetChild(k).transform.position.x;
								if (drawnZPosition >= 0) {
									if (Mathf.Abs(patternZPosition - drawnZPosition) <= rangeCheck) {
										if (drawnXPosition >= (patternStartX + rangeCheck) && drawnXPosition <= (patternEndX - rangeCheck)) {
											//Debug.Log("Drawn node's Z within range.");
											//Debug.Log("Node within range.");
											thisCheckValue += section;
											//Debug.Log(thisCheckValue);
										}

										if (checkedVector == 0) {
											if (Mathf.Abs(Mathf.Abs(patternVectorX) - Mathf.Abs(drawnVectorX)) <= rangeCheck && Mathf.Abs(Mathf.Abs(patternVectorZ) - Mathf.Abs(drawnVectorZ)) <= rangeCheck) {
												//Debug.Log("Drawn vector within range.");
												//Debug.Log("Line within range.");
												thisCheckValue += 0.5f;
												//Debug.Log(thisCheckValue);
											}
											checkedVector = 1;
										}
									}
								}

								else if (drawnZPosition < 0) {
									if (Mathf.Abs(drawnZPosition - patternZPosition) <= rangeCheck) {
										if (drawnXPosition >= (patternStartX + rangeCheck) && drawnXPosition <= (patternEndX - rangeCheck)) {
											//Debug.Log("Drawn node's Z within range.");
											//Debug.Log("Node within range.");
											thisCheckValue += section;
											//Debug.Log(thisCheckValue);
										}

										if (checkedVector == 0) {
											if (Mathf.Abs(Mathf.Abs(patternVectorX) - Mathf.Abs(drawnVectorX)) <= rangeCheck && Mathf.Abs(Mathf.Abs(patternVectorZ) - Mathf.Abs(drawnVectorZ)) <= rangeCheck) {
												//Debug.Log("Drawn vector within range.");
												//Debug.Log("Line within range.");
												thisCheckValue += 0.5f;
												//Debug.Log(thisCheckValue);
											}
											checkedVector = 1;
										}
									}
								}
							}
						}

						//Debug.Log("This check value: " + thisCheckValue);

						if (thisCheckValue > highestCheckValue) {
							//Debug.Log (thisCheckValue + " is higher than " + highestCheckValue);
							highestCheckValue = thisCheckValue;

							//bestLine = drawnPattern.transform.GetChild(j).GetComponent<DrawnLine>().drawnLineNumber;
						}
					}
				}

				/*bestLineMatches[index] = bestLine;
				if (index < (linesOnDimension - 1)) {
					index++;
				}*/

				//Debug.Log("Highest Check Value: " + highestCheckValue);

				similarity += highestCheckValue;

				//Debug.Log("Current similarity: " + similarity);
			}
		}

		//Debug.Log("Lines on dimension: " + linesOnDimension);
		//Debug.Log("Similarity: " + similarity);
		//Debug.Log("Lines on Dimension: " + linesOnDimension);
		float percent = similarity/linesOnDimension;
		similarity = 0.0f;
		Debug.Log("Pattern drawn by player is " + (percent * 100.0f) + "% similar to pattern " + GetComponent<HyperObject>().w); 

		return percent;
	}
}
