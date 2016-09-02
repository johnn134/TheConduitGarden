using UnityEngine;
using System.Collections;

public class PoolWater : MonoBehaviour {

    public float gap;

    /*void Update()
    {
        Debug.DrawLine(new Vector3(transform.parent.transform.position.x - (transform.lossyScale.z / 2) + gap,
                                                                            transform.parent.transform.position.y + gap,
                                                                            transform.parent.transform.position.z - (transform.lossyScale.x / 2) + gap), 
                                                                new Vector3(transform.parent.transform.position.x + (transform.lossyScale.z / 2) - gap,
                                                                            transform.parent.transform.position.y + (transform.lossyScale.y) - gap,
                                                                            transform.parent.transform.position.z + (transform.lossyScale.x / 2) - gap));
    }*/

	void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Fish"))
        {
            other.gameObject.GetComponent<Fish>().wanderArea1 = new Vector3(transform.parent.transform.position.x - (transform.lossyScale.z / 2) + gap, 
                                                                            transform.parent.transform.position.y + gap, 
                                                                            transform.parent.transform.position.z - (transform.lossyScale.x / 2) + gap);
            other.gameObject.GetComponent<Fish>().wanderArea2 = new Vector3(transform.parent.transform.position.x + (transform.lossyScale.z / 2) - gap,
                                                                            transform.parent.transform.position.y + (transform.lossyScale.y) - gap,
                                                                            transform.parent.transform.position.z + (transform.lossyScale.x / 2) - gap);
            other.gameObject.GetComponent<Fish>().InWater(true);
        }
        else if (other.gameObject.name.StartsWith("Food"))
            other.gameObject.GetComponent<FishFood>().InWater(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.StartsWith("Fish"))
        {
            other.gameObject.GetComponent<Fish>().InWater(false);
        }
        else if (other.gameObject.name.StartsWith("Food"))
            other.gameObject.GetComponent<FishFood>().InWater(false);
    }
}
