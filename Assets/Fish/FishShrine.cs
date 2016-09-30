using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishShrine : MonoBehaviour {

	public GameObject[] redLights, yellowLights, cyanLights, magentaLights;

    public int[] onWs = new int[] {0, 0, 0, 0, 0 };	//the number of fish required on each w point to activate the shrine

	KamiManager kamiManager;						//reference to the kami manager

	ParticleSystem particleObj_LA, particleObj_LB, particleObj_RA, particleObj_RB;

    float maxPoints = 0.0f;							//the total points the shine needs to activate
    float points = 0.0f;							//the current number of points the shrine has

	int stage;										//how many thirds of the way the shrine is to activation

    bool fullyActivated;							//is the shine activated

	void Awake() {
		fullyActivated = false;

		stage = 0;

		//Cache particle systems
		particleObj_LA = transform.GetChild(1).GetComponent<ParticleSystem>();
		particleObj_LB = transform.GetChild(1).GetChild(0).GetComponent<ParticleSystem>();

		particleObj_RA = transform.GetChild(2).GetComponent<ParticleSystem>();
		particleObj_RB = transform.GetChild(2).GetChild(0).GetComponent<ParticleSystem>();

		foreach (int node in onWs)
			maxPoints += node;

		//Deactivate lights
		updateLights(new int[] { 0, 0, 0, 0, 0 });
	}

	void Start() {
		kamiManager = KamiManager.instance;
    }
	
    public void processFish(List<GameObject> allFish)
    {
		int[] pointMatrix = new int[] { 0, 0, 0, 0, 0 };
		float oldPoints = points;
		points = 0;

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

		if(points > oldPoints) {
			particleObj_LA.Play();
			particleObj_LB.Play();
			particleObj_RA.Play();
			particleObj_RB.Play();
		}

		updateLights(pointMatrix);

        if(points >= maxPoints / 3 && stage == 0 ||
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
            if(kamiManager.NumberOfHappyKami(0) > 0)
                ScareKami();
            else
            {
                //cancel all invoked make kami scrips and restart the ones that would not have been scared away
                CancelInvoke();

                for(int i = 0; i < stage - kamiManager.NumberOfHappyKami(0); i++)
                    Invoke("MakeKami", kamiManager.kamiArriveTime);
            }
        }

		if (points >= maxPoints && !fullyActivated)
        {
            CancelInvoke();	//clear invoked methods

			//Spawn remaining kami
            for (int i = 0; i < stage - kamiManager.NumberOfHappyKami(0); i++)
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

	void updateLights(int[] fish) {
		Color temp = Color.white;

		if(redLights.Length == 3) {
			if(redLights[0] != null) {
				temp = fish[0] >= 1 ? Color.red : (Color.white * 0.1f);
				redLights[0].GetComponent<Renderer>().material.color = temp;
			}
			if(redLights[1] != null) {
				temp = fish[0] >= 2 ? Color.red : (Color.white * 0.1f);
				redLights[1].GetComponent<Renderer>().material.color = temp;
			}
			if(redLights[2] != null) {
				temp = fish[0] >= 3 ? Color.red : (Color.white * 0.1f);
				redLights[2].GetComponent<Renderer>().material.color = temp;
			}
		}
		if(yellowLights.Length == 3) {
			if(yellowLights[0] != null) {
				temp = fish[1] >= 1 ? Color.yellow : (Color.white * 0.1f);
				yellowLights[0].GetComponent<Renderer>().material.color = temp;
			}
			if(yellowLights[1] != null) {
				temp = fish[1] >= 2 ? Color.yellow : (Color.white * 0.1f);
				yellowLights[1].GetComponent<Renderer>().material.color = temp;
			}
			if(yellowLights[2] != null) {
				temp = fish[1] >= 3 ? Color.yellow : (Color.white * 0.1f);
				yellowLights[2].GetComponent<Renderer>().material.color = temp;
			}
		}
		if(cyanLights.Length == 3) {
			if(cyanLights[0] != null) {
				temp = fish[3] >= 1 ? Color.cyan : (Color.white * 0.1f);
				cyanLights[0].GetComponent<Renderer>().material.color = temp;
			}
			if(cyanLights[1] != null) {
				temp = fish[3] >= 2 ? Color.cyan : (Color.white * 0.1f);
				cyanLights[1].GetComponent<Renderer>().material.color = temp;
			}
			if(cyanLights[2] != null) {
				temp = fish[3] >= 3 ? Color.cyan : (Color.white * 0.1f);
				cyanLights[2].GetComponent<Renderer>().material.color = temp;
			}
		}
		if(magentaLights.Length == 3) {
			if(magentaLights[0] != null) {
				temp = fish[4] >= 1 ? Color.magenta : (Color.white * 0.1f);
				magentaLights[0].GetComponent<Renderer>().material.color = temp;
			}
			if(magentaLights[1] != null) {
				temp = fish[4] >= 2 ? Color.magenta : (Color.white * 0.1f);
				magentaLights[1].GetComponent<Renderer>().material.color = temp;
			}
			if(magentaLights[2] != null) {
				temp = fish[4] >= 3 ? Color.magenta : (Color.white * 0.1f);
				magentaLights[2].GetComponent<Renderer>().material.color = temp;
			}
		}
	}

    void MakeKami()
    {
		kamiManager.MakeKami(kamiManager.transform.position, transform.rotation, Random.Range(0, HyperObject.W_RANGE + 1), 0);
    }

    void ScareKami()
    {
        //check if there are kami to scare away
        if (kamiManager.NumberOfHappyKami(0) > stage)
            kamiManager.MakeKamiFlee(0);
        else
        {
            //cancel all invoked make kami scrips and restart the ones that would not have been scared away
            CancelInvoke();

            for (int i = 0; i < stage - kamiManager.NumberOfHappyKami(0); i++)
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
