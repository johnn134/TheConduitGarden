using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class FreezeRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.identity;
	}
}
