using UnityEngine;
using System.Collections;

public class Fish : MonoBehaviour {
	public enum Size{
		Small,
		Medium,
		Large
	}
	public enum State{
		Happy,
		Hungry,
		Hunting,
		Dead
	}

	public Size size = Size.Large;
	public State state = State.Happy;
	public int feedCycle = 30;
	Vector3 wanderLoc;
    public float maxSize = .66f;
    public float minSize = .33f;
    public Vector3 wanderArea1;
    public Vector3 wanderArea2;
    public GameObject target = null;
    int foodsEaten = 0;
    int foodsToGrow = 1;
    GameObject fishManager;
    public bool noTargets = false;
    public bool food = false;
    int curW = -1;
    HyperColliderManager myHyper;
    public bool inWater = false;
    public int offset = 0;
    Transform _cachedTransform;
    
    void Start()
    {
        //locate the fish manager
        fishManager = GameObject.Find("FishManager");

        //wanderLoc = new Vector3(Random.Range(wanderArea1.x, wanderArea2.x), Random.Range(wanderArea1.y, wanderArea2.y), Random.Range(wanderArea1.z, wanderArea2.z));
        wanderLoc = transform.position;
        InvokeRepeating("FeedCycle", Random.Range(feedCycle - offset, feedCycle + offset), feedCycle);
        ChangeSize((Size)Random.Range(0, 3));

        myHyper = GetComponent<HyperColliderManager>();
    }

    void Update()
    {
        //only perform the behavior if in water
        if (inWater)
        {
            //over ride behaviors if there is food
            if (food && state != State.Happy)
            {
                BehaveFood();
            }
            else {
                //Behave based on state
                if (state == State.Happy)
                    BehaveHappy();
                else if (state == State.Hungry)
                    BehaveHungry();
                else if (state == State.Hunting)
                    BehaveHunting();
            }

            //check if w has changed
            if (myHyper.w != curW)
            {
                if (curW != -1)
                    fishManager.GetComponent<FishManager>().alertMove(gameObject, curW, myHyper.w);
                curW = myHyper.w;
            }
        }
    }

    void OnCollisionStay(Collision other)
    {
        //did the fish reach the target
        if (other.gameObject.Equals(target))
        {
            if (fishManager.GetComponent<FishManager>().RequestEat(this.gameObject, target))
            {
                if (target.name.StartsWith("Fish"))
                    target.GetComponent<Fish>().state = State.Dead;
                DoEat();
                fishManager.GetComponent<FishManager>().RequestToRemove(target, true);
            }
        }
    }

    //function called by water to let the fish know if it is in the water or not
    public void InWater(bool isIn)
    {
        if (isIn)
        {
            GetComponent<Rigidbody>().drag = 20;
            GetComponent<Rigidbody>().angularDrag = 5;
            GetComponent<Rigidbody>().useGravity = false;
            inWater = true;
            wanderLoc = new Vector3(Random.Range(wanderArea1.x, wanderArea2.x), Random.Range(wanderArea1.y, wanderArea2.y), Random.Range(wanderArea1.z, wanderArea2.z));
        }
        else
        {
            GetComponent<Rigidbody>().drag = 0;
            GetComponent<Rigidbody>().angularDrag = .05f;
            GetComponent<Rigidbody>().useGravity = true;
            inWater = false;
        }
    }

