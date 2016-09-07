using UnityEngine;
using System.Collections;

public class FoodContainer : MonoBehaviour {

    FishManager fishManager;
    public GameObject opening;
    public int pourRate = 10;
    int curRate = 0;

    void Start()
    {
        //find the opening to the container
        opening = GameObject.Find("ToolFoodContainer/Opening");

        fishManager = Object.FindObjectOfType<FishManager>();
    }

    void Update()
    {
        //check if the container is rotated to be upside down to start spawning food
        if (transform.parent)
        {
            if(Vector3.Dot(transform.up, Vector3.up) < -.2f)
            {
                if (curRate == pourRate)
                {
                    curRate = 0;
                    fishManager.MakeFish(opening.transform.position, opening.transform.rotation, GetComponent<HyperColliderManager>().w, true);
                }
                else
                    curRate++;
            }
        }
    }
}
