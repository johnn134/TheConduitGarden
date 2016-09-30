using UnityEngine;
using System.Collections;

public class Ladle : MonoBehaviour {

	GameObject player;

	bool isFull;
	bool nearPlayer;

	void Awake() {
		isFull = false;
		nearPlayer = false;
	}

	// Use this for initialization
	void Start () {
		emptyLadle();
	}

	// Update is called once per frame
	void Update () {
		if(isFull) {
			if(Vector3.Dot(transform.parent.up, Vector3.up) < 0.0f) {
				if(nearPlayer) {
					drinkLadle();
				}
				else {
					emptyLadle();
				}
			}
		}
	}

	/*
	 * Fill the ladle with basin water
	 */
	public void fillLadle() {
		if(!isFull) {
			transform.parent.GetChild(0).GetChild(1).GetComponent<Renderer>().enabled = true;
			isFull = true;
		}
	}

	/*
	 * Remove the water from the ladle cup
	 */
	void emptyLadle() {
		transform.parent.GetChild(0).GetChild(1).GetComponent<Renderer>().enabled = false;
		isFull = false;
	}

	/*
	 * Remove the water from the ladle cup
	 * and apply the drinking effect to the player
	 */
	public void drinkLadle() {
		if(player != null) {
			if(player.GetComponent<PlayerDrink>() != null)
				player.GetComponent<PlayerDrink>().registerDrink();
		}

		transform.parent.GetChild(0).GetChild(1).GetComponent<Renderer>().enabled = false;
		isFull = false;
	}

	/*
	 * Registers that the ladle is within range of the player's face
	 */
	public void touchingPlayer(GameObject other) {
		player = other;
		nearPlayer = true;
	}

	/*
	 * Registers that the ladle has left the range of the player's face
	 */
	public void leavingPlayer() {
		nearPlayer = false;
	}
}
