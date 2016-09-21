using UnityEngine;
using System.Collections;

public class HyperCreature : MonoBehaviour {

	public int w = 0;                       //point on w axis
	public int w_perif = 0;                 //the perifial view of the w axis

    public static HyperCreature instance = null;

    void Awake()
    {
        //Application.targetFrameRate = 60;

        //declare as singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //add this later for persistant creature
        //DontDestroyOnLoad(gameObject);
    }

    public bool FadeToBlack()
    {
        if(Camera.main.farClipPlane > 0)
        {
            if (Camera.main.farClipPlane > 5.0F)
                Camera.main.farClipPlane -= .1f;
            else
                Camera.main.farClipPlane -= .005f;
            return false;
        }
        return true;

    }

    public void WMoveAllHyperObjects()
    {
        var allHyper = Object.FindObjectsOfType<HyperObject>();

        foreach (HyperObject hyperObj in allHyper)
            hyperObj.WMove();
    }

	//public function for other objects to call to tell the creature to move along the w axis
	public void WMove(int deltaW){
		if((deltaW > 0 && w != 6) || (deltaW < 0 && w != 0)){
			w += deltaW;
		}
	}
}
