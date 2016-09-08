using UnityEngine;
using System.Collections;

public class Alpha : MonoBehaviour {
	public GameObject rake;
	public GameObject tine1;
	public Vector3 tine1Position;
	public bool tine1Collision;
	public GameObject tine2;
	public Vector3 tine2Position;
	public bool tine2Collision;
	public GameObject tine3;
	public Vector3 tine3Position;
	public bool tine3Collision;
	public GameObject tine4;
	public Vector3 tine4Position;
	public bool tine4Collision;
	public Texture2D texture;
	public Color color;
	public int[,] coordinates;
	public int raked;
	public Texture2D originalPit;
	public Texture2D currentPit;
	private int x;
	private int z;
	private int originalThird;
	public bool isRakedEnough; 

	void Start() {
		rake = GameObject.FindGameObjectWithTag("Rake");
		tine1 = GameObject.FindGameObjectWithTag("Tine 1");
		tine2 = GameObject.FindGameObjectWithTag("Tine 2");
		tine3 = GameObject.FindGameObjectWithTag("Tine 3");
		tine4 = GameObject.FindGameObjectWithTag("Tine 4");

		GetComponent<Renderer>().material.mainTexture = texture;

		raked = 0;

		color.r = 0.2f;
		color.g = 0.2f;
		color.b = 0.2f;
		color.a = 1.0f;

		coordinates = new int[1024, 1024];

		originalThird = (int)Mathf.Ceil(originalPit.width / 3);

		isRakedEnough = false;

		Debug.Log (texture.GetPixel(512,512));
	}

	void Update() {


		tine1Position.x = Mathf.Ceil((tine1.GetComponent<Transform>().position.x + 5) * (float)102.4);
		tine1Position.z = Mathf.Ceil((tine1.GetComponent<Transform>().position.z + 5) * (float)102.4);

		tine2Position.x = Mathf.Ceil((tine2.GetComponent<Transform>().position.x + 5) * (float)102.4);
		tine2Position.z = Mathf.Ceil((tine2.GetComponent<Transform>().position.z + 5) * (float)102.4);

		tine3Position.x = Mathf.Ceil((tine3.GetComponent<Transform>().position.x + 5) * (float)102.4);
		tine3Position.z = Mathf.Ceil((tine3.GetComponent<Transform>().position.z + 5) * (float)102.4);

		tine4Position.x = Mathf.Ceil((tine4.GetComponent<Transform>().position.x + 5) * (float)102.4);
		tine4Position.z = Mathf.Ceil((tine4.GetComponent<Transform>().position.z + 5) * (float)102.4);

		float rotation = (float)rake.GetComponent<Transform>().rotation.z;

		if (Vector3.Dot(Vector3.up, transform.up) > 0) {
			if (tine1Collision == true) {
				for (x = ((int)tine1Position.x - 5); x <= ((int)tine1Position.x + 5); x++) {
					for (z = ((int)tine1Position.z - 5); z <= ((int)tine1Position.z + 5); z++) {
						if (coordinates [x, z] == 1) {
							break;
						} 

						else {
							texture.SetPixel (x, z, color);
							coordinates [x, z] = 1;
						}
					}
				}
			}

			if (tine2Collision == true) {
				for (x = ((int)tine2Position.x - 5); x <= ((int)tine2Position.x + 5); x++) {
					for (z = ((int)tine2Position.z - 5); z <= ((int)tine2Position.z + 5); z++) {
						if (coordinates [x, z] == 1) {
							break;
						} 

						else {
							texture.SetPixel (x, z, color);
							coordinates [x, z] = 1;
						}
					}
				}
			}

			if (tine3Collision == true) {
				for (x = ((int)tine3Position.x - 5); x <= ((int)tine3Position.x + 5); x++) {
					for (z = ((int)tine3Position.z - 5); z <= ((int)tine3Position.z + 5); z++) {
						if (coordinates [x, z] == 1) {
							break;
						} 

						else {
							texture.SetPixel (x, z, color);
							coordinates [x, z] = 1;
						}
					}
				}
			}

			if (tine4Collision == true) {
				for (x = ((int)tine4Position.x - 5); x <= ((int)tine4Position.x + 5); x++) {
					for (z = ((int)tine4Position.z - 5); z <= ((int)tine4Position.z + 5); z++) {
						if (coordinates [x, z] == 1) {
							break;
						} 

						else {
							texture.SetPixel (x, z, color);
							coordinates [x, z] = 1;
						}
					}
				}
			}
		}

		foreach (int coordinate in coordinates) {
			if (coordinate == 1) {
				raked++;
			}
		}

		if (raked == (originalThird * originalPit.height)) {
			isRakedEnough = true;
			Debug.Log("Raked enough.");
		}

		raked = 0;

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			clearPitSection1();
		}

		if (Input.GetKeyDown(KeyCode.Alpha2)) {
			clearPitSection2();
		}

		if (Input.GetKeyDown(KeyCode.Alpha3)) {
			clearPitSection3();
		}

		texture.Apply();
	}

	void OnTriggerEnter(Collider col) {
		if (col.gameObject.name == "Tine 1") {
			tine1Collision = true;
		}

		if (col.gameObject.name == "Tine 2") {
			tine2Collision = true;
		}

		if (col.gameObject.name == "Tine 3") {
			tine3Collision = true;
		}

		if (col.gameObject.name == "Tine 4") {
			tine4Collision = true;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.transform.name == "Tine 1") {
			tine1Collision = false;
		}

		if (col.transform.name == "Tine 2") {
			tine2Collision = false;
		}

		if (col.transform.name == "Tine 3") {
			tine3Collision = false;
		}

		if (col.transform.name == "Tine 4") {
			tine4Collision = false;
		}
	}

	void clearPitSection1() {
		for (int i = 0; i <= originalThird; i++) {
			for (int j = 0; j <= originalPit.height; j++) {
				Color originalColor = originalPit.GetPixel(i, j);
				currentPit.SetPixel(i, j, originalColor);
			}
		}
	}

	void clearPitSection2() {
		for (int i = originalThird; i <= (originalThird * 2); i++) {
			for (int j = 0; j <= originalPit.height; j++) {
				Color originalColor = originalPit.GetPixel(i, j);
				currentPit.SetPixel(i, j, originalColor);
			}
		}
	}

	void clearPitSection3() {
		for (int i = (originalThird * 2); i <= originalPit.width; i++) {
			for (int j = 0; j <= originalPit.height; j++) {
				Color originalColor = originalPit.GetPixel(i, j);
				currentPit.SetPixel(i, j, originalColor);
			}
		}
	}
}
