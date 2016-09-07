using UnityEngine;
using System.Collections;

public class GravelPit : MonoBehaviour {

	public GameObject rake;
	public GameObject tine1;
	public Vector3 tine1Position;
	public bool tine1collision;
	public GameObject tine2;
	public Vector3 tine2Position;
	public bool tine2collision;
	public GameObject tine3;
	public Vector3 tine3Position;
	public bool tine3collision;
	public GameObject tine4;
	public Vector3 tine4Position;
	public bool tine4collision;
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

		coordinates = new int[1024, 1024];

		originalThird = (int)Mathf.Ceil(originalPit.width / 3);

		isRakedEnough = false;
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

		if (((rotation <= 50.0f) && (rotation >= 0.0f)) || ((rotation < 360.0f) && (rotation >= 310.0f))) {
			if (tine1collision == true) {
				for (x = ((int)tine1Position.x - 5); x <= ((int)tine1Position.x + 5); x++) {
					for (z = ((int)tine1Position.z - 5); z <= ((int)tine1Position.z + 5); z++) {
						if (coordinates [x, z] == 1) {
							break;
						} else {
							texture.SetPixel (x, z, color);
							coordinates [x, z] = 1;
						}
					}
				}
			}

			if (tine2collision == true) {
				for (x = ((int)tine2Position.x - 5); x <= ((int)tine2Position.x + 5); x++) {
					for (z = ((int)tine2Position.z - 5); z <= ((int)tine2Position.z + 5); z++) {
						if (coordinates [x, z] == 1) {
							break;
						} else {
							texture.SetPixel (x, z, color);
							coordinates [x, z] = 1;
						}
					}
				}
			}

			if (tine3collision == true) {
				for (x = ((int)tine3Position.x - 5); x <= ((int)tine3Position.x + 5); x++) {
					for (z = ((int)tine3Position.z - 5); z <= ((int)tine3Position.z + 5); z++) {
						if (coordinates [x, z] == 1) {
							break;
						} else {
							texture.SetPixel (x, z, color);
							coordinates [x, z] = 1;
						}
					}
				}
			}

			if (tine4collision == true) {
				for (x = ((int)tine4Position.x - 5); x <= ((int)tine4Position.x + 5); x++) {
					for (z = ((int)tine4Position.z - 5); z <= ((int)tine4Position.z + 5); z++) {
						if (coordinates [x, z] == 1) {
							break;
						} else {
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
			tine1collision = true;
		}

		if (col.gameObject.name == "Tine 2") {
			tine2collision = true;
		}

		if (col.gameObject.name == "Tine 3") {
			tine3collision = true;
		}

		if (col.gameObject.name == "Tine 4") {
			tine4collision = true;
		}
	}

	void OnTriggerExit(Collider col) {
		if (col.transform.name == "Tine 1") {
			tine1collision = false;
		}

		if (col.transform.name == "Tine 2") {
			tine2collision = false;
		}

		if (col.transform.name == "Tine 3") {
			tine3collision = false;
		}

		if (col.transform.name == "Tine 4") {
			tine4collision = false;
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