using UnityEngine;
using System.Collections;

public class BonsaiBug : MonoBehaviour {
	
	float yOrigin;
	float cycleStart;
	float deathStartTime;
	float xOrigin;
	float zOrigin;

	float movementRange = 0.1f;

	bool movingUp;
	bool isDying;

	const float BUG_SPEED = 1.0f;
	const float DEATH_PHASE_TIME = 3.0f;

	static int ID = 0;

	// Use this for initialization
	void Start () {
		//Initialize Variables
		movingUp = Random.Range(0, 2) == 0 ? true : false;
		isDying = false;
		cycleStart = Time.time - Random.Range(0.0f, 1.0f);

		GetComponent<BoxCollider>().enabled = false;

		//Name the bug
		this.gameObject.name = "BonsaiBug_" + ID;
		ID++;
	}
	
	// Update is called once per frame
	void Update () {
		if(!isDying) {
			float start = movingUp ? yOrigin - movementRange : yOrigin + movementRange;
			float end = movingUp ? yOrigin + movementRange : yOrigin - movementRange;

			transform.localPosition = new Vector3(xOrigin, Mathf.Lerp(start, end, (Time.time - cycleStart) * BUG_SPEED), zOrigin);
			transform.localRotation = Quaternion.identity;

			if(Mathf.Abs(end - transform.localPosition.y) <= 0.01f) {
				movingUp = !movingUp;
				cycleStart = Time.time;
			}
		}
		else {
			if(Time.time >= deathStartTime + DEATH_PHASE_TIME) {
				Destroy(this.gameObject);
			}
		}
	}

	/*
	 * resets the yOrigin to this object's current position
	 */
	public void updateYOrigin() {
		yOrigin = transform.localPosition.y;
		xOrigin = transform.localPosition.x;
		zOrigin = transform.localPosition.z;
	}

	/*
	 * sets the yOrigin
	 */
	public void setOrigin(float newX, float newY, float newZ) {
		xOrigin = newX;
		yOrigin = newY;
		zOrigin = newZ;
	}

	/*
	 * sets movement range
	 */
	public void setMovementRange(float newRange) {
		movementRange = newRange;
	}

	/*
	 * Begins the bug's death phase
	 */
	public void startDeath() {
		GetComponent<BoxCollider>().enabled = true;
		GetComponent<Rigidbody>().useGravity = true;
		deathStartTime = Time.time;
		isDying = true;
	}
}
