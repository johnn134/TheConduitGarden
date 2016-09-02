﻿using UnityEngine;
using System.Collections;

public class HyperCreature : MonoBehaviour {

	public int w = 0;                       //point on w axis
	public int w_perif = 0;                 //the perifial view of the w axis

    public FourthDManager IVDManager;       //the 4D manager

    void Start()
    {
        //locate the 4Dmanager
        IVDManager = Object.FindObjectOfType<FourthDManager>();

        //WMove();//remove once 4D shaders are implemented
    }

	//Move along the w axis, remove once 4D shader is implemented
	public void WMove()
    {
		HyperObject[] hyperObjects; //all hyper objects in the world

        //hyperObjects = GameObject.FindGameObjectsWithTag("HyperObject");
        hyperObjects = Object.FindObjectsOfType<HyperObject>();

		//tell each hyper object that the player is moving along the w axis
		foreach(var hypObj in hyperObjects)
        {
            if(hypObj)
			    hypObj.WMove(w);
		}
    }

	//public function for other objects to call to tell the creature to move along the w axis
	public void WMove(int deltaW){
		if((deltaW > 0 && w != 6) || (deltaW < 0 && w != 0)){
			w += deltaW;

			WMove ();
		}
	}
}
