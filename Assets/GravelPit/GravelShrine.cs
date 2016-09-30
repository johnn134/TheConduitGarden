using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GravelShrine : MonoBehaviour {

	public GameObject[] lights;
	public GameObject[] pits = new GameObject[] {null, null, null, null, null};

	KamiManager kamiManager;                                    //reference to the kami manager

	ParticleSystem particleObj_LA, particleObj_LB, particleObj_RA, particleObj_RB;

	float maxPoints;                                     //the total points the shine needs to activate
	float points;                                        //the current number of points the shrine has

	int stage;                                              //how many thirds of the way the shrine is to activation

	bool fullyActivated;                                     //is the shine activated

	void Awake() {
		fullyActivated = false;

		maxPoints = 0.0f;
		points = 0.0f;
		stage = 0;

		//Cache particle systems
		particleObj_LA = transform.GetChild(1).GetComponent<ParticleSystem>();
		particleObj_LB = transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>();

		particleObj_RA = transform.GetChild(2).GetComponent<ParticleSystem>();
		particleObj_RB = transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>();

		foreach(GameObject g in pits) {
			if(g != null)
				maxPoints += 2;
		}

		//Deactivate lights
		updateLights(new int[] { 0, 0, 0, 0, 0 });
	}

	void Start() {
		kamiManager = KamiManager.instance;
		player = HyperCreature.instance;
    }

    public void processPits()
    {
		int[] pointMatrix = new int[] { 0, 0, 0, 0, 0 };
		float oldPoints = points;
		points = 0;

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

				if (pit.GetComponent<PatternRecognition>().patternMatches) {
					points++;
					pointMatrix[index]++;
				}
                //add to point if pattern matched
            }
            index++;
        }

		if(points > oldPoints) {
			particleObj_LA.Play();
			particleObj_LB.Play();
			particleObj_RA.Play();
			particleObj_RB.Play();
		}

		updateLights(pointMatrix);

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

		if (points >= maxPoints && !fullyActivated)
        {
			CancelInvoke();	//clear invoked methods

			//Spawn remaining kami
            for (int i = 0; i < stage - kamiManager.NumberOfHappyKami(2); i++)
				Invoke("MakeKami", kamiManager.kamiArriveTime);

			//start repeating kami spawning
			InvokeRepeating("MakeKami", kamiManager.kamiArriveTime, kamiManager.kamiComeRate);

			//Emit Particles
			particleObj_LA.loop = true;
			particleObj_LB.loop = true;
			particleObj_RA.loop = true;
			particleObj_RB.loop = true;

			//Increase peripheral vision

			fullyActivated = true;
        }

		if (points < maxPoints && fullyActivated)
        {
			CancelInvoke();	//clear invoked methods
			InvokeRepeating("ScareKami", 0, kamiManager.kamiLeaveRate);

			//Stop emmitting particles
			particleObj_LA.loop = false;
			particleObj_LB.loop = false;
			particleObj_RA.loop = false;
			particleObj_RB.loop = false;

			//Decrease peripheral vision

			fullyActivated = false;
        }
	}

	void updateLights(int[] pointMatrix) {
		if(lights.Length == 10) {
			//Red
			lights[0].GetComponent<Renderer>().material.color = pointMatrix[0] >= 1 ? Color.red : (Color.white * 0.1f);
			lights[1].GetComponent<Renderer>().material.color = pointMatrix[0] >= 2 ? Color.red : (Color.white * 0.1f);

			//Yellow
			lights[2].GetComponent<Renderer>().material.color = pointMatrix[1] >= 1 ? Color.yellow : (Color.white * 0.1f);
			lights[3].GetComponent<Renderer>().material.color = pointMatrix[1] >= 2 ? Color.yellow : (Color.white * 0.1f);

			//Green
			lights[4].GetComponent<Renderer>().material.color = pointMatrix[2] >= 1 ? Color.green : (Color.white * 0.1f);
			lights[5].GetComponent<Renderer>().material.color = pointMatrix[2] >= 2 ? Color.green : (Color.white * 0.1f);

			//Cyan
			lights[6].GetComponent<Renderer>().material.color = pointMatrix[3] >= 1 ? Color.cyan : (Color.white * 0.1f);
			lights[7].GetComponent<Renderer>().material.color = pointMatrix[3] >= 2 ? Color.cyan : (Color.white * 0.1f);
		
			//Magenta
			lights[8].GetComponent<Renderer>().material.color = pointMatrix[4] >= 1 ? Color.magenta : (Color.white * 0.1f);
			lights[9].GetComponent<Renderer>().material.color = pointMatrix[4] >= 2 ? Color.magenta : (Color.white * 0.1f);
		}
	}

    void MakeKami()
    {
		kamiManager.MakeKami(kamiManager.transform.position, transform.rotation, Random.Range(0, HyperObject.W_RANGE + 1), 2);
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
