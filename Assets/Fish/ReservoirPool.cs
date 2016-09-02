using UnityEngine;
using System.Collections;

public class ReservoirPool : MonoBehaviour {

    FishManager fishManager;
    public int spawnTime = 15;

	void Start () {
        //find the fish manager
        fishManager = Object.FindObjectOfType<FishManager>();

        Invoke("SpawnFish", spawnTime);
        //SpawnFish();
    }

    /*void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.StartsWith("Fish"))
            Invoke("SpawnFish", 15);
    }*/

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name.StartsWith("Fish"))
        {
            CancelInvoke();
            Invoke("SpawnFish", spawnTime);
        }
    }

    void SpawnFish()
    {
        fishManager.MakeFish(transform.position, transform.rotation, GetComponent<HyperColliderManager>().w, false);
    }
}
