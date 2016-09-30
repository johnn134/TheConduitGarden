using UnityEngine;
using System.Collections;

public class PatternLine : MonoBehaviour {
	public HyperCreature player;
	public GameObject rake;
	public GameObject patternStartNode;
	public GameObject patternEndNode;
	public Vector3 patternVector;
	public bool isVertical;
	public int patternDimension;

	void Awake() {
		player = HyperCreature.instance;
		player.w = 1;
		patternStartNode = transform.GetChild (0).gameObject;
		patternEndNode = transform.GetChild (1).gameObject;

		GetComponent<LineRenderer>().SetPosition(0, transform.GetChild(0).transform.position);
		GetComponent<LineRenderer>().SetPosition(1, transform.GetChild(1).transform.position);
		GetComponent<LineRenderer>().SetWidth(0.01f, 0.01f);
		Color lineColor = Color.red;
		switch(patternDimension) {
			case 0:
				lineColor = new Color(255, 0, 0);
				break;
			case 1:
				lineColor = new Color(251, 255, 0);
				break;
			case 2:
				lineColor = new Color(0, 255, 12);
				break;
			case 3:
				lineColor = new Color(0, 234, 255);
				break;
			case 4:
				lineColor = new Color(255, 0, 255);
				break;
			default:
				lineColor = Color.red;
				break;
		}
		GetComponent<LineRenderer>().SetColors(lineColor, lineColor);
		GetComponent<LineRenderer>().enabled = false;
	}

	void Update() {
		if (rake.transform.parent && patternDimension == player.w && GetComponent<LineRenderer>().enabled == false) {
			GetComponent<LineRenderer>().enabled = true;
		}

		else if (patternDimension != player.w || rake.transform.parent == null) {
			GetComponent<LineRenderer>().enabled = false;
		}
	}

}
