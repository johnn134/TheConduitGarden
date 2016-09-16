using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour {

	public float gameMinutes;
	public float gameSeconds;

	public bool isActive = false;

	float startTime;
	float gameDuration;

	void Awake() {
		gameDuration = (gameMinutes * 60.0f) + gameSeconds;
	}

	// Use this for initialization
	void Start () {
		if(isActive)
			startClock();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(isActive) {
			//Minute Hand
			transform.GetChild(0).GetChild(0).localRotation = Quaternion.Euler(new Vector3(0.0f, 360.0f * ((Time.time - startTime) / gameDuration), 0.0f));

			//check for gameover
			if(Time.time > gameDuration + startTime) {
				endLevel();
			}
		}
	}

	public void startClock() {
		startTime = Time.time;
		isActive = true;
	}

	void endLevel() {
		isActive = false;

		//End the level if time is up
	}
}
