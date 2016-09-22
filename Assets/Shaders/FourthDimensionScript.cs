using UnityEngine;
using System.Collections;

//[ExecuteInEditMode]
public class FourthDimensionScript : MonoBehaviour {

	public Texture2D texture;

	public float dimValue = 1.0f;

	public int w = 0;

	Material mat;

	int creatureW = 0;


	void Awake() {
		mat = GetComponent<Renderer>().material;
	}

	void Start() {
		updateVisuals(creatureW);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.UpArrow)) {
			updateVisuals(creatureW + 1);
		}
		if(Input.GetKeyDown(KeyCode.DownArrow)) {
			updateVisuals(creatureW - 1);
		}
	}

	//Set the w position and shader values of this object
	void updateVisuals(int newPos) {
		if(newPos >= 0 && newPos <= 6) {
			if(newPos == w)
				setOpaqueShader();
			else
				setTransparentShader();

			creatureW = newPos;
		}
	}

	void setOpaqueShader() {
		mat.shader = Shader.Find("FourthDimension/FourthDimensionOpaqueShader");
		mat.SetTexture("_MainTex", texture);
		updateColor(1.0f);
	}

	void setTransparentShader() {
		mat.shader = Shader.Find("FourthDimension/FourthDimensionTransparentShader");
		mat.SetTexture("_MainTex", texture);
		updateColor(0.25f);
	}

	//Set the mat color to the w position
	void updateColor(float alphaValue) {
		Color temp = Color.black;

		switch(w) {
			case 0:
				temp = Color.red;
				break;
			case 1:
				temp = new Color(1.0f, 0.5f, 0.0f);
				break;
			case 2:
				temp = Color.yellow;
				break;
			case 3:
				temp = Color.green;
				break;
			case 4:
				temp = Color.cyan;
				break;
			case 5:
				temp = Color.blue;
				break;
			case 6:
				temp = Color.magenta;
				break;
		}

		temp.r /= dimValue;
		temp.g /= dimValue;
		temp.b /= dimValue;
		temp.a = alphaValue;

		mat.SetColor("_Color", temp);
	}
}
