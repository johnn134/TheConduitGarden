using UnityEngine;
using System.Collections;

public class GravelPit : MonoBehaviour {

	public GameObject tine1;
	public Vector3 tine1Position;
	public GameObject tine2;
	public Vector3 tine2Position;
	public GameObject tine3;
	public Vector3 tine3Position;
	public GameObject tine4;
	public Vector3 tine4Position;
	public Texture2D texture;
	public Color color;
	public int[,] coordinates;
	public int raked;
	public int similarity;
	public Texture2D contract;
	public Texture2D current;
	private Texture2D container;
	public Texture2D originalPit;
	public Texture2D currentPit;
	private int x;
	private int z;
	private int originalThird;
	public bool isRakedEnough; 

	void Start() {
		tine1 = GameObject.FindGameObjectWithTag("Tine 1");
		tine2 = GameObject.FindGameObjectWithTag("Tine 2");
		tine3 = GameObject.FindGameObjectWithTag("Tine 3");
		tine4 = GameObject.FindGameObjectWithTag("Tine 4");

		GetComponent<Renderer>().material.mainTexture = texture;

		raked = 0;

		similarity = 0;

		coordinates = new int[1024, 1024];

		container = new Texture2D(contract.width, contract.height);

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

		color.r = 0.2f;
		color.g = 0.2f;
		color.b = 0.2f;

		if ((float)tine1.GetComponent<Transform>().position.y <= (float)0.3) {
			for (x = ((int)tine1Position.x - 5); x <= ((int)tine1Position.x + 5); x++) {
				for (z = ((int)tine1Position.z - 5); z <= ((int)tine1Position.z + 5); z++) {
					if (coordinates[x, z] == 1) {
						break;
					}

					else {
						texture.SetPixel(x, z, color);
						coordinates[x, z] = 1;
					}
				}
			}

			for (x = ((int)tine2Position.x - 5); x <= ((int)tine2Position.x + 5); x++) {
				for (z = ((int)tine2Position.z - 5); z <= ((int)tine2Position.z + 5); z++) {
					if (coordinates[x, z] == 1) {
						break;
					}

					else {
						texture.SetPixel(x, z, color);
						coordinates[x, z] = 1;
					}
				}
			}

			for (x = ((int)tine3Position.x - 5); x <= ((int)tine3Position.x + 5); x++) {
				for (z = ((int)tine3Position.z - 5); z <= ((int)tine3Position.z + 5); z++) {
					if (coordinates[x, z] == 1) {
						break;
					}

					else {
						texture.SetPixel(x, z, color);
						coordinates[x, z] = 1;
					}
				}
			}

			for (x = ((int)tine4Position.x - 5); x <= ((int)tine4Position.x + 5); x++) {
				for (z = ((int)tine4Position.z - 5); z <= ((int)tine4Position.z + 5); z++) {
					if (coordinates[x, z] == 1) {
						break;
					}

					else {
						texture.SetPixel(x, z, color);
						coordinates[x, z] = 1;
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

		if (Input.GetKeyDown(KeyCode.I)) {
			float percentage = CompareImages();
			Debug.Log(percentage);
		}

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

	float CompareImages() {
		for (int i = 0; i < contract.width; i++) {
			for (int j = 0; j < contract.height; j++) {
				Color currentColor = current.GetPixel(i, j);
				Color contractColor = contract.GetPixel(i, j);

				if (contractColor != currentColor) {
					similarity++;
					container.SetPixel(i, j, Color.red);
				}

				else {
					container.SetPixel(i, j, contract.GetPixel(i, j));
				}
			}
		}

		int totalPixels = contract.width * contract.height;
		float difference = (float)((float)similarity / (float)totalPixels);
		float percentage = difference * 100;

		return percentage;
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