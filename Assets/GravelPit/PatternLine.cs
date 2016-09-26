using UnityEngine;
using System.Collections;

public class PatternLine : MonoBehaviour {
	public GameObject patternStartNode;
	public GameObject patternEndNode;
	public Vector3 patternVectorFromStart;
	public Vector3 patternVectorFromEnd;
	public bool isVertical;
	public bool wasChecked;
	public int patternLineNumber;
	public int patternDimension;
}
