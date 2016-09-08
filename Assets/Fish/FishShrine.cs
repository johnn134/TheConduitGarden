using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishShrine : MonoBehaviour {

    int stage = 0;                                              //how many thirds of the way the shrine is to activation

    public int[] onWs = new int[] {0, 0, 0, 0, 0, 0, 0};        //the number of fish required on each w point to activate the shrine

    float maxPoints = 0.0f;                                     //the total points the shine needs to activate
    float points = 0.0f;                                        //the current number of points the shrine has

    bool activated = false;                                     //is the shine activated

    FishManager fishManager;                                    //reference to the fish manager
    KamiManager kamiManager;                                    //reference to the kami manager
    GameObject particleObj;                                     //the particle emmiter on this shrine

	void Start () {
        fishManager = Object.FindObjectOfType<FishManager>();
        kamiManager = Object.FindObjectOfType<KamiManager>();

        particleObj = GameObject.Find("ShrineFish/Particles");

        particleObj.GetComponent<ParticleSystem>().emissionRate = 0;

        foreach (int node in onWs)
            maxPoints += node;

        Debug.Log("MAX POINTS: " + maxPoints);
    }
	
    public void processFish(List<GameObject> allFish)
    {
        float oldPoints = points;
        points = 0;
        int[] pointMatrix = new int[] { 0, 0, 0, 0, 0, 0, 0 };
        foreach(GameObject fish in allFish)
        {
            if (fish)
            {
                if (onWs[fish.GetComponent<HyperColliderManager>().w] > 0 && pointMatrix[fish.GetComponent<HyperColliderManager>().w] < onWs[fish.GetComponent<HyperColliderManager>().w] && (int)fish.GetComponent<Fish>().size == 2)
                {
                    pointMatrix[fish.GetComponent<HyperColliderManager>().w] += 1;
                    points++;
                }
            }
        }

        if(points > oldPoints)
            particleObj.GetComponent<ParticleSystem>().Emit(20);

        if (points > 0 && !activated)
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<HyperObject>())
                {
                    child.GetComponent<HyperObject>().dullCoef = (maxPoints / points) / 2;
                    child.GetComponent<HyperObject>().WMove();
                }
            }
        }
        else if (points == 0)
        {
            foreach (Transform child in transform)
            {
                if (child.GetComponent<HyperObject>())
                {
                    child.GetComponent<HyperObject>().dullCoef = maxPoints;
                    child.GetComponent<HyperObject>().WMove();
                }
            }
        }

        Debug.Log("Matrix: " + pointMatrix[0] + ", "
                             + pointMatrix[1] + ", "
                             + pointMatrix[2] + ", "
                             + pointMatrix[3] + ", "
                             + pointMatrix[4] + ", "
                             + pointMatrix[5] + ", "
                             + pointMatrix[6]);

        if(points >= maxPoints/3 && stage == 0 ||
            points >= (maxPoints/3)*2 && stage == 1)
        {
            stage++;
            kamiManager.MakeKami(transform.position, transform.rotation, 0);
        }

        if (points < maxPoints / 3 && stage == 1 ||
            points < (maxPoints / 3) * 2 && stage == 2)
        {
            stage--;
            ScareKami();
        }

        if (points >= maxPoints && !activated)
        {
            activated = true;
            Debug.Log("ACTIVATED");
            CancelInvoke();
            particleObj.GetComponent<ParticleSystem>().emissionRate = 5;
            kamiManager.MakeKami(transform.position, transform.rotation, 0);
            InvokeRepeating("MakeKami", kamiManager.kamiComeRate, kamiManager.kamiComeRate);
            //GetComponent<HyperObject>().dullCoef = .1f;
        }

        if (points < maxPoints && activated)
        {
            activated = false;
            Debug.Log("DEACTIVATED");
            CancelInvoke();
            ScareKami();
            InvokeRepeating("ScareKami", kamiManager.kamiLeaveRate, kamiManager.kamiLeaveRate);
            particleObj.GetComponent<ParticleSystem>().emissionRate = 0;
            /*foreach (Transform child in transform)
            {
                if (child.GetComponent<HyperObject>())
                {
                    child.GetComponent<HyperObject>().dullCoef = (maxPoints / points) / 2;
                    child.GetComponent<HyperObject>().WMove(GameObject.Find("CameraRig").GetComponent<HyperCreature>().w);
                }
            }*/
        }
    }

    void MakeKami()
    {
        kamiManager.MakeKami(transform.position, transform.rotation, 0);
    }

    void ScareKami()
    {
        kamiManager.MakeKamiFlee();
    }

    public void CheckKami()
    {
        if (kamiManager.numKami == stage || kamiManager.numKami == 0)
            CancelInvoke();
    }
}
