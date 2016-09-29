using UnityEngine;
using System.Collections;

public class PatternLine : MonoBehaviour {
	public GameObject patternStartNode;
	public GameObject patternEndNode;
	public Vector3 patternVector;
	public bool isVertical;
	public int patternDimension;

	void Awake(){
		patternStartNode = transform.GetChild (0).gameObject;
		patternEndNode = transform.GetChild (1).gameObject;
	}
}
