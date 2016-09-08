using UnityEngine;
using System.Collections;

public class Rake : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		Vector3 movement = Vector3.zero;

		if (Input.GetKey(KeyCode.W)) {
			movement.z += 1f;
		}

		if (Input.GetKey(KeyCode.A)) {
			movement.x -= 1f;
		}

		if (Input.GetKey(KeyCode.S)) {
			movement.z -= 1f;
		}

		if (Input.GetKey(KeyCode.D)) {
			movement.x += 1f;
		}

		transform.Translate(movement * Time.deltaTime);
	}
}