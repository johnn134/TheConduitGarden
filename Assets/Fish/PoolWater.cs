using UnityEngine;
using System.Collections;

public class PoolWater : MonoBehaviour {

    public float gap;                               //how far from the edge of the pool the area where the fish can get wander points is
    public float happySpeed = .5f;                  //the speed of the fish in this pool when happy
    public float hungrySpeed = .1f;                 //the speed of the fish in this pool when hungry
    public float huntingSpeed = 1f;                 //the speed of the fish in this pool when hunting

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.StartsWith("Fish"))
        {
            Fish enteringFish = other.gameObject.GetComponent<Fish>();

            enteringFish.wanderArea1 = new Vector3(transform.parent.transform.position.x - (transform.lossyScale.z / 2) + gap, 
                                                                            transform.parent.transform.position.y + gap, 
                                                                            transform.parent.transform.position.z - (transform.lossyScale.x / 2) + gap);
            enteringFish.wanderArea2 = new Vector3(transform.parent.transform.position.x + (transform.lossyScale.z / 2) - gap,
                                                                            transform.parent.transform.position.y + (transform.lossyScale.y) - gap,
                                                                            transform.parent.transform.position.z + (transform.lossyScale.x / 2) - gap);

            //set the speed for the entering fish based on this pool's speed values
            enteringFish.happySpeed = happySpeed;
            enteringFish.hungrySpeed = hungrySpeed;
            enteringFish.huntingSpeed = huntingSpeed;

            enteringFish.InWater(true);
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
