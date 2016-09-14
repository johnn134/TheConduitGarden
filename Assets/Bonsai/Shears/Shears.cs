using UnityEngine;
using System.Collections;

public class Shears : MonoBehaviour {

	bool startedSnip;
    int counter = 0;

	// Use this for initialization
	void Start () {
		startedSnip = false;

		transform.GetChild(1).GetComponent<BoxCollider>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		//Check for turning off collisions for snipping
		if(startedSnip) {
            counter++;
            if (counter >= 2)
            {
                transform.GetChild(1).GetComponent<BoxCollider>().enabled = false;
                startedSnip = false;
            }
		}

		//Check for input
		if(Input.GetMouseButtonDown(0)) {
			snip();
		}
	}

	public void snip() {
        Debug.Log("snip");
		transform.GetChild(1).GetComponent<BoxCollider>().enabled = true;
        Debug.Log("Shearzone is " + transform.GetChild(1).GetComponent<BoxCollider>().enabled);
        counter = 0;
        startedSnip = true;
	}
}
