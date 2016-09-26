using UnityEngine;
using System.Collections;

public class ChimeHook : MonoBehaviour {

	GameObject attachedChime;

	/*
	 * sets this hook's focus to the given chime gameobject
	 */
	public void attachChime(GameObject g) {
		attachedChime = g;
	}

	/*
	 * nullifies the focused hook
	 */
	public void removeChime() {
		attachedChime = null;
	}

	/*
	 * plays the note on the attached hook if present
	 */
	public void playChime() {
		if(attachedChime != null)
			attachedChime.GetComponent<WindChime>().playNote();
	}
}
