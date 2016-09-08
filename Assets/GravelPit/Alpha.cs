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
	public Color originalColor;
	public int[,] coordinates;
	public int raked;
	public bool isRakedEnough;
	private int originalThird;

	// Use this for initialization
	void Start () {
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

		originalColor.r = 0.0f;
		originalColor.g = 0.0f;
		originalColor.b = 0.0f;
		originalColor.a = 0.0f;

		coordinates = new int[1024, 1024];

		originalThird = (int)Mathf.Ceil(texture.width / 3);

		tine1Collision = false;
		tine2Collision = false;
		tine3Collision = false;
		tine4Collision = false;

		isRakedEnough = false;
	}
	
	// Update is called once per frame
	void Update () {
		tine1Position.x = Mathf.Ceil((tine1.GetComponent<Transform>().position.x + 5) * 102.4f);
		tine1Position.z = Mathf.Ceil((tine1.GetComponent<Transform>().position.z + 5) * 102.4f);

		tine2Position.x = Mathf.Ceil((tine2.GetComponent<Transform>().position.x + 5) * 102.4f);
		tine2Position.z = Mathf.Ceil((tine2.GetComponent<Transform>().position.z + 5) * 102.4f);

		tine3Position.x = Mathf.Ceil((tine3.GetComponent<Transform>().position.x + 5) * 102.4f);
		tine3Position.z = Mathf.Ceil((tine3.GetComponent<Transform>().position.z + 5) * 102.4f);

		tine4Position.x = Mathf.Ceil((tine4.GetComponent<Transform>().position.x + 5) * 102.4f);
		tine4Position.z = Mathf.Ceil((tine4.GetComponent<Transform>().position.z + 5) * 102.4f);

		if (Vector3.Dot(Vector3.up, transform.up) > 0) {
			if (tine1Collision == true) {
				for (int x = ((int)tine1Position.x - 5); x <= ((int)tine1Position.x + 5); x++) {
					for (int z = ((int)tine1Position.z - 5); z <= ((int)tine1Position.z + 5); z++) {
						if (coordinates [x, z] == 1) {
							break;
						} 

						else {
							texture.SetPixel(x, z, color);
							coordinates [x, z] = 1;
						}
					}
				}
			}

			if (tine2Collision == true) {
				for (int x = ((int)tine2Position.x - 5); x <= ((int)tine2Position.x + 5); x++) {
					for (int z = ((int)tine2Position.z - 5); z <= ((int)tine2Position.z + 5); z++) {
						if (coordinates [x, z] == 1) {
							break;
						} 

						else {
							texture.SetPixel(x, z, color);
							coordinates [x, z] = 1;
						}
					}
				}
			}

			if (tine3Collision == true) {
				for (int x = ((int)tine3Position.x - 5); x <= ((int)tine3Position.x + 5); x++) {
					for (int z = ((int)tine3Position.z - 5); z <= ((int)tine3Position.z + 5); z++) {
						if (coordinates [x, z] == 1) {
							break;
						} 

						else {
							texture.SetPixel(x, z, color);
							coordinates [x, z] = 1;
						}
					}
				}
			}

			if (tine4Collision == true) {
				for (int x = ((int)tine4Position.x - 5); x <= ((int)tine4Position.x + 5); x++) {
					for (int z = ((int)tine4Position.z - 5); z <= ((int)tine4Position.z + 5); z++) {
						if (coordinates [x, z] == 1) {
							break;
						} 

						else {
							texture.SetPixel(x, z, color);
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

		if (raked == (originalThird * texture.height)) {
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

		if (Input.GetKeyDown (KeyCode.Alpha3)) {
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
		for (int i = 1; i <= originalThird; i++) {
			for (int j = 1; j <= texture.height; j++) {
				texture.SetPixel((i - 1), (j - 1), originalColor);
				coordinates [(i - 1), (j - 1)] = 0;
			}
		}
	}

	void clearPitSection2() {
		for (int i = (originalThird + 1); i <= (originalThird * 2); i++) {
			for (int j = 1; j <= texture.height; j++) {
				texture.SetPixel((i - 1), (j - 1), originalColor);
				coordinates [(i - 1), (j - 1)] = 0;
			}
		}
	}

	void clearPitSection3() {
		for (int i = ((originalThird * 2) + 1); i <= texture.width; i++) {
			for (int j = 1; j <= texture.height; j++) {
				texture.SetPixel((i - 1), (j - 1), originalColor);
				coordinates [(i - 1), (j - 1)] = 0;
			}
		}
	}
}
