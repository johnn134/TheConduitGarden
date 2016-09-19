using UnityEngine;
using System.Collections;

public class PatternRecognition : MonoBehaviour {
	public GameObject alpha;
	public GameObject tine1;
	public GameObject tine2;
	public GameObject tine3;
	public GameObject tine4;
	public Transform node;
	public GameObject[] originalPattern;
	public GameObject[] drawnPattern;
	public int frame;
	public float similarity;
	public bool patternMatches;
	public float nextActionTime1;
	public float nextActionTime2;
	public float nextActionTime3;
	public float nextActionTime4;
	public float nextPatternCheck;
	public float drawPeriod;
	public float checkPeriod;

	// Use this for initialization
	void Start() {
		originalPattern = GameObject.FindGameObjectsWithTag("Pattern Node");
		frame = 1;
		drawPeriod = 5.0f;
		checkPeriod = 4.0f;
	}
	
	// Update is called once per frame
	void Update() {
		if (frame == 1) {
			if (alpha.GetComponent<Alpha>().tine1Collision == true) {
				if (Time.time > nextActionTime1) {
					nextActionTime1 = Time.time + drawPeriod;
					GameObject clone;
					clone = Instantiate (node, new Vector3 (tine1.GetComponent<Transform> ().position.x, 0, tine1.GetComponent<Transform> ().position.z), Quaternion.identity) as GameObject;

				}
			}
		}

		if (frame == 2) {
			if (alpha.GetComponent<Alpha>().tine2Collision == true) {
				if (Time.time > nextActionTime2) {
					nextActionTime2 = Time.time + drawPeriod;
					GameObject clone;
					clone = Instantiate (node, new Vector3 (tine2.GetComponent<Transform> ().position.x, 0, tine2.GetComponent<Transform> ().position.z), Quaternion.identity) as GameObject;
				}
			}
		}

		if (frame == 3) {
			if (alpha.GetComponent<Alpha>().tine3Collision == true) {
				if (Time.time > nextActionTime3) {
					nextActionTime3 = Time.time + drawPeriod;
					GameObject clone;
					clone = Instantiate (node, new Vector3 (tine3.GetComponent<Transform> ().position.x, 0, tine3.GetComponent<Transform> ().position.z), Quaternion.identity) as GameObject;
				}
			}
		}

		if (frame == 4) {
			if (alpha.GetComponent<Alpha>().tine4Collision == true) {
				if (Time.time > nextActionTime4) {
					nextActionTime4 = Time.time + drawPeriod;
					GameObject clone;
					clone = Instantiate (node, new Vector3 (tine4.GetComponent<Transform> ().position.x, 0, tine4.GetComponent<Transform> ().position.z), Quaternion.identity) as GameObject;
				}
			}

			frame = 0;
		}
			
		drawnPattern = GameObject.FindGameObjectsWithTag("Drawn Node");

		if (Time.time > nextPatternCheck) {
			nextPatternCheck = Time.time + checkPeriod;

			if (checkPattern() >= 0.80f) {
				patternMatches = true;
				Debug.Log("Pattern matches.");
			} 

			else {
				Debug.Log("Pattern does not match.");
			}
		}
			
		frame++;
	}

	float checkPattern() {
		Debug.Log(originalPattern.Length + " nodes in designed pattern.");
		Debug.Log(drawnPattern.Length + " nodes drawn so far.");

		foreach (GameObject patternNode in originalPattern) {
			float patternX = patternNode.GetComponent<Transform>().position.x;
			float patternZ = patternNode.GetComponent<Transform>().position.z;

			foreach (GameObject drawnNode in drawnPattern) {
				float drawnX = drawnNode.GetComponent<Transform>().position.x;
				float drawnZ = drawnNode.GetComponent<Transform>().position.z;

				if (drawnX >= (patternX - 0.02f) && drawnX <= (patternX + 0.02f) && drawnZ >= (patternZ - 0.02f) && drawnZ <= (patternZ + 0.02f)) {
					similarity++;
					break;
				}
			}
		}
			
		float percent = similarity/(float)originalPattern.Length;
		similarity = 0;
		Debug.Log("Pattern drawn by player is " + (percent * 100.0f) + "% similar to the designed pattern."); 

		return percent;
	}
}
