using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravelShrine : MonoBehaviour
{

    int stage = 0;                                              //how many thirds of the way the shrine is to activation

    //public bool[] onWs = new bool[] { false, false, false, false, false, false, false };        //the number of fish required on each w point to activate the shrine

    public GameObject[] pits = new GameObject[] {null, null, null, null, null, null, null};

    float maxPoints = 0.0f;                                     //the total points the shine needs to activate
    float points = 0.0f;                                        //the current number of points the shrine has

    bool activated = false;                                     //is the shine activated

    KamiManager kamiManager;                                    //reference to the kami manager
    ParticleSystem particleObj;                                 //the particle emmiter on this shrine
    HyperCreature player;                                       //reference to the hyper creature

    //Chached light transforms
    Transform[] lights = new Transform[] { null, null, null, null, null, null, null };

    void Start()
    {
        kamiManager = KamiManager.instance;
        player = HyperCreature.instance;

        particleObj = GameObject.Find("ShrineGravelPit/Particles").GetComponent<ParticleSystem>();

        var em = particleObj.emission;
        em.rate = 0;

        foreach (GameObject node in pits)
            if (node)
                maxPoints++;
        //maxPoints *= 2;

        //cache the transforms of all the lights and set their color
        lights[0] = GameObject.Find("ShrineGravelPit/Visual/LevelOneLight").transform;
        lights[0].gameObject.GetComponent<Renderer>().material.color = Color.red;

        lights[1] = GameObject.Find("ShrineGravelPit/Visual/LevelTwoLight").transform;
        lights[1].gameObject.GetComponent<Renderer>().material.color = new Color(1, .45f, 0);

        lights[2] = GameObject.Find("ShrineGravelPit/Visual/LevelThreeLight").transform;
        lights[2].gameObject.GetComponent<Renderer>().material.color = Color.yellow;

        lights[3] = GameObject.Find("ShrineGravelPit/Visual/LevelFourLight").transform;
        lights[3].gameObject.GetComponent<Renderer>().material.color = Color.green;

        lights[4] = GameObject.Find("ShrineGravelPit/Visual/LevelFiveLight").transform;
        lights[4].gameObject.GetComponent<Renderer>().material.color = Color.cyan;

        lights[5] = GameObject.Find("ShrineGravelPit/Visual/LevelSixLight").transform;
        lights[5].gameObject.GetComponent<Renderer>().material.color = Color.blue;

        lights[6] = GameObject.Find("ShrineGravelPit/Visual/LevelSevenLight").transform;
        lights[6].gameObject.GetComponent<Renderer>().material.color = Color.magenta;

        //initalize the lights
        UpdateLights(new float[] { 0, 0, 0, 0, 0, 0, 0 });
    }

    public void processPits()
    {
        float oldPoints = points;
        points = 0;
        float[] pointMatrix = new float[] { 0, 0, 0, 0, 0, 0, 0 };
        int index = 0;
        foreach (GameObject pit in pits)
        {
            if (pit)
            {
                if (pit.GetComponent<Alpha>().isRakedEnough)
                {
                    points++;
                    pointMatrix[index]++;
                }

                //add to point if pattern matched
            }
            index++;
        }

        if (points > oldPoints)
            particleObj.Emit(20);

        UpdateLights(pointMatrix);

        if (points >= maxPoints / 3 && stage == 0 ||
            points >= (maxPoints / 3) * 2 && stage == 1)
        {
            stage++;
            Invoke("MakeKami", kamiManager.kamiArriveTime);
        }

        if (points < maxPoints / 3 && stage == 1 ||
            points < (maxPoints / 3) * 2 && stage == 2)
        {
            stage--;

            //check if there are kami to scare away
            if (kamiManager.NumberOfHappyKami(2) > 0)
                ScareKami();
            else
            {
                //cancel all invoked make kami scrips and restart the ones that would not have been scared away
                CancelInvoke();

                for (int i = 0; i < stage - kamiManager.NumberOfHappyKami(2); i++)
                    Invoke("MakeKami", kamiManager.kamiArriveTime);
            }
        }

        if (points >= maxPoints && !activated)
        {
            activated = true;
            CancelInvoke();
            for (int i = 0; i < stage - kamiManager.NumberOfHappyKami(2); i++)
                Invoke("MakeKami", kamiManager.kamiArriveTime);
            var em = particleObj.emission;
            em.rate = 5;
            player.w_perif++;
            player.WMoveAllHyperObjects();
            //kamiManager.MakeKami(kamiManager.transform.position, transform.rotation, Random.Range(0, 7));
            InvokeRepeating("MakeKami", kamiManager.kamiArriveTime, kamiManager.kamiComeRate);
            //GetComponent<HyperObject>().dullCoef = .1f;
        }

        if (points < maxPoints && activated)
        {
            activated = false;
            CancelInvoke();
            player.w_perif--;
            player.WMoveAllHyperObjects();
            InvokeRepeating("ScareKami", 0, kamiManager.kamiLeaveRate);
            var em = particleObj.emission;
            em.rate = 0;
        }
    }

    void UpdateLights(float[] pointMatrix)
    {
        for (int i = 0; i < 7; i++)
        {
            //skip lights that track a layer where no fish are required for activation
            if (pits[i] != null)
            {
                if (pointMatrix[i] != 0)
                    lights[i].localScale = new Vector3(lights[i].localScale.x, .075f / (2 / pointMatrix[i]), lights[i].localScale.z);
                else
                    lights[i].localScale = new Vector3(lights[i].localScale.x, 0, lights[i].localScale.z);

                lights[i].localPosition = new Vector3(lights[i].localPosition.x, 1.41f + (.1f * i) - (.075f / 2.0f) + (lights[i].localScale.y / 2.0f), lights[i].localPosition.z);
            }
        }
    }

    void MakeKami()
    {
        kamiManager.MakeKami(kamiManager.transform.position, transform.rotation, Random.Range(0, 7), 2);
    }

    void ScareKami()
    {
        //check if there are kami to scare away
        if (kamiManager.NumberOfHappyKami(2) > stage)
            kamiManager.MakeKamiFlee(2);
        else
        {
            //cancel all invoked make kami scrips and restart the ones that would not have been scared away
            CancelInvoke();

            for (int i = 0; i < stage - kamiManager.NumberOfHappyKami(2); i++)
                Invoke("MakeKami", kamiManager.kamiArriveTime);
        }
    }

    /*public void CheckKami()
    {
        if (kamiManager.NumberOfHappyKami(0) == stage || kamiManager.NumberOfHappyKami(0) == 0)
        {
            CancelInvoke();
            for (int i = 0; i < stage - kamiManager.NumberOfHappyKami(0); i++)
                Invoke("MakeKami", kamiManager.kamiArriveTime);
        }
    }*/
}
