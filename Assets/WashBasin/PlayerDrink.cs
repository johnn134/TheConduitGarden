using UnityEngine;
using System.Collections;

public class PlayerDrink : MonoBehaviour {

    HyperCreature hyperC;

    int player_periph;

	float drinkStartTime;

	bool isEffected;

	const float VISION_DURATION = 5.0f;

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
			if(Time.time > drinkStartTime + VISION_DURATION) {
                Debug.Log("Ending Drink");
				removeDrinkEffect();
				isEffected = false;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if(other.name == "WaterZone") {
			other.GetComponent<Ladle>().touchingPlayer(this.gameObject);
		}
	}

	void OnTriggerExit(Collider other) {
		if(other.name == "WaterZone") {
			other.GetComponent<Ladle>().leavingPlayer();
		}
	}

	public void registerDrink() {
        Debug.Log("Starting Drink");
		addDrinkEffect();
		drinkStartTime = Time.time;
		isEffected = true;
	}

	void addDrinkEffect() {
		player_periph = hyperC.w_perif;
        hyperC.w_perif = 3;
	}

	void removeDrinkEffect() {
        hyperC.w_perif = player_periph;
	}
}