    //the happy behavior of a fish, wander to the wander locations
    void BehaveHappy()
    {
        //move forwards while restricting speed
        transform.Translate(Vector3.forward * Time.deltaTime / 2);

        //smoothly turn towards the wander location
        if (transform.rotation != Quaternion.LookRotation(wanderLoc - transform.position))
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(wanderLoc - transform.position),
                Time.deltaTime
                );
        }

        //change locaiton randomly or if reached the target locaiton
        if (transform.position.Equals(wanderLoc) || Random.Range(0, 300) == 1)
        {
            wanderLoc = new Vector3(Random.Range(wanderArea1.x, wanderArea2.x), Random.Range(wanderArea1.y, wanderArea2.y), Random.Range(wanderArea1.z, wanderArea2.z));

            while(wanderLoc.x < (wanderArea2.x - wanderArea1.x) / 4 - wanderArea2.x && wanderLoc.x > (wanderArea2.x - wanderArea1.x) / 4 + wanderArea1.x &&
                wanderLoc.z < (wanderArea2.z - wanderArea1.z) / 4 - wanderArea2.z && wanderLoc.z > (wanderArea2.z - wanderArea1.z) / 4 + wanderArea1.z)
            {
                wanderLoc = new Vector3(Random.Range(wanderArea1.x, wanderArea2.x), Random.Range(wanderArea1.y, wanderArea2.y), Random.Range(wanderArea1.z, wanderArea2.z));
            }
        }
    }

    //the hungry behavior of a fish, wander to the wander locations slowly
    void BehaveHungry()
    {
        //move forwards while restricting speed
        transform.Translate(Vector3.forward * Time.deltaTime / 10);

        //smoothly turn towards the wander location
        if (transform.rotation != Quaternion.LookRotation(wanderLoc - transform.position))
        {
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(wanderLoc - transform.position),
                Time.deltaTime
                );
        }

        //change locaiton randomly or if reached the target locaiton
        if (transform.position.Equals(wanderLoc) || Random.Range(0, 300) == 1)
        {
            wanderLoc = new Vector3(Random.Range(wanderArea1.x, wanderArea2.x), Random.Range(wanderArea1.y, wanderArea2.y), Random.Range(wanderArea1.z, wanderArea2.z));
        }
    }

    //look for a fish to eat and speed towards that fish
    void BehaveHunting()
    {
        //does the fish have a target already?
        if (target)
        {
            //is the target still valid, can be invalid if on another w or has grown larger than this fish
            if (target.GetComponent<HyperColliderManager>().w == GetComponent<HyperColliderManager>().w && target.GetComponent<Fish>().size <= size)
            {
                //move forwards while restricting speed
                transform.Translate(Vector3.forward * Time.deltaTime);

                //smoothly turn towards the target location
                if (transform.rotation != Quaternion.LookRotation(target.transform.position - transform.position))
                {
                    transform.rotation = Quaternion.Slerp(
                        transform.rotation,
                        Quaternion.LookRotation(target.transform.position - transform.position),
                        Time.deltaTime * 10
                        );
                }
            }
            else {//look for a new target
                target = fishManager.GetComponent<FishManager>().RequestTarget(this.gameObject, false);
            }
        }
        else {//fish does not have a target
              //are there valid targets?
            if (!noTargets)
            {
                target = fishManager.GetComponent<FishManager>().RequestTarget(this.gameObject, false);
            }
            else
            {
                BehaveHungry();
            }
        }
    }

    //look for the closest instant of food, does not divert the fish from any previous targets
    void BehaveFood()
    {
        //does the fish have a target already?
        if (target)
        {
            //is the target still valid, can be invalid if on another w or has grown larger than this fish
            if (target.GetComponent<HyperColliderManager>().w == GetComponent<HyperColliderManager>().w)
            {
                if (Vector3.Distance(transform.position, target.transform.position) < .5f)
                {
                    transform.Translate(Vector3.forward * Time.deltaTime / 2);

                    //smoothly turn towards the target location
                    if (transform.rotation != Quaternion.LookRotation(target.transform.position - transform.position))
                    {
                        transform.rotation = Quaternion.Slerp(
                            transform.rotation,
                            Quaternion.LookRotation(target.transform.position - transform.position),
                            Time.deltaTime * 10
                            );
                    }
                }
                else
                {
                    transform.Translate(Vector3.forward * Time.deltaTime / 6);

                    //smoothly turn towards the target location
                    if (transform.rotation != Quaternion.LookRotation(target.transform.position - transform.position))
                    {
                        transform.rotation = Quaternion.Slerp(
                            transform.rotation,
                            Quaternion.LookRotation(target.transform.position - transform.position),
                            Time.deltaTime
                            );
                    }
                }
            }
            else {//look for a new target, can change from fish to food
                target = fishManager.GetComponent<FishManager>().RequestTarget(this.gameObject, true);
            }
        }
        else {//fish does not have a target
              //are there valid targets?
            if (food)
            {
                target = fishManager.GetComponent<FishManager>().RequestTarget(this.gameObject, true);
            }
        }
    }

    //change the state of the fish
    void FeedCycle()
    {
        state += 1;

        //stop movement and dull color
        if (state == State.Hungry)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<HyperObject>().dullCoef = 2;
                child.GetComponent<HyperObject>().WMove();
            }

        }
        else if (state == State.Dead)
            fishManager.GetComponent<FishManager>().RequestToRemove(gameObject, true);
    }

    //this fish has eaten so become happy, restart the feed cycle and reset color
    void DoEat()
    {
        state = State.Happy;
        foreach (Transform child in transform)
        {
            child.GetComponent<HyperObject>().dullCoef = 1;
            child.GetComponent<HyperObject>().WMove();
        }

        //restart feeding cycle
        CancelInvoke();
        InvokeRepeating("FeedCycle", feedCycle, feedCycle);

        //see if this fish has eaten enough to grow
        foodsEaten++;
        if (foodsEaten == foodsToGrow)
        {
            foodsEaten = 0;
            ChangeSize(size + 1);
        }

        noTargets = false;
    }

    //change the size of the fish
    public void ChangeSize(Size newSize)
    {
        //make sure not trying to change to a size larger than large
        if (!(newSize > Size.Large))
        {
            size = newSize;
            if (size == Size.Small)
            {
                transform.localScale = new Vector3(minSize, minSize, minSize);
            }
            else if (size == Size.Medium)
            {
                transform.localScale = new Vector3(maxSize - (maxSize - minSize)/2, maxSize - (maxSize - minSize) / 2, maxSize - (maxSize - minSize) / 2);
            }
            else if (size == Size.Large)
            {
                transform.localScale = new Vector3(maxSize, maxSize, maxSize);
            }
            else
            {
                Debug.Log("NOT A SIZE");
            }
        }
    }

	public void updateWVisual() {
		transform.GetChild(0).GetComponent<HyperObject>().WMove();
		transform.GetChild(1).GetComponent<HyperObject>().WMove();
		transform.GetChild(2).GetComponent<HyperObject>().WMove();
		transform.GetChild(3).GetComponent<HyperObject>().WMove();
		transform.GetChild(4).GetComponent<HyperObject>().WMove();
		transform.GetChild(5).GetComponent<HyperObject>().WMove();
	}
}
