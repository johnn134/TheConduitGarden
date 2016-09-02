using UnityEngine;
using System.Collections;

public class Chime2 : MonoBehaviour {

    public float offset;
    public string note;
    public bool isSelected;
    public bool isPlaying;
    public float maxSize;
    public float minSize;
    public float height;
    public GameObject[] chimes;
    public AudioSource c_chime_low;
    public AudioSource c_sharp_chime;
    public AudioSource d_chime;
    public AudioSource d_sharp_chime;
    public AudioSource e_chime;
    public AudioSource f_chime;
    public AudioSource f_sharp_chime;
    public AudioSource g_chime;
    public AudioSource g_sharp_chime;
    public AudioSource a_chime;
    public AudioSource a_sharp_chime;
    public AudioSource b_chime;
    public AudioSource c_chime_high;

    // Use this for initialization
    void Start() {
        note = "c_chime_low";
        isSelected = false;
        isPlaying = false;

		chimes = GameObject.FindGameObjectsWithTag("Chime");
		SetChimeSounds();
    }

    // Update is called once per frame
    void Update() {
         if ((int)Time.time % 60 == offset && Time.time >= 60) {
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

	void SetChimeSounds() {
		c_chime_low = (AudioSource)gameObject.AddComponent<AudioSource>();
		AudioClip c_chime_low_clip;
		c_chime_low_clip = (AudioClip)Resources.Load("Sounds/c_chime_low");
		c_chime_low.clip = c_chime_low_clip;

		c_sharp_chime = (AudioSource)gameObject.AddComponent<AudioSource>();
		AudioClip c_sharp_chime_clip;
		c_sharp_chime_clip = (AudioClip)Resources.Load("Sounds/c_sharp_chime");
		c_sharp_chime.clip = c_sharp_chime_clip;

		d_chime = (AudioSource)gameObject.AddComponent<AudioSource>();
		AudioClip d_chime_clip;
		d_chime_clip = (AudioClip)Resources.Load("Sounds/d_chime");
		d_chime.clip = d_chime_clip;

		d_sharp_chime = (AudioSource)gameObject.AddComponent<AudioSource>();
		AudioClip d_sharp_chime_clip;
		d_sharp_chime_clip = (AudioClip)Resources.Load("Sounds/d_sharp_chime");
		d_sharp_chime.clip = d_sharp_chime_clip;

		e_chime = (AudioSource)gameObject.AddComponent<AudioSource>();
		AudioClip e_chime_clip;
		e_chime_clip = (AudioClip)Resources.Load("Sounds/e_chime");
		e_chime.clip = e_chime_clip;

		f_chime = (AudioSource)gameObject.AddComponent<AudioSource>();
		AudioClip f_chime_clip;
		f_chime_clip = (AudioClip)Resources.Load("Sounds/f_chime");
		f_chime.clip = f_chime_clip;

		f_sharp_chime = (AudioSource)gameObject.AddComponent<AudioSource>();
		AudioClip f_sharp_chime_clip;
		f_sharp_chime_clip = (AudioClip)Resources.Load("Sounds/f_sharp_chime");
		f_sharp_chime.clip = f_sharp_chime_clip;

		g_chime = (AudioSource)gameObject.AddComponent<AudioSource>();
		AudioClip g_chime_clip;
		g_chime_clip = (AudioClip)Resources.Load("Sounds/g_chime");
		g_chime.clip = g_chime_clip;

		g_sharp_chime = (AudioSource)gameObject.AddComponent<AudioSource>();
		AudioClip g_sharp_chime_clip;
		g_sharp_chime_clip = (AudioClip)Resources.Load("Sounds/g_sharp_chime");
		g_sharp_chime.clip = g_sharp_chime_clip;

		a_chime = (AudioSource)gameObject.AddComponent<AudioSource>();
		AudioClip a_chime_clip;
		a_chime_clip = (AudioClip)Resources.Load("Sounds/a_chime");
		a_chime.clip = a_chime_clip;

		a_sharp_chime = (AudioSource)gameObject.AddComponent<AudioSource>();
		AudioClip a_sharp_chime_clip;
		a_sharp_chime_clip = (AudioClip)Resources.Load("Sounds/a_sharp_chime");
		a_sharp_chime.clip = a_sharp_chime_clip;

		b_chime = (AudioSource)gameObject.AddComponent<AudioSource>();
		AudioClip b_chime_clip;
		b_chime_clip = (AudioClip)Resources.Load("Sounds/b_chime");
		b_chime.clip = b_chime_clip;

		c_chime_high = (AudioSource)gameObject.AddComponent<AudioSource>();
		AudioClip c_chime_high_clip;
		c_chime_high_clip = (AudioClip)Resources.Load("Sounds/c_chime_high");
		c_chime_high.clip = c_chime_high_clip;
	}

	void PlaySound() {
		switch (note) {
			case "c_chime_low":
				c_chime_low.Play();
				break;
			case "c_sharp_chime":
				c_sharp_chime.Play();
				break;
			case "d_chime":
				d_chime.Play();
				break;
			case "d_sharp_chime":
				d_sharp_chime.Play();
				break;
			case "e_chime":
				e_chime.Play();
				break;
			case "f_chime":
				f_chime.Play();
				break;
			case "f_sharp_chime":
				f_sharp_chime.Play();
				break;
			case "g_chime":
				g_chime.Play();
				break;
			case "g_sharp_chime":
				g_sharp_chime.Play();
				break;
			case "a_chime":
				a_chime.Play();
				break;
			case "a_sharp_chime":
				a_sharp_chime.Play();
				break;
			case "b_chime":
				b_chime.Play();
				break;
			case "c_chime_high":
				c_chime_high.Play();
				break;
			default:
				c_chime_low.Play();
				break;
		}
	}

	public void IncreasePitch() {
		switch (note) {
			case "c_chime_low":
				gameObject.transform.localScale = new Vector3(minSize + (11 * (maxSize - minSize) / 12), minSize + (11 * (maxSize - minSize) / 12), minSize + (11 * (maxSize - minSize) / 12));
                note = "c_sharp_chime";
				break;
			case "c_sharp_chime":
				gameObject.transform.localScale = new Vector3(minSize + (10 * (maxSize - minSize) / 12), minSize + (10 * (maxSize - minSize) / 12), minSize + (10 * (maxSize - minSize) / 12));
                note = "d_chime";
				break;
			case "d_chime":
				gameObject.transform.localScale = new Vector3(minSize + (9 * (maxSize - minSize) / 12), minSize + (9 * (maxSize - minSize) / 12), minSize + (9 * (maxSize - minSize) / 12));
                note = "d_sharp_chime";
				break;
			case "d_sharp_chime":
				gameObject.transform.localScale = new Vector3(minSize + (8 * (maxSize - minSize) / 12), minSize + (8 * (maxSize - minSize) / 12), minSize + (8 * (maxSize - minSize) / 12));
                note = "e_chime";
				break;
			case "e_chime":
				gameObject.transform.localScale = new Vector3(minSize + (7 * (maxSize - minSize) / 12), minSize + (7 * (maxSize - minSize) / 12), minSize + (7 * (maxSize - minSize) / 12));
                note = "f_chime";
				break;
			case "f_chime":
				gameObject.transform.localScale = new Vector3(minSize + (6 * (maxSize - minSize) / 12), minSize + (6 * (maxSize - minSize) / 12), minSize + (6 * (maxSize - minSize) / 12));
                note = "f_sharp_chime";
				break;
			case "f_sharp_chime":
				gameObject.transform.localScale = new Vector3(minSize + (5 * (maxSize - minSize) / 12), minSize + (5 * (maxSize - minSize) / 12), minSize + (5 * (maxSize - minSize) / 12));
                note = "g_chime";
				break;
			case "g_chime":
				gameObject.transform.localScale = new Vector3(minSize + (4 * (maxSize - minSize) / 12), minSize + (4 * (maxSize - minSize) / 12), minSize + (4 * (maxSize - minSize) / 12));
                note = "g_sharp_chime";
				break;
			case "g_sharp_chime":
				gameObject.transform.localScale = new Vector3(minSize + (3 * (maxSize - minSize) / 12), minSize + (3 * (maxSize - minSize) / 12), minSize + (3 * (maxSize - minSize) / 12));
                note = "a_chime";
				break;
			case "a_chime":
				gameObject.transform.localScale = new Vector3(minSize + (2*(maxSize - minSize) / 12), minSize + (2 * (maxSize - minSize) / 12), minSize + (2*(maxSize - minSize) / 12));
                note = "a_sharp_chime";
				break;
			case "a_sharp_chime":
				gameObject.transform.localScale = new Vector3(minSize + (maxSize-minSize)/12, minSize + (maxSize - minSize) / 12, minSize + (maxSize - minSize) / 12);
				note = "b_chime";
				break;
			case "b_chime":
				gameObject.transform.localScale = new Vector3(minSize, minSize, minSize);
				note = "c_chime_high";
				break;
			case "c_chime_high":
				note = "c_chime_high";
				break;
			default:
				note = "c_chime_low";
				break;
		}
	}

	void DecreasePitch() {
		switch (note) {
			case "c_chime_low":
				note = "c_chime_low";
				break;
			case "c_sharp_chime":
				gameObject.transform.localScale = new Vector3(maxSize, maxSize, maxSize);
                note = "c_chime_low";
				break;
			case "d_chime":
				gameObject.transform.localScale = new Vector3(maxSize - (1 * (maxSize - minSize) / 12), maxSize - (1 * (maxSize - minSize) / 12), maxSize - (1 * (maxSize - minSize) / 12));
                note = "c_sharp_chime";
				break;
			case "d_sharp_chime":
				gameObject.transform.localScale = new Vector3(maxSize - (2 * (maxSize - minSize) / 12), maxSize - (2 * (maxSize - minSize) / 12), maxSize - (2 * (maxSize - minSize) / 12));
                note = "d_chime";
				break;
			case "e_chime":
				gameObject.transform.localScale = new Vector3(maxSize - (3 * (maxSize - minSize) / 12), maxSize - (3 * (maxSize - minSize) / 12), maxSize - (3 * (maxSize - minSize) / 12));
                note = "d_sharp_chime";
				break;
			case "f_chime":
				gameObject.transform.localScale = new Vector3(maxSize - (4 * (maxSize - minSize) / 12), maxSize - (4 * (maxSize - minSize) / 12), maxSize - (4 * (maxSize - minSize) / 12));
                note = "e_chime";
				break;
			case "f_sharp_chime":
				gameObject.transform.localScale = new Vector3(maxSize - (5 * (maxSize - minSize) / 12), maxSize - (5 * (maxSize - minSize) / 12), maxSize - (5 * (maxSize - minSize) / 12));
                note = "f_chime";
				break;
			case "g_chime":
				gameObject.transform.localScale = new Vector3(maxSize - (6 * (maxSize - minSize) / 12), maxSize - (6 * (maxSize - minSize) / 12), maxSize - (6 * (maxSize - minSize) / 12));
                note = "f_sharp_chime";
				break;
			case "g_sharp_chime":
				gameObject.transform.localScale = new Vector3(maxSize - (7 * (maxSize - minSize) / 12), maxSize - (7 * (maxSize - minSize) / 12), maxSize - (7 * (maxSize - minSize) / 12));
                note = "g_chime";
				break;
			case "a_chime":
				gameObject.transform.localScale = new Vector3(maxSize - (8 * (maxSize - minSize) / 12), maxSize - (8 * (maxSize - minSize) / 12), maxSize - (8 * (maxSize - minSize) / 12));
                note = "g_sharp_chime";
				break;
			case "a_sharp_chime":
				gameObject.transform.localScale = new Vector3(maxSize - (9 * (maxSize - minSize) / 12), maxSize - (9 * (maxSize - minSize) / 12), maxSize - (9 * (maxSize - minSize) / 12));
                note = "a_chime";
				break;
			case "b_chime":
				gameObject.transform.localScale = new Vector3(maxSize - (10 * (maxSize - minSize) / 12), maxSize - (10 * (maxSize - minSize) / 12), maxSize - (10 * (maxSize - minSize) / 12));
                note = "a_sharp_chime";
				break;
			case "c_chime_high":
				gameObject.transform.localScale = new Vector3(maxSize - (11 * (maxSize - minSize) / 12), maxSize - (11 * (maxSize - minSize) / 12), maxSize - (11 * (maxSize - minSize) / 12));
                note = "b_chime";
				break;
			default:
				note = "c_chime_low";
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