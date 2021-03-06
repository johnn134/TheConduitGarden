﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FishManager : MonoBehaviour {

    //list of all fish in the world
    List<GameObject> allFish;

    //list of all food in the world
    List<GameObject> allFood;

    //max number of fish allowed in the world
    public int maxFish = 30;

    //max number of food allowed on each w point
    public int maxFood = 30;

    //number of fish in the world
    int numFish = 0;

    //number of food on each w point
    int[] numFood = new int[] { 0, 0, 0, 0, 0 };

    //the fish shrine in the world
    FishShrine fishShrine;

    public static FishManager instance = null;

    void Awake()
    {
        //declare as singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start()
    {
        allFish = new List<GameObject>();
        allFood = new List<GameObject>();

        fishShrine = Object.FindObjectOfType<FishShrine>();
    }

    /*
     * An object has requested to make a fish (or food if isFood is true)
     * at position with rotation
     * return true if successful
     */
    public bool MakeFish(Vector3 nPosition, Quaternion nRotation, int nW, bool isFood)
    {
        GameObject nObj;

        //deturmine if this new object is fish or food
        if (isFood)
        {
            //check to make sure the w point the food is being added on isnt full
            if (numFood[nW] < maxFood)
            {
				nObj = (GameObject)Instantiate(Resources.Load("Food/Food"), nPosition, nRotation);
				nObj.GetComponent<HyperObject>().setW(nW);
                allFood.Add(nObj);
                numFood[nW] += 1;

                return true;
            }
            return false;
        }
        else
        {
            //check to make sure there are not too many fish in the world
            if (numFish < maxFish)
            {
				nObj = (GameObject)Instantiate(Resources.Load("Fish/Fish"), nPosition, nRotation);
                nObj.GetComponent<HyperColliderManager>().setW(nW);
                allFish.Add(nObj);
                numFish += 1;
                alertNewTargets(nW, (int)nObj.GetComponent<Fish>().size);

                //if there is food already in the world and on the new fish's w, let the new fish know
                if (numFood[nObj.GetComponent<HyperColliderManager>().w] > 0)
                    nObj.GetComponent<Fish>().food = true;

                if((int)nObj.GetComponent<Fish>().size == 2)
                    fishShrine.processFish(allFish);

                return true;
            }
            return false;
        }
    }

    //a fish has requested to be removed from the world, return true if successful (also works for fish food)
    public bool RequestToRemove(GameObject rObj, bool alertShrine)
    {
        //deturmine if this new object is fish or food
        if (rObj.name.StartsWith("Food"))
        {
            if (allFood.Remove(rObj))
            {
                numFood[rObj.GetComponent<HyperObject>().w] -= 1;

                //if no food is left then alert the fish that is none
                if(numFood[rObj.GetComponent<HyperObject>().w] == 0)
                    alertFood(rObj.GetComponent<HyperObject>().w, false);

                Destroy(rObj);

                if (alertShrine)
                    fishShrine.processFish(allFish);

                return true;
            }
            else
                return false;
        }
        else
        {
            if (allFish.Remove(rObj))
            {
                numFish -= 1;
                Destroy(rObj);

                if (alertShrine)
                    fishShrine.processFish(allFish);

                return true;
            }
            else
                return false;
        }
    }

    /* 
     * Returns a target gameobject to the requesting fish, 
     * second argument is for if looking for food or not(looking for fish)
     * if no valid target then tell the fish it has no targets so it wont request again
     */
    public GameObject RequestTarget(GameObject rfish, bool isFood)
    {
        //make sure fish isnt dead
        if ((int)rfish.GetComponent<Fish>().state != 3)
        {
            //looking for food?
            if (isFood)
            {
                //closest food and distance
                GameObject closest = null;
                float distance = Mathf.Infinity;

                //suspect each food to find the closest target
                foreach (GameObject suspect in allFood)
                {
                    Vector3 diff = suspect.transform.position - rfish.transform.position;
                    float curDistance = diff.sqrMagnitude;
                    //fish must be the closest fish on the same w and smaller or the same size as the requesting fish
                    if (suspect.GetComponent<HyperColliderManager>().w == rfish.GetComponent<HyperColliderManager>().w)
                    {
                        //find the closest target
                        if (curDistance < distance)
                        {
                            closest = suspect;
                            distance = curDistance;
                        }
                    }
                }
                if (closest)
                    return closest;
                else
                {
                    //No valid targets, return null and and tell the fish 
					//there are no targets so they stop requesting
                    rfish.GetComponent<Fish>().food = false;
                    return null;
                }
            }
            else
            {
                //closest fish and distance in each size
                GameObject closestS = null;
                GameObject closestM = null;
                GameObject closestL = null;
                float distanceS = Mathf.Infinity;
                float distanceM = Mathf.Infinity;
                float distanceL = Mathf.Infinity;

                //suspect each fish to find the closest valid targets
                foreach (GameObject suspect in allFish)
                {
                    Vector3 diff = suspect.transform.position - rfish.transform.position;
                    float curDistance = diff.sqrMagnitude;
                    //fish must be the closest fish on the same w and smaller or the same size as the requesting fish
                    if (suspect.GetComponent<Fish>().size <= rfish.GetComponent<Fish>().size
						&& suspect.GetComponent<HyperColliderManager>().w == rfish.GetComponent<HyperColliderManager>().w
						&& !suspect.gameObject.Equals(rfish))
                    {
                        //find the closest target for each size
                        if ((int)suspect.GetComponent<Fish>().size == 0)//small
                        {
                            if (curDistance < distanceS)
                            {
                                closestS = suspect;
                                distanceS = curDistance;
                            }
                        }
                        else if ((int)suspect.GetComponent<Fish>().size == 1)//medium
                        {
                            if (curDistance < distanceM)
                            {
                                closestM = suspect;
                                distanceM = curDistance;
                            }
                        }
                        else if ((int)suspect.GetComponent<Fish>().size == 2)//large
                        {
                            if (curDistance < distanceL)
                            {
                                closestL = suspect;
                                distanceL = curDistance;
                            }
                        }
                    }
                }
                if (closestS)
                    return closestS;
                else if (closestM && (int)rfish.GetComponent<Fish>().size >= 1)
                    return closestM;
                else if (closestL && (int)rfish.GetComponent<Fish>().size >= 2)
                    return closestL;
                else
                {
                    //no valid targets, return null and and tell the fish there are no targets so they stop requesting
                    rfish.GetComponent<Fish>().noTargets = true;
                    return null;
                }
            }
        }
        else
            return null; //dead
    }

    //returns true if requesting fish can successfully eat the target
    public bool RequestEat(GameObject rfish, GameObject target)
    {
        //make sure the fish isnt dead
        if ((int)rfish.GetComponent<Fish>().state != 3)
        {
            //check to see if the target is a fish or food
            if (target.name.StartsWith("Food"))
            {
                return true;
            }
            else
            {
                //make sure the target is still valid
                if (target.GetComponent<HyperColliderManager>().w == rfish.GetComponent<HyperColliderManager>().w
					&& target.GetComponent<Fish>().size <= rfish.GetComponent<Fish>().size)
                {
                    //check to see if there is a tie in size, only if the target is also hunting this fish
                    if (target.GetComponent<Fish>().size == rfish.GetComponent<Fish>().size
						&& (int)target.GetComponent<Fish>().state == 2
						&& target.GetComponent<Fish>().target.Equals(rfish))
                    {
                        //eat the other fish if older than it
                        if (isOlder(rfish, target))
                            return true;
                        else
                            return false;
                    }
                    else
                        return true; //no tie
                }
                else
                    return false; //not valid
            }
        }
        else
            return false; //this fish is dead
    }

    /*
     * Returns true if the first fish game object is older
     * than the second using their indecies in the allFish list
     */
    bool isOlder(GameObject fish1, GameObject fish2)
    {
        return (allFish.IndexOf(fish1) < allFish.IndexOf(fish2));
    }

    /*
     * Alert all fish on a w point that there is a fish that it can eat
     */
    void alertNewTargets(int alertW, int nSize)
    {
		foreach(GameObject fish in allFish)
			if(fish.GetComponent<HyperColliderManager>().w == alertW
			             && (int)fish.GetComponent<Fish>().size >= nSize) {
				fish.GetComponent<Fish>().noTargets = false;
			}
    }

    /*
     * Alert all fish on a w point if there is or is not food it can eat
     */
    public void alertFood(int alertW, bool isFood)
    {
        foreach (GameObject fish in allFish)
            if (fish.GetComponent<HyperColliderManager>().w == alertW)
                fish.GetComponent<Fish>().food = isFood;
    }

    /* 
     * Alert the manager and other affected fish that this fish is moving to newW
     */
    public void alertMove(GameObject rfish, int newW)
    {
        foreach (GameObject fish in allFish)
            if (fish.GetComponent<HyperColliderManager>().w == newW
				&& (int)fish.GetComponent<Fish>().size >= (int)rfish.GetComponent<Fish>().size)
                fish.GetComponent<Fish>().noTargets = false;

        fishShrine.processFish(allFish);
    }
}
