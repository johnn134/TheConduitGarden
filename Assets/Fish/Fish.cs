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

	public Size size = Size.Large;                  //the size of the fish
	public State state = State.Happy;               //the hunger state of the fish

	public int feedCycle = 30;                      //the length in seconds of time between hunger states
    public int offset = 0;                          //how big the time offset is for the first feed cycle

    int foodsEaten = 0;                             //how many times has the fish eaten
    int foodsToGrow = 1;                            //how many times the fish needs to eat in order to grow bigger
    int curW = -1;                                  //the current w position the fish thinks it is on

    public float maxSize = .66f;                    //the size of the fish at the largest size
    public float minSize = .33f;                    //the size of the fish at the smallest size
    public float happySpeed = .5f;                  //the speed of the fish when happy
    public float hungrySpeed = .1f;                 //the speed of the fish when hungry
    public float huntingSpeed = 1f;                 //the speed of the fish when hunting

    public Vector3 wanderArea1;                     //point 1 of the area the fish can swin in
    public Vector3 wanderArea2;                     //point 2 of the area the fish can swin in

    Vector3 wanderLoc;                              //the point that the fish is trying to swim to
    
    public GameObject target = null;                       //the game object that the fish is trying to swim to

    FishManager fishManager;                        //reference to the fish manager

    public bool noTargets = false;                         //are there no valid targets
    public bool food = false;                              //is there valid food in the pool
    public bool inWater = false;                           //is the fish in water

    HyperColliderManager myHyper;                   //reference to the hyper script

    Transform _cachedTransform;                     //the transform of the fish

    Rigidbody _cachedRigidBody;                     //the rigid body on the fish
    
    void Awake()
    {
        //cache components to improve performance
        myHyper = GetComponent<HyperColliderManager>();
        _cachedTransform = transform;
        _cachedRigidBody = GetComponent<Rigidbody>();
    }

    void Start()
    {
        //locate the fish manager
        fishManager = Object.FindObjectOfType<FishManager>();

        //set the wander location up to get a random point once the behavior starts
        wanderLoc = _cachedTransform.position;

        //start the feed cycles with the first one effected by the offset
        InvokeRepeating("FeedCycle", Random.Range(feedCycle - offset, feedCycle + offset), feedCycle);

        //pick a random size
        ChangeSize((Size)Random.Range(0, 3));
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
                //dont alert for the first time since the fish starts out not knowing its w
                if (curW != -1)
                    fishManager.alertMove(gameObject, myHyper.w);
                curW = myHyper.w;
            }
        }
    }

    void OnCollisionStay(Collision other)
    {
        //did the fish reach the target
        if (other.gameObject.Equals(target))
        {
            if (fishManager.RequestEat(gameObject, target))
            {
                //set the other fish to be dead so it doesnt try to do anything else
                if (target.name.StartsWith("Fish"))
                    target.GetComponent<Fish>().state = State.Dead;

                DoEat();
                fishManager.RequestToRemove(target, true);
            }
        }
    }

    //function called by water to let the fish know if it is in the water or not
    public void InWater(bool isIn)
    {
        if (isIn)
        {
            //set variables to simulate being in water
            _cachedRigidBody.drag = 20;
            _cachedRigidBody.angularDrag = 5;
            _cachedRigidBody.useGravity = false;

            inWater = true;
            wanderLoc = new Vector3(Random.Range(wanderArea1.x, wanderArea2.x), Random.Range(wanderArea1.y, wanderArea2.y), Random.Range(wanderArea1.z, wanderArea2.z));
        }
        else
        {
            //reset variables to normal gravity
            _cachedRigidBody.drag = 0;
            _cachedRigidBody.angularDrag = .05f;
            _cachedRigidBody.useGravity = true;

            inWater = false;
        }
    }

    //the happy behavior of a fish, wander to the wander locations
    void BehaveHappy()
    {
        //move forwards while restricting speed
        _cachedTransform.Translate(Vector3.forward * Time.deltaTime * happySpeed);

        //smoothly turn towards the wander location
        if (_cachedTransform.rotation != Quaternion.LookRotation(wanderLoc - _cachedTransform.position))
        {
            _cachedTransform.rotation = Quaternion.Slerp(
                _cachedTransform.rotation,
                Quaternion.LookRotation(wanderLoc - _cachedTransform.position),
                Time.deltaTime
                );
        }

        //change locaiton randomly or if reached the target locaiton
        if (_cachedTransform.position.Equals(wanderLoc) || Random.Range(0, 300) == 1)
        {
            wanderLoc = new Vector3(Random.Range(wanderArea1.x, wanderArea2.x), Random.Range(wanderArea1.y, wanderArea2.y), Random.Range(wanderArea1.z, wanderArea2.z));

            //force the fish to choose a point that is not in the middle of the wander area
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
        _cachedTransform.Translate(Vector3.forward * Time.deltaTime * hungrySpeed);

        //smoothly turn towards the wander location
        if (_cachedTransform.rotation != Quaternion.LookRotation(wanderLoc - _cachedTransform.position))
        {
            _cachedTransform.rotation = Quaternion.Slerp(
                _cachedTransform.rotation,
                Quaternion.LookRotation(wanderLoc - _cachedTransform.position),
                Time.deltaTime
                );
        }

        //change locaiton randomly or if reached the target locaiton
        if (_cachedTransform.position.Equals(wanderLoc) || Random.Range(0, 300) == 1)
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
            if (target.GetComponent<HyperColliderManager>().w == myHyper.w && target.GetComponent<Fish>().size <= size)
            {
                //move forwards while restricting speed
                _cachedTransform.Translate(Vector3.forward * Time.deltaTime * huntingSpeed);

                //smoothly turn towards the target location
                if (_cachedTransform.rotation != Quaternion.LookRotation(target.transform.position - _cachedTransform.position))
                {
                    _cachedTransform.rotation = Quaternion.Slerp(
                        _cachedTransform.rotation,
                        Quaternion.LookRotation(target.transform.position - _cachedTransform.position),
                        Time.deltaTime * 10
                        );
                }
            }
            else {//look for a new target
                target = fishManager.RequestTarget(gameObject, false);
            }
        }
        else {//fish does not have a target
            //are there valid targets?
            if (!noTargets)
            {
                target = fishManager.RequestTarget(gameObject, false);
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
            if (target.GetComponent<HyperColliderManager>().w == myHyper.w)
            {
                //slowly approach the target until close enough
                if (Vector3.Distance(_cachedTransform.position, target.transform.position) < .5f)
                {
                    _cachedTransform.Translate(Vector3.forward * Time.deltaTime * happySpeed);

                    //smoothly turn towards the target location
                    if (_cachedTransform.rotation != Quaternion.LookRotation(target.transform.position - _cachedTransform.position))
                    {
                        _cachedTransform.rotation = Quaternion.Slerp(
                            _cachedTransform.rotation,
                            Quaternion.LookRotation(target.transform.position - _cachedTransform.position),
                            Time.deltaTime * 10
                            );
                    }
                }
                else
                {
                    _cachedTransform.Translate(Vector3.forward * Time.deltaTime * hungrySpeed * 2);

                    //smoothly turn towards the target location
                    if (_cachedTransform.rotation != Quaternion.LookRotation(target.transform.position - _cachedTransform.position))
                    {
                        _cachedTransform.rotation = Quaternion.Slerp(
                            _cachedTransform.rotation,
                            Quaternion.LookRotation(target.transform.position - _cachedTransform.position),
                            Time.deltaTime
                            );
                    }
                }
            }
            else {//look for a new target, can change from fish to food
                target = fishManager.RequestTarget(gameObject, true);
            }
        }
        else {//fish does not have a target
            //are there valid targets?
            if (food)
            {
                target = fishManager.RequestTarget(gameObject, true);
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
            foreach (Transform child in _cachedTransform)
            {
                child.GetComponent<HyperObject>().dullCoef = 2;
                child.GetComponent<HyperObject>().WMove();
            }

        }
        else if (state == State.Dead)
            fishManager.RequestToRemove(gameObject, true);
    }

    //this fish has eaten so become happy, restart the feed cycle and reset color
    void DoEat()
    {
        state = State.Happy;
        foreach (Transform child in _cachedTransform)
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
                _cachedTransform.localScale = new Vector3(minSize, minSize, minSize);
            }
            else if (size == Size.Medium)
            {
                _cachedTransform.localScale = new Vector3(maxSize - (maxSize - minSize)/2, maxSize - (maxSize - minSize) / 2, maxSize - (maxSize - minSize) / 2);
            }
            else if (size == Size.Large)
            {
                _cachedTransform.localScale = new Vector3(maxSize, maxSize, maxSize);
            }
            else
            {
                Debug.Log("NOT A SIZE");
            }
        }
    }
}
