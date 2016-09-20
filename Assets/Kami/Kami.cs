﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR;

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

    public Type type;                           //the type of objects the kami responds to
    public State state;                         //the state that dictates how the kami acts

    public GameObject target;                   //The object the kami it trying to land on
    public GameObject holding;                  //the object the kami is holding

    public List<GameObject> targets;            //list of possible targets the kami can land on

    public HyperCreature player;                //the hyper creature in the scene

    public bool helping;                        //is the kami currently helping *Not currently used*

    HyperObject myHyper;                        //reference to the hyper code on this kami

    public int id;                              //unique id number for this kami

    float leavingY = -.5f;                       //the y that the kami should be at for the ending behavior

    Vector3 wanderLoc;                          //the current target location the kami is going to

    public Vector3 standbyLoc;                  //the place where kami should wait *Not currently used*

    Renderer _cachedRenderer;						//The renderer for this object

    ParticleSystem _cachedParticleSystem;           //the particle system on this object

    Light _cachedLight;                             //the light on this object

    SteamVR_ControllerManager controllerManager;    //The steam controller manager that holds the controller indices

    KamiManager kamiManager;                    //reference to the kami manager

    void Awake()
    {
        _cachedRenderer = GetComponent<Renderer>();

        _cachedParticleSystem = GetComponent<ParticleSystem>();

        _cachedLight = GetComponent<Light>();

        controllerManager = SteamVR_ControllerManager.instance;
    }

    void Start()
    {
        player = HyperCreature.instance;
        targets = new List<GameObject>();
        myHyper = GetComponent<HyperObject>();
        wanderLoc = new Vector3(Random.Range(-6, 7), 3, Random.Range(-6, 7));
        kamiManager = KamiManager.instance;

        //get targets depending on type
        //NOTE: good format is to always have the type shrine at index 0
        if (type == Type.Fish)
        {
            targets.Add(GameObject.Find("ShrineFish/Visual/TopSphere"));
        }

        StartCoroutine(ColorTrans());
    }

    void Update()
    {
        if(state == State.Happy)
        {
            if (target)
                LandOnTarget();
            else
                BehaveHappy();

            if(Random.Range(0,500) == 1)
                if (myHyper.SlideW(Random.Range(-1, 2)))
                {
                    myHyper.WMove();
                    StartCoroutine(ColorTrans());
                }
        }
        else if(state == State.Flee)
        {
            Flee();
        }
        else if(state == State.Ending)
        {
            BehaveEnding();
        }
    }

    void LateUpdate()
    {
        for (int i = 0; i < controllerManager.indices.Length; i++)
        {
            if (controllerManager.indices[i] != OpenVR.k_unTrackedDeviceIndexInvalid)
            {
                if (SteamVR_Controller.Input((int)controllerManager.indices[i]).GetPressDown(EVRButtonId.k_EButton_SteamVR_Trigger))
                {
                    StartCoroutine(ColorTrans());
                }
            }
        }
    }

    /*void FixedUpdate()
    {
        if (_cachedRenderer.material.color.a < .5f)
            _cachedLight.color = Color.black;
        else
            _cachedLight.color = _cachedRenderer.material.color;

        _cachedParticleSystem.startColor = _cachedRenderer.material.color;
    }*/

    //smoothly change the color of this object, rmove once 4D shader is implemented
    IEnumerator ColorTrans()
    {
        Color targetColor;

        //deturmine the target color based on w point
        if (myHyper.w == 0)
            targetColor = Color.red;
        else if (myHyper.w == 1)
            targetColor = new Color(1, .45f, 0);
        else if (myHyper.w == 2)
            targetColor = Color.yellow;
        else if (myHyper.w == 3)
            targetColor = Color.green;
        else if (myHyper.w == 4)
            targetColor = Color.cyan;
        else if (myHyper.w == 5)
            targetColor = Color.blue;
        else
            targetColor = Color.magenta;

        targetColor.a = _cachedRenderer.material.color.a;

        for (float i = 0.0f; i <= 1.0f; i += .1f)
        {

            _cachedParticleSystem.startColor = Color.Lerp(_cachedParticleSystem.startColor, targetColor, .1f);

            if (myHyper.w != player.w)
                _cachedLight.color = Color.Lerp(_cachedLight.color, Color.black, .1f);
            else
                _cachedLight.color = Color.Lerp(_cachedLight.color, targetColor, .1f);

            yield return null;
        }

        if (myHyper.w != player.w)
            _cachedLight.color = Color.black;
        else
            _cachedLight.color = _cachedRenderer.material.color;

        _cachedParticleSystem.startColor = _cachedRenderer.material.color;
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
            if (Random.Range(0, 100) == 1)
                target = targets[Random.Range(0, targets.Count)];
            else
                wanderLoc = new Vector3(Random.Range(kamiManager.wanderArea1.x, kamiManager.wanderArea2.x), Random.Range(kamiManager.wanderArea1.y, kamiManager.wanderArea2.y), Random.Range(kamiManager.wanderArea1.z, kamiManager.wanderArea2.z));
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

    void BehaveEnding()
    {
        wanderLoc = new Vector3(player.transform.Find("Camera (eye)").position.x, leavingY, player.transform.Find("Camera (eye)").position.z);

        if (Vector3.Distance(transform.position, wanderLoc) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, wanderLoc, Time.deltaTime * 2);
        }
        else
            transform.RotateAround(wanderLoc, Vector3.up, Time.deltaTime * 200);

        if (transform.position.y != wanderLoc.y)
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, wanderLoc.y, transform.position.z), Time.deltaTime / 2);

        leavingY += .01f;

        if (transform.position.y > 20f)
        {
            kamiManager.RequestToRemove(gameObject);
        }
    }
}
