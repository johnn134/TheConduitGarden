using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Kami : MonoBehaviour {
    public enum Type
    {
        Fish,
        Bonsai,
        Gravel
    }
    public enum State
    {
        Happy,
        Sad,
        Flee,
        Help,
        Ending
    }

    public GameObject target;
    public Type type;
    public State state;
    public GameObject holding;
    public List<GameObject> targets;
    public HyperCreature player;
    public bool helping;
    HyperObject myHyper;
    public Vector3 standbyLoc;
    Vector3 wanderLoc;
    public int id;
    public Vector3 wanderArea1;
    public Vector3 wanderArea2;
    KamiManager kamiManager;

    void Start()
    {
        player = Object.FindObjectOfType<HyperCreature>();
        targets = new List<GameObject>();
        myHyper = GetComponent<HyperObject>();
        wanderLoc = new Vector3(Random.Range(-6, 7), 3, Random.Range(-6, 7));
        kamiManager = Object.FindObjectOfType<KamiManager>();

        if (type == Type.Fish)
        {
            targets.Add(GameObject.Find("ShrineFish/Sphere"));
            targets.Add(GameObject.Find("ToolFoodContainer"));
            targets.Add(GameObject.Find("Reservoir/Water"));
            targets.Add(GameObject.Find("FishPool/Water"));
        }
    }

    void Update()
    {
        if(state == State.Happy)
        {
            if (target)
                LandOnTarget();
            else
                BehaveHappy();
        }
        else if(state == State.Flee)
        {
            Flee();
        }
    }

    //the happy behavior of a kami, wander to the wander locations
    void BehaveHappy()
    {
        //move forwards while restricting speed
        transform.Translate(Vector3.forward * Time.deltaTime);

        //smoothly turn towards the wander location
        if (transform.rotation != Quaternion.LookRotation(wanderLoc - transform.position))
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(wanderLoc - transform.position),
                Time.deltaTime * 2
                );
        }

        //change locaiton randomly or if reached the target locaiton
        if (transform.position.Equals(wanderLoc) || Random.Range(0, 20) == 1)
        {
            if (Random.Range(0, 50) == 1)
                target = targets[Random.Range(0, targets.Count - 1)];
            else
                wanderLoc = new Vector3(Random.Range(wanderArea1.x, wanderArea2.x), Random.Range(wanderArea1.y, wanderArea2.y), Random.Range(wanderArea1.z, wanderArea2.z));

            /*while (wanderLoc.x < (wanderArea2.x - wanderArea1.x) / 4 - wanderArea2.x && wanderLoc.x > (wanderArea2.x - wanderArea1.x) / 4 + wanderArea1.x &&
                wanderLoc.z < (wanderArea2.z - wanderArea1.z) / 4 - wanderArea2.z && wanderLoc.z > (wanderArea2.z - wanderArea1.z) / 4 + wanderArea1.z)
            {
                wanderLoc = new Vector3(Random.Range(wanderArea1.x, wanderArea2.x), Random.Range(wanderArea1.y, wanderArea2.y), Random.Range(wanderArea1.z, wanderArea2.z));
            }*/
        }
    }

    //run away from the garden
    void Flee()
    {
        wanderLoc = new Vector3(wanderLoc.x, 20, wanderLoc.z);

        //move forwards while restricting speed
        transform.Translate(Vector3.forward * Time.deltaTime * 3);

        //smoothly turn towards the wander location
        if (transform.rotation != Quaternion.LookRotation(wanderLoc - transform.position))
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(wanderLoc - transform.position),
                Time.deltaTime * 2
                );
        }

        if (Vector3.Distance(transform.position, wanderLoc) < .2)
        {
            kamiManager.RequestToRemove(gameObject);
        }
            
    }

    void LandOnTarget()
    {
        wanderLoc = new Vector3(target.transform.position.x, target.transform.position.y + (target.transform.lossyScale.y / 2) + (transform.lossyScale.y / 2), target.transform.position.z);

        //move forwards while restricting speed
        if(Vector3.Distance(transform.position, wanderLoc) < .05)
            transform.Translate(Vector3.forward * Time.deltaTime / 10);
        else if (Vector3.Distance(transform.position, wanderLoc) < .2)
            transform.Translate(Vector3.forward * Time.deltaTime / 6);
        else if (Vector3.Distance(transform.position, wanderLoc) < .3)
            transform.Translate(Vector3.forward * Time.deltaTime / 5);
        else if (Vector3.Distance(transform.position, wanderLoc) < .4)
            transform.Translate(Vector3.forward * Time.deltaTime / 4);
        else if (Vector3.Distance(transform.position, wanderLoc) <.5)
            transform.Translate(Vector3.forward * Time.deltaTime / 2);
        else
            transform.Translate(Vector3.forward * Time.deltaTime);

        //smoothly turn towards the wander location
        if (transform.rotation != Quaternion.LookRotation(wanderLoc - transform.position))
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(wanderLoc - transform.position),
                Time.deltaTime * 2
                );
        }
            

        //change target
        if (Random.Range(0, 800) == 1)
            target = null;
    }

    /*void HelpTut()
    {
        //help the player learn how to care for fish
        if (helping)
        {
            if (!holding && !transform.position.Equals(targets[2].transform.position)) //Step 1: move to reservoir to look for a fish
            {
                Debug.Log("STEP 1");
                transform.position = Vector3.MoveTowards(transform.position, targets[2].transform.position, Time.deltaTime / 4);
            }
            else if (!holding && myHyper.w != targets[2].GetComponent<HyperObject>().w)//Step 2: move to the w of the reservoir
            {
                Debug.Log("STEP 2");
                myHyper.w = targets[2].GetComponent<HyperObject>().w;
                myHyper.WMove(player.w);
            }
            else if (!holding && myHyper.w == player.w)//Step 3: grab a fish once the player matches the w
            {
                Debug.Log("STEP 3");
                holding = GameObject.Find("Fish(Clone)");
            }
            else if (holding && !transform.position.Equals(targets[3].transform.position))//Step 4: move the fish over the fish pool
            {
                Debug.Log("STEP 4");
                transform.position = Vector3.MoveTowards(transform.position, targets[3].transform.position, Time.deltaTime / 4);
                holding.transform.position = transform.position;
                holding.transform.rotation = transform.rotation;
                holding.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                holding.GetComponent<HyperColliderManager>().movable = false;
            }
            else if (holding && myHyper.w != targets[3].GetComponent<HyperObject>().w)//Step 5: change to fish pool w
            {
                Debug.Log("STEP 5");
                myHyper.w = targets[3].GetComponent<HyperObject>().w;
                myHyper.WMove(player.w);
                holding.GetComponent<HyperColliderManager>().setW(myHyper.w);
                holding.GetComponent<HyperColliderManager>().WMove(player.w);
                holding.GetComponent<HyperColliderManager>().SetCollisions();
            }
            else if (holding && myHyper.w != player.w)//Step 6: wait for player to match w
            {
                Debug.Log("STEP 6");
                holding.transform.position = transform.position;
                holding.transform.rotation = transform.rotation;
                holding.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
                holding.GetComponent<HyperColliderManager>().movable = false;
            }
            else if (holding && myHyper.w == player.w)//Step 7: release the fish and stop helping
            {
                Debug.Log("STEP 7");
                holding.GetComponent<HyperColliderManager>().movable = true;
                helping = false;
            }
        }
        else if (!transform.position.Equals(standbyLoc))
        {
            transform.position = Vector3.MoveTowards(transform.position, standbyLoc, Time.deltaTime);
        }
    }*/

    /*void Update () {
        //temp key input to test helping the player
        if (Input.GetKey(KeyCode.Alpha1))
        {
            GetComponent<HyperObject>().w = 0;
            GetComponent<HyperObject>().WMove(0);
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            GetComponent<HyperObject>().w = 1;
            GetComponent<HyperObject>().WMove(1);
        }
        if (Input.GetKey(KeyCode.Alpha3))
        {
            GetComponent<HyperObject>().w = 2;
            GetComponent<HyperObject>().WMove(2);
        }
        if (Input.GetKey(KeyCode.Alpha4))
        {
            GetComponent<HyperObject>().w = 3;
            GetComponent<HyperObject>().WMove(3);
        }
        if (Input.GetKey(KeyCode.Alpha5))
        {
            GetComponent<HyperObject>().w = 4;
            GetComponent<HyperObject>().WMove(4);
        }
        if (Input.GetKey(KeyCode.Alpha6))
        {
            GetComponent<HyperObject>().w = 5;
            GetComponent<HyperObject>().WMove(5);
        }
        if (Input.GetKey(KeyCode.Alpha7))
        {
            GetComponent<HyperObject>().w = 6;
            GetComponent<HyperObject>().WMove(6);
        }

        if (target)
        {
            if (Vector3.Distance(transform.position, target.transform.position) > .5)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * 2); 
            }
            else
                transform.RotateAround(target.transform.position, Vector3.up, Time.deltaTime * 100);

            if (transform.position.y != target.transform.position.y + .3f)
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, target.transform.position.y + .3f, transform.position.z), Time.deltaTime /2);
        }
    }*/
}
