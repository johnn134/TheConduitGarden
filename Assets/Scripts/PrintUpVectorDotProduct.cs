using UnityEngine;
using System.Collections;

public class PrintUpVectorDotProduct : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time % 0.5f < 0.01)
			Debug.Log(gameObject.name + " dot product: " + Vector3.Dot(Vector3.up, transform.up));
	}
}
