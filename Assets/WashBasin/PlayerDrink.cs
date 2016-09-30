using UnityEngine;
using System.Collections;

public class PlayerDrink : MonoBehaviour {

	public float visionDuration = 5.0f;

    HyperCreature hyperC;

    int player_periph;

	float drinkStartTime;

	bool isEffected;

	void Awake() {
		isEffected = false;
        hyperC = Object.FindObjectOfType<HyperCreature>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isEffected) {
			if(Time.time > drinkStartTime + visionDuration) {	//End effect after duration
				removeDrinkEffect();
				isEffected = false;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.name == "WaterZone") {	//inform ladle it is within range of the player's face
			other.GetComponent<Ladle>().touchingPlayer(this.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.name == "WaterZone") {	//inform ladle it has left face range
			other.GetComponent<Ladle>().leavingPlayer();
		}
	}

	/*
	 * Begin drink effect
	 */
	public void registerDrink() {
		addDrinkEffect();
		drinkStartTime = Time.time;
		isEffected = true;
	}

	/*
	 * Increase player peripheral vision
	 */
	void addDrinkEffect() {
		player_periph = hyperC.w_perif;
        hyperC.w_perif = 3;
		hyperC.WMoveAllHyperObjects ();
	}

	/*
	 * Revert player peripheral vision
	 */
	void removeDrinkEffect() {
        hyperC.w_perif = player_periph;
		hyperC.WMoveAllHyperObjects ();
	}
}
