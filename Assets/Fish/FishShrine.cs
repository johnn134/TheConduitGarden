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
    ParticleSystem particleObj;                                 //the particle emmiter on this shrine

    //Chached light transforms
    Transform[] lights = new Transform[] { null, null, null, null, null, null, null };

    void Start() {
        fishManager = Object.FindObjectOfType<FishManager>();
        kamiManager = Object.FindObjectOfType<KamiManager>();

        particleObj = GameObject.Find("ShrineFish/Particles").GetComponent<ParticleSystem>();

        var em = particleObj.emission;
        em.rate = 0;

        foreach (int node in onWs)
            maxPoints += node;

        //cache the transforms of all the lights and set their color
        lights[0] = GameObject.Find("ShrineFish/Visual/LevelOneLight").transform;
        lights[0].gameObject.GetComponent<Renderer>().material.color = Color.red;

        lights[1] = GameObject.Find("ShrineFish/Visual/LevelTwoLight").transform;
        lights[1].gameObject.GetComponent<Renderer>().material.color = new Color(1, .45f, 0);

        lights[2] = GameObject.Find("ShrineFish/Visual/LevelThreeLight").transform;
        lights[2].gameObject.GetComponent<Renderer>().material.color = Color.yellow;

        lights[3] = GameObject.Find("ShrineFish/Visual/LevelFourLight").transform;
        lights[3].gameObject.GetComponent<Renderer>().material.color = Color.green;

        lights[4] = GameObject.Find("ShrineFish/Visual/LevelFiveLight").transform;
        lights[4].gameObject.GetComponent<Renderer>().material.color = Color.cyan;

        lights[5] = GameObject.Find("ShrineFish/Visual/LevelSixLight").transform;
        lights[5].gameObject.GetComponent<Renderer>().material.color = Color.blue;

        lights[6] = GameObject.Find("ShrineFish/Visual/LevelSevenLight").transform;
        lights[6].gameObject.GetComponent<Renderer>().material.color = Color.magenta;

        //initalize the lights
        UpdateLights(new float[] { 0, 0, 0, 0, 0, 0, 0 });
    }
	
    public void processFish(List<GameObject> allFish)
    {
        float oldPoints = points;
        points = 0;
        float[] pointMatrix = new float[] { 0, 0, 0, 0, 0, 0, 0 };
        foreach(GameObject fish in allFish)
        {
            int fishW = fish.GetComponent<HyperColliderManager>().w;
            if (fish)
            {
                if (onWs[fishW] > 0 && pointMatrix[fishW] < onWs[fishW] && (int)fish.GetComponent<Fish>().size == 2)
                {
                    pointMatrix[fishW] += 1;
                    points++;
                }
            }
        }

        if(points > oldPoints)
            particleObj.Emit(20);

        /*if (points > 0 && !activated)
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
        }*/

        UpdateLights(pointMatrix);

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
            CancelInvoke();
            var em = particleObj.emission;
            em.rate = 5;
            kamiManager.MakeKami(transform.position, transform.rotation, 0);
            InvokeRepeating("MakeKami", kamiManager.kamiComeRate, kamiManager.kamiComeRate);
            //GetComponent<HyperObject>().dullCoef = .1f;
        }

        if (points < maxPoints && activated)
        {
            activated = false;
            CancelInvoke();
            ScareKami();
            InvokeRepeating("ScareKami", kamiManager.kamiLeaveRate, kamiManager.kamiLeaveRate);
            var em = particleObj.emission;
            em.rate = 0;
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

    void UpdateLights(float[] pointMatrix)
    {
        for(int i = 0; i < 7; i++)
        {
            //skip lights that track a layer where no fish are required for activation
            if(onWs[i] != 0)
            {
                if (pointMatrix[i] != 0)
                    lights[i].localScale = new Vector3(lights[i].localScale.x, .075f / (onWs[i] / pointMatrix[i]), lights[i].localScale.z);
                else
                    lights[i].localScale = new Vector3(lights[i].localScale.x, 0, lights[i].localScale.z);

                lights[i].localPosition = new Vector3(lights[i].localPosition.x, 1.41f + (.1f * i) - (.075f / 2.0f) + (lights[i].localScale.y / 2.0f), lights[i].localPosition.z);
            }
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
