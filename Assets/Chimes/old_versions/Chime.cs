using UnityEngine;
using System.Collections;

public class Chime : MonoBehaviour {

    public float offset;
    public string note;
    public bool isSelected;
    public bool isPlaying;
    public float maxSize;
    public float minSize;
    public float height;
    public GameObject[] chimes;
	public AudioClip note1;
	public AudioClip note2;
	public AudioClip note3;
	public AudioClip note4;
	public AudioClip note5;
	public AudioClip note6;
	public AudioClip note7;
	public AudioClip note8;
	public AudioClip note9;
	public AudioClip note10;
	public AudioClip note11;
	public AudioClip note12;
	public AudioClip note13;

	// Use this for initialization
	void Start() {
        note = "note_1";
		GetComponent<AudioSource>().clip = note1;

		isSelected = false;
        isPlaying = false;

		chimes = GameObject.FindGameObjectsWithTag("Chime");
	}

    // Update is called once per frame
    void Update() {
		if ((int)Time.time % 5 == offset && Time.time >= 5) {
            isPlaying = true;

			PlaySound();

            isPlaying = false;
        }

		if (Input.GetKeyDown(KeyCode.UpArrow) == true) {
            if (isSelected == true && isPlaying == false) {
				IncreasePitch();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) == true) {
            if (isSelected == true && isPlaying == false) {
				DecreasePitch();
            }
        }
    }

	void PlaySound() {
		GetComponent<AudioSource>().Play();
	}

	public void IncreasePitch() {
		switch (note) {
			case "note_1":
				gameObject.transform.localScale = new Vector3(minSize + (11 * (maxSize - minSize) / 12), minSize + (11 * (maxSize - minSize) / 12), minSize + (11 * (maxSize - minSize) / 12));
                note = "note_2";
				Debug.Log("HERE");
				GetComponent<AudioSource>().clip = note2;
				break;
			case "note_2":
				gameObject.transform.localScale = new Vector3(minSize + (10 * (maxSize - minSize) / 12), minSize + (10 * (maxSize - minSize) / 12), minSize + (10 * (maxSize - minSize) / 12));
                note = "note_3";
				GetComponent<AudioSource>().clip = note3;
				break;
			case "note_3":
				gameObject.transform.localScale = new Vector3(minSize + (9 * (maxSize - minSize) / 12), minSize + (9 * (maxSize - minSize) / 12), minSize + (9 * (maxSize - minSize) / 12));
                note = "note_4";
				GetComponent<AudioSource>().clip = note4;
				break;
			case "note_4":
				gameObject.transform.localScale = new Vector3(minSize + (8 * (maxSize - minSize) / 12), minSize + (8 * (maxSize - minSize) / 12), minSize + (8 * (maxSize - minSize) / 12));
                note = "note_5";
				GetComponent<AudioSource>().clip = note5;
				break;
			case "note_5":
				gameObject.transform.localScale = new Vector3(minSize + (7 * (maxSize - minSize) / 12), minSize + (7 * (maxSize - minSize) / 12), minSize + (7 * (maxSize - minSize) / 12));
                note = "note_6";
				GetComponent<AudioSource>().clip = note6;
				break;
			case "note_6":
				gameObject.transform.localScale = new Vector3(minSize + (6 * (maxSize - minSize) / 12), minSize + (6 * (maxSize - minSize) / 12), minSize + (6 * (maxSize - minSize) / 12));
                note = "note_7";
				GetComponent<AudioSource>().clip = note7;
				break;
			case "note_7":
				gameObject.transform.localScale = new Vector3(minSize + (5 * (maxSize - minSize) / 12), minSize + (5 * (maxSize - minSize) / 12), minSize + (5 * (maxSize - minSize) / 12));
                note = "note_8";
				GetComponent<AudioSource>().clip = note8;
				break;
			case "note_8":
				gameObject.transform.localScale = new Vector3(minSize + (4 * (maxSize - minSize) / 12), minSize + (4 * (maxSize - minSize) / 12), minSize + (4 * (maxSize - minSize) / 12));
                note = "note_9";
				GetComponent<AudioSource>().clip = note9;
				break;
			case "note_9":
				gameObject.transform.localScale = new Vector3(minSize + (3 * (maxSize - minSize) / 12), minSize + (3 * (maxSize - minSize) / 12), minSize + (3 * (maxSize - minSize) / 12));
                note = "note_10";
				GetComponent<AudioSource>().clip = note10;
				break;
			case "note_10":
				gameObject.transform.localScale = new Vector3(minSize + (2*(maxSize - minSize) / 12), minSize + (2 * (maxSize - minSize) / 12), minSize + (2*(maxSize - minSize) / 12));
                note = "note_11";
				GetComponent<AudioSource>().clip = note11;
				break;
			case "note_11":
				gameObject.transform.localScale = new Vector3(minSize + (maxSize-minSize)/12, minSize + (maxSize - minSize) / 12, minSize + (maxSize - minSize) / 12);
				note = "note_12";
				GetComponent<AudioSource>().clip = note12;
				break;
			case "note_12":
				gameObject.transform.localScale = new Vector3(minSize, minSize, minSize);
				note = "note_13";
				GetComponent<AudioSource>().clip = note13;
				break;
			case "note_13":
				note = "note_13";
				GetComponent<AudioSource>().clip = note13;
				break;
			default:
				note = "note_1";
				GetComponent<AudioSource>().clip = note1;
				break;
		}
	}

