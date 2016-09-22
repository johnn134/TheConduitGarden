using UnityEngine;
using System.Collections;

public class PreLoadPrefabs : MonoBehaviour {

    public string[] prefabPaths = new string[] { "" };

    HyperCreature player;

    public float fadeInSpeed = .005f;

    void Awake()
    {
        foreach(string prefabPath in prefabPaths)
        {
            Resources.Load(prefabPath);
        }

        player = HyperCreature.instance;
    }

    void Update()
    {
        if (player.FadeInTransitionStep(fadeInSpeed))
        {
            Destroy(gameObject);
        }
    }
}
