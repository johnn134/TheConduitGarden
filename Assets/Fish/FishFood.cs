using UnityEngine;
using System.Collections;

public class FishFood : MonoBehaviour {

    GameObject fishManager;
    bool inWater = false;
    public int lifeTime = 10;

	void Start () {
        //locate the fish manager
        fishManager = GameObject.Find("FishManager");

        Invoke("LifeEnd", lifeTime);

        //send a request to the manager to be added to the world, if request is denied then destroy this food
        /*if (fishManager.GetComponent<FishManager>().RequestToAdd(this.gameObject))
        {
            // inWorld = true;
            //Destroy(this.gameObject, lifeTime);
            Invoke("LifeEnd", lifeTime);
        }
        else
            Destroy(this.gameObject);*/
    }

    //function called by water to let the fish know if it is in the water or not
    public void InWater(bool isIn)
    {
        if (isIn)
        {
            GetComponent<Rigidbody>().drag = 70;
            fishManager.GetComponent<FishManager>().alertFood(GetComponent<HyperObject>().w, true);
            inWater = true;
        }
        else
        {
            GetComponent<Rigidbody>().drag = 0;
            inWater = false;
        }
    }

    void LifeEnd()
    {
        fishManager.GetComponent<FishManager>().RequestToRemove(this.gameObject, false);
    }
}
