using UnityEngine;
using System.Collections;

public class GravelPit : MonoBehaviour {

	public Texture2D texture;

	void Start() {
		GetComponent<Renderer>().material.mainTexture = texture;
	}		
}