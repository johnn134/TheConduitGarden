using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FourthDManager : MonoBehaviour {

    public static int MAX_W = 6;
    public static int MIN_W = 0;

    List<GameObject> allHypColliders;

    public static FourthDManager instance = null;

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
        allHypColliders = new List<GameObject>();
    }

    public List<GameObject> GetList()
    {
        return allHypColliders;
    }

    public void AddToList(GameObject newCollider)
    {
        allHypColliders.Add(newCollider);
    }

    public void RemoveFromList(GameObject rCollider)
    {
        allHypColliders.Remove(rCollider);
    }
}
