using UnityEngine;
using System.Collections;

public class CleanUpObjects : MonoBehaviour {

	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Fish"))
            GameObject.Find("FishManager").GetComponent<FishManager>().RequestToRemove(other.gameObject, true);
    }
}
