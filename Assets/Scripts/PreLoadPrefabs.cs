using UnityEngine;
using System.Collections;

public class PreLoadPrefabs : MonoBehaviour {

    public string[] prefabPaths = new string[] { "" };

    HyperCreature player;

    public float fadeInSpeed = .005f;

    public int initialWPerif = 0;

    void Awake()
    {
        foreach(string prefabPath in prefabPaths)
        {
            Resources.Load(prefabPath);
        }

        player = HyperCreature.instance;
        player.w_perif = initialWPerif;
    }

    void Update()
    {
        if (player.FadeInTransitionStep(fadeInSpeed))
        {
            Destroy(gameObject);
        }
    }
}
