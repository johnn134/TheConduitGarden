using UnityEngine;
using System.Collections;

public class HouseManager : MonoBehaviour {

	public GameObject[] houses;

	// Use this for initialization
	void Start () {
		fillHouses(PlayerPrefs.GetInt("KamiHighscore"));
		//fillHouses(29);
	}

	void fillHouses(int numKami) {
		for(int i = 0; i < numKami; i++) {
			KamiManager.instance.MakeKami(houses[i].transform.position + new Vector3( -0.05f, 0.1f, 0.0f), Quaternion.identity, Random.Range(0, 7), 0);
		}

		KamiManager.instance.MakeKamiSad();
	}
}
