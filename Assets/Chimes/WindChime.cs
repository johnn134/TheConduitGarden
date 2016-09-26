using UnityEngine;
using System.Collections;

public class WindChime : MonoBehaviour {

	public AudioClip[] chimeSounds;

	GameObject hook;

	int note;
	int noteRange;

	bool isHanging;
	bool isHeld;

	const float CHIME_SCALE_FACTOR = 0.05f;

	void Awake() {
		isHanging = false;
		isHeld = false;
		noteRange = chimeSounds.Length;
		note = (int)(noteRange / 2);
	}

	void OnTriggerStay(Collider other) {
		if(!isHeld && !isHanging) {
			if(other.name.Substring(0, 9).Equals("ChimeHook")) {
				hangChime(other.gameObject);	//hang chime if not held or hanging
			}
		}
	}

	/*
	 * Hangs this chime on the given hook
	 */
	void hangChime(GameObject g) {
		isHanging = true;
		GetComponent<Rigidbody>().useGravity = false;
		hook = g;
		transform.position = g.transform.position;
		g.GetComponent<ChimeHook>().attachChime(gameObject);
	}

	/*
	 * called by controllers when grabbing this chime
	 */
	public void grabChime() {
		isHeld = true;
		if(isHanging) {
			isHanging = false;
			hook.GetComponent<ChimeHook>().removeChime();
		}
	}

	/*
	 * called by controllers when dropping this chime
	 */
	public void dropChime() {
		isHeld = false;
		GetComponent<Rigidbody>().useGravity = true;
	}

	/*
	 * set this chime's note to the next highest note
	 */
	public void increaseNote() {
		if(note < noteRange - 1) {
			note++;
			updateChime();
		}
	}

	/*
	 * set this chime's note to the next lowest note
	 */
	public void decreaseNote() {
		if(note > 0) {
			note--;
			updateChime();
		}
	}

	/*
	 * set the chime to its selected note and update visual appearence
	 */
	void updateChime() {
		//Change audio clip
		GetComponent<AudioSource>().clip = chimeSounds[note];

		//Change scale
		float newScale = 1.0f + CHIME_SCALE_FACTOR * (note - (int)(noteRange / 2));
		transform.GetChild(0).GetChild(0).localScale = new Vector3(newScale, newScale, newScale * 1.2795f);

		//move bottom collider
		float newPos = -0.2f - CHIME_SCALE_FACTOR * 0.15f * (note - (int)(noteRange / 2));
		transform.GetChild(1).localPosition = new Vector3(0, newPos, 0);
	}

	/*
	 * calls the audiosource on this chime to play the given note
	 */
	public void playNote() {
		GetComponent<AudioSource>().Play();
	}

	public bool getIsHanging() {
		return isHanging;
	}
}
