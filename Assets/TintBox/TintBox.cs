using UnityEngine;
using System.Collections;
using Valve.VR;

public class TintBox : MonoBehaviour {

	SteamVR_ControllerManager controllerManager;    //The steam controller manager that holds the controller indices

	const float TINT_ALPHA = 0.1f;

	// Use this for initialization
	void Awake(){
		controllerManager = SteamVR_ControllerManager.instance;
	}

	void Start() {
		updateBoxVisuals();
	}

	void LateUpdate()
	{
		for (int i = 0; i < controllerManager.indices.Length; i++)
		{
			if (controllerManager.indices[i] != OpenVR.k_unTrackedDeviceIndexInvalid)
			{
				if (SteamVR_Controller.Input((int)controllerManager.indices[i]).GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger))
				{
					updateBoxVisuals();
				}
			}
		}
	}

	/*
	 * change the tint color on all box planes
	 */
	void updateBoxVisuals() {
		foreach(Transform child in transform) {
			updateChildVisual(child);
		}
	}

	/*
	 * Update the plane color to the player's w position
	 */
	public void updateChildVisual(Transform obj) {
		Color temp = Color.white;

		switch(HyperCreature.instance.w) {
			case 0:
				temp = Color.red;
				break;
			case 1:
				temp = new Color(1.0f, 0.45f, 0.0f);
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

		//Set alpha
		temp.a = TINT_ALPHA;

		//Update material color
		obj.GetComponent<Renderer>().material.SetColor("_TintColor", temp);
	}
}
