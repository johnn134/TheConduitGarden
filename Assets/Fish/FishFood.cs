using UnityEngine;
using System.Collections;

public class FishFood : MonoBehaviour {

	public int lifeTime = 10;

    FishManager fishManager;

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
        if(isIn)
        {
            _cachedRigidbody.drag = 70;
            fishManager.alertFood(myHyper.w, true);
        }
        else
        {
            _cachedRigidbody.drag = 0;
        }
    }

    void LifeEnd()
    {
        fishManager.RequestToRemove(gameObject, false);
    }
}
