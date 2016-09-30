using UnityEngine;
using System.Collections;
using Valve.VR;

public class Insecticide : MonoBehaviour {

	SteamVR_ControllerManager controllerManager;    //The steam controller manager that holds the controller indices

	// Use this for initialization
	void Awake(){
		controllerManager = SteamVR_ControllerManager.instance;
	}

	// Use this for initialization
	void Start () {
		transform.GetChild(2).GetComponent<ParticleSystem>().Stop();
		transform.GetChild(1).GetComponent<CapsuleCollider>().enabled = false;

		updateSprayVisual();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1)) {
			startSpray();
		}
		if(Input.GetMouseButtonUp(1)) {
			endSpray();
		}
	}

	void LateUpdate()
	{
		for (int i = 0; i < controllerManager.indices.Length; i++)
		{
			if (controllerManager.indices[i] != OpenVR.k_unTrackedDeviceIndexInvalid)
			{
				if (SteamVR_Controller.Input((int)controllerManager.indices[i]).GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger))
				{
					updateSprayVisual();
				}
			}
		}
	}

	/*
	 * Update the plane color to the player's w position
	 */
	public void updateSprayVisual() {
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
		transform.GetChild(2).GetComponent<ParticleSystem>().startColor = temp;
	}

	public void startSpray() {
		//Begin Particle Effect
		transform.GetChild(2).GetComponent<ParticleSystem>().Play();

		//Turn on collisions
		transform.GetChild(1).GetComponent<CapsuleCollider>().enabled = true;

		CancelInvoke();
		Invoke("endSpray", .2f);
	}

	public void endSpray() {
		//End Particle Effect
		transform.GetChild(2).GetComponent<ParticleSystem>().Stop();

		//Turn off collisions
		transform.GetChild(1).GetComponent<CapsuleCollider>().enabled = false;
	}
}