	void DecreasePitch() {
		switch (note) {
			case "note_1":
				note = "note_1";
				GetComponent<AudioSource>().clip = note1;
				break;
			case "note_2":
				gameObject.transform.localScale = new Vector3(maxSize, maxSize, maxSize);
                note = "note_1";
				GetComponent<AudioSource>().clip = note1;
				break;
			case "note_3":
				gameObject.transform.localScale = new Vector3(maxSize - (1 * (maxSize - minSize) / 12), maxSize - (1 * (maxSize - minSize) / 12), maxSize - (1 * (maxSize - minSize) / 12));
                note = "note_2";
				GetComponent<AudioSource>().clip = note2;
				break;
			case "note_4":
				gameObject.transform.localScale = new Vector3(maxSize - (2 * (maxSize - minSize) / 12), maxSize - (2 * (maxSize - minSize) / 12), maxSize - (2 * (maxSize - minSize) / 12));
                note = "note_3";
				GetComponent<AudioSource>().clip = note3;
				break;
			case "note_5":
				gameObject.transform.localScale = new Vector3(maxSize - (3 * (maxSize - minSize) / 12), maxSize - (3 * (maxSize - minSize) / 12), maxSize - (3 * (maxSize - minSize) / 12));
                note = "note_4";
				GetComponent<AudioSource>().clip = note4;
				break;
			case "note_6":
				gameObject.transform.localScale = new Vector3(maxSize - (4 * (maxSize - minSize) / 12), maxSize - (4 * (maxSize - minSize) / 12), maxSize - (4 * (maxSize - minSize) / 12));
                note = "note_5";
				GetComponent<AudioSource>().clip = note5;
				break;
			case "note_7":
				gameObject.transform.localScale = new Vector3(maxSize - (5 * (maxSize - minSize) / 12), maxSize - (5 * (maxSize - minSize) / 12), maxSize - (5 * (maxSize - minSize) / 12));
                note = "note_6";
				GetComponent<AudioSource>().clip = note6;
				break;
			case "note_8":
				gameObject.transform.localScale = new Vector3(maxSize - (6 * (maxSize - minSize) / 12), maxSize - (6 * (maxSize - minSize) / 12), maxSize - (6 * (maxSize - minSize) / 12));
                note = "note_7";
				GetComponent<AudioSource>().clip = note7;
				break;
			case "note_9":
				gameObject.transform.localScale = new Vector3(maxSize - (7 * (maxSize - minSize) / 12), maxSize - (7 * (maxSize - minSize) / 12), maxSize - (7 * (maxSize - minSize) / 12));
                note = "note_8";
				GetComponent<AudioSource>().clip = note8;
				break;
			case "note_10":
				gameObject.transform.localScale = new Vector3(maxSize - (8 * (maxSize - minSize) / 12), maxSize - (8 * (maxSize - minSize) / 12), maxSize - (8 * (maxSize - minSize) / 12));
                note = "note_9";
				GetComponent<AudioSource>().clip = note9;
				break;
			case "note_11":
				gameObject.transform.localScale = new Vector3(maxSize - (9 * (maxSize - minSize) / 12), maxSize - (9 * (maxSize - minSize) / 12), maxSize - (9 * (maxSize - minSize) / 12));
                note = "note_10";
				GetComponent<AudioSource>().clip = note10;
				break;
			case "note_12":
				gameObject.transform.localScale = new Vector3(maxSize - (10 * (maxSize - minSize) / 12), maxSize - (10 * (maxSize - minSize) / 12), maxSize - (10 * (maxSize - minSize) / 12));
                note = "note_11";
				GetComponent<AudioSource>().clip = note11;
				break;
			case "note_13":
				gameObject.transform.localScale = new Vector3(maxSize - (11 * (maxSize - minSize) / 12), maxSize - (11 * (maxSize - minSize) / 12), maxSize - (11 * (maxSize - minSize) / 12));
                note = "note_12";
				GetComponent<AudioSource>().clip = note12;
				break;
			default:
				note = "note_1";
				GetComponent<AudioSource>().clip = note1;
				break;
		}
	}

    void OnMouseDown() {
        foreach (GameObject chime in chimes) {
            chime.GetComponent<Chime>().isSelected = false;
        }

        isSelected = true;
    }
}