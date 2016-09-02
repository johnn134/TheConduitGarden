using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class KamiManager : MonoBehaviour {

    //list of all kami in the world
    List<GameObject> allKami;

    //max number of kami allowed in the world
    public int maxKami = 30;

    //number of kami in the world
    public int numKami = 0;

    //current Kami index to set the id of new kami
    int curIndex = 0;

    public int kamiComeRate = 60;
    public int kamiLeaveRate = 60;

    FishShrine fishShrine;

    void Start () {
	    allKami = new List<GameObject>();
        fishShrine = Object.FindObjectOfType<FishShrine>();
    }

    //a object has requested to make a fish (or food if isFood is true) at position with rotation, return true if successful
    public bool MakeKami(Vector3 nPosition, Quaternion nRotation, int nW)
    {
        GameObject nObj;

        //check to make sure the w point the fish is being added on isnt full
        if (numKami < maxKami)
        {
			nObj = (GameObject)Instantiate(Resources.Load("Kami/Kami"), nPosition, nRotation);
            nObj.GetComponent<HyperObject>().setW(nW);
            nObj.GetComponent<HyperObject>().WMove(GameObject.Find("CameraRig").GetComponent<HyperCreature>().w);//change to Slide(0) once 4d shader is implemented
            allKami.Add(nObj);
            numKami += 1;
            nObj.GetComponent<Kami>().id = curIndex;
            curIndex++;

            return true;
        }
        return false;
    }

    //a kami has requested to be removed from the world, return true if successful
    public bool RequestToRemove(GameObject rObj)
    {
        if (allKami.Remove(rObj))
        {
            numKami -= 1;

            if (rObj.GetComponent<Kami>().type == Kami.Type.Fish)
                fishShrine.CheckKami();

            Destroy(rObj);

            return true;
        }
        else
            return false;
    }

    public void MakeKamiFlee()
    {
        foreach(GameObject kami in allKami)
        {
            if((int)kami.GetComponent<Kami>().state != 2)
            {
                kami.GetComponent<Kami>().state = Kami.State.Flee;
                break;
            }
        }
    }
}
