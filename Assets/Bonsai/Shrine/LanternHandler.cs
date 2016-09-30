using UnityEngine;
using System.Collections;
using Valve.VR;

public class LanternHandler : MonoBehaviour {

	SteamVR_ControllerManager controllerManager;    //The steam controller manager that holds the controller indices

	// Use this for initialization
	void Awake(){
		controllerManager = SteamVR_ControllerManager.instance;
	}

	// Use this for initialization
	void Start () {
		updateParticleColors();
	}

	void LateUpdate()
	{
		for (int i = 0; i < controllerManager.indices.Length; i++)
		{
			if (controllerManager.indices[i] != OpenVR.k_unTrackedDeviceIndexInvalid)
			{
				if (SteamVR_Controller.Input((int)controllerManager.indices[i]).GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger))
				{
					updateParticleColors();
				}
			}
		}
	}

	/*
	 * Update the plane color to the player's w position
	 */
	public void updateParticleColors() {
		Color temp = Color.white;

		switch(HyperCreature.instance.w) {
			case 0:
				temp = Color.red;
				break;
			case 1:
				temp = Color.yellow;
				break;
			case 2:
				temp = Color.green;
				break;
			case 3:
				temp = Color.cyan;
				break;
			case 4:
				temp = Color.magenta;
				break;
		}

		//Update material color
		transform.GetChild(1).GetComponent<ParticleSystem>().startColor = temp;
		transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>().startColor = temp;
		transform.GetChild(2).GetComponent<ParticleSystem>().startColor = temp;
		transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>().startColor = temp;
	}
}
