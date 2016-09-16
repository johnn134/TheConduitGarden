using UnityEngine;
using System.Collections;

public class PreLoadPrefabs : MonoBehaviour {

    public string[] prefabPaths = new string[] { "" };

    void Awake()
    {
        foreach(string prefabPath in prefabPaths)
        {
            Resources.Load(prefabPath);
        }
    }
}
