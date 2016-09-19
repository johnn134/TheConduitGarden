using UnityEngine;
using System.Collections;

public class FishFood : MonoBehaviour {

    FishManager fishManager;
    bool inWater = false;
    public int lifeTime = 10;

    HyperObject myHyper;

    Rigidbody _cachedRigidbody;

	void Start () {
        //locate the fish manager
        fishManager = FishManager.instance;

        myHyper = GetComponent<HyperObject>();

        _cachedRigidbody = GetComponent<Rigidbody>();

        Invoke("LifeEnd", lifeTime);
    }

    //function called by water to let the fish know if it is in the water or not
    public void InWater(bool isIn)
    {
        if (isIn)
        {
            _cachedRigidbody.drag = 70;
            fishManager.alertFood(myHyper.w, true);
            inWater = true;
        }
        else
        {
            _cachedRigidbody.drag = 0;
            inWater = false;
        }
    }

    void LifeEnd()
    {
        fishManager.RequestToRemove(gameObject, false);
    }
}
