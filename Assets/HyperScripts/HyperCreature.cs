using UnityEngine;
using System.Collections;

public class HyperCreature : MonoBehaviour {

	public int w = 0;                       //point on w axis
	public int w_perif = 0;                 //the perifial view of the w axis

    public FourthDManager IVDManager;       //the 4D manager

    void Start()
    {
        //locate the 4Dmanager
        IVDManager = Object.FindObjectOfType<FourthDManager>();
    }

	//public function for other objects to call to tell the creature to move along the w axis
	public void WMove(int deltaW){
		if((deltaW > 0 && w != 6) || (deltaW < 0 && w != 0)){
			w += deltaW;
		}
	}
}
