using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Clock : MonoBehaviour {

	public GameObject kamimanager;

	public float gameMinutes;
	public float gameSeconds;

	float startTime;
	float gameDuration;

	public bool isActive = false;

	bool isEnding;

	const float KAMI_FLEE_TIME = 10.0f;

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
				if(Time.time > startTime + gameDuration + KAMI_FLEE_TIME) {
					endLevel();
				}
			}
		}
	}

	public void startClock() {
		startTime = Time.time;
		isActive = true;
	}

	void startLevelEnd() {
		isEnding = true;
		kamimanager.GetComponent<KamiManager>().MakeKamiEnd();
	}

	void endLevel() {
		isActive = false;

		//End the level if time is up
		SceneManager.LoadScene(HOME_LEVEL);
	}
}
