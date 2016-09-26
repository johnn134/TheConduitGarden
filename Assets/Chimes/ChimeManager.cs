using UnityEngine;
using System.Collections;

public class ChimeManager : MonoBehaviour {

	public GameObject[] hooks;

	public float loopTime;	//time offset for looping chime notes

	float lastPlayTime;		//last time at the end of a loop cycle

	int curHook;			//current hook to be activated

	const float CHIME_PLAY_OFFSET = 0.25f;

	public static ChimeManager instance = null;

	void Awake() {
		//declare as singleton
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		lastPlayTime = 0;
		curHook = 0;
	}
	
	// Update is called once per frame
	void Update () {
		//after the wait time between loops, play the note on each hook's chime
		if(Time.time > lastPlayTime + loopTime + CHIME_PLAY_OFFSET * curHook) {
			hooks[curHook].GetComponent<ChimeHook>().playChime();
			curHook++;

			if(curHook >= hooks.Length) {	//reset the wait time between loops
				lastPlayTime = Time.time;
				curHook = 0;
			}
		}
	}
}
