﻿using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Clock : MonoBehaviour {

	public float gameMinutes;
	public float gameSeconds;

	float startTime;
	float gameDuration;

	public bool isActive = false;

	bool isEnding;

	const string HOME_LEVEL = "Home";

	void Awake() {
		isEnding = false;
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
			if(!isEnding) {
				//Minute Hand
				transform.GetChild(0).GetChild(0).localRotation = Quaternion.Euler(new Vector3(0.0f, 360.0f * ((Time.time - startTime) / gameDuration), 0.0f));

				//check for gameover
				if(Time.time > startTime + gameDuration) {
					startLevelEnd();
				}
			}
			else {
				if(KamiManager.instance.getNumberOfKami() <= 0) {
					endLevel();
				}
			}
		}
	}

	/*
	 * Sets the clock to start counting and moving
	 */
	public void startClock() {
		startTime = Time.time;
		isActive = true;
	}

	/*
	 * Begin the level ending sequence and update highscore if necessary
	 */
	void startLevelEnd() {
		isEnding = true;

		//Save Kami Count
		if(PlayerPrefs.GetInt("KamiHighscore") < KamiManager.instance.getNumberOfKami()) {
			PlayerPrefs.SetInt("KamiHighscore", KamiManager.instance.getNumberOfKami());
		}

		//Start Kami End Sequence
		KamiManager.instance.MakeKamiEnd();
	}

	/*
	 * Return to the home level
	 */
	void endLevel() {
		isActive = false;

		//End the level if time is up
		SceneManager.LoadScene(HOME_LEVEL);
	}
}
