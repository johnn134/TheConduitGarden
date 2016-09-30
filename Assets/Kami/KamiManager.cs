using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KamiManager : MonoBehaviour {

    //list of all kami in the world
    List<GameObject> allKami;

    //max number of kami allowed in the world
    public int maxKami = 30;

    //number of kami in the world of each type
    int[] numKami = new int[] { 0, 0, 0 };

    //current Kami index to set the id of new kami
    int curIndex = 0;

    public int kamiComeRate = 60;
    public int kamiLeaveRate = 60;
    public int kamiArriveTime = 10;

    public Vector3 wanderArea1;                 //point 1 of the area of possible wander locations
    public Vector3 wanderArea2;                 //point 2 of the area of possible wander locations

    public static KamiManager instance = null;

    void Awake()
    {
        //declare as singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

		allKami = new List<GameObject>();
    }

    /*
     * An object has requested to make a fish (or food if isFood is true)
     * at position with rotation
     * return true if successful
     */
    public bool MakeKami(Vector3 nPosition, Quaternion nRotation, int nW, int newType)
    {
        GameObject nObj;

        //check to make sure the w point the fish is being added on isnt full
        if (numKami[newType] < maxKami)
        {
			nObj = (GameObject)Instantiate(Resources.Load("Kami/Kami"), nPosition, nRotation);
            nObj.GetComponent<Kami>().type = (Kami.Type)newType;
            allKami.Add(nObj);
            numKami[newType] += 1;
            nObj.GetComponent<Kami>().id = curIndex;
            curIndex++;

            return true;
        }
        return false;
    }

    /*
     * A kami has requested to be removed from the world
     * return true if successful
     */
    public bool RequestToRemove(GameObject rObj)
    {
        if (allKami.Remove(rObj))
        {
            numKami[(int)rObj.GetComponent<Kami>().type] -= 1;

            Destroy(rObj);

            return true;
        }
        else
            return false;
    }

    public void MakeKamiFlee(int kamiType)
    {
        foreach(GameObject kami in allKami)
        {
            if((int)kami.GetComponent<Kami>().state != 2 && (int)kami.GetComponent<Kami>().type == kamiType)
            {
                kami.GetComponent<Kami>().state = Kami.State.Flee;
                break;
            }
        }
    }

    public void MakeKamiEnd()
    {
        foreach (GameObject kami in allKami)
        {
            if ((int)kami.GetComponent<Kami>().state != 4)
            {
                kami.GetComponent<Kami>().state = Kami.State.Ending;
            }
        }
    }

    public int NumberOfHappyKami(int kamiType)
    {
        int numHappy = 0;
        foreach (GameObject kami in allKami)
        {
            if ((int)kami.GetComponent<Kami>().state == 0 && (int)kami.GetComponent<Kami>().type == kamiType)
                numHappy++;
        }

        return numHappy;
    }

	public void MakeKamiSad() {
		foreach (GameObject kami in allKami)
		{
			kami.GetComponent<Kami>().state = Kami.State.Sad;
		}
	}

	public int getNumberOfKami() {
		return allKami.Count;
	}
}
