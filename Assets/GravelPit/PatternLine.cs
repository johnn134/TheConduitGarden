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
	private Color lineColor;

	void Awake() {
		player = HyperCreature.instance;
		patternStartNode = transform.GetChild (0).gameObject;
		patternEndNode = transform.GetChild (1).gameObject;

		rake = GameObject.Find("Rake");
		GetComponent<LineRenderer>().SetPosition(0, transform.GetChild(0).transform.position);
		GetComponent<LineRenderer>().SetPosition(1, transform.GetChild(1).transform.position);
		GetComponent<LineRenderer>().SetWidth(0.01f, 0.01f);
		switch(patternDimension) {
			case 0:
				lineColor = new Color(1.0f, 0.0f, 0.0f);
				break;
			case 1:
				lineColor = new Color(1.0f, 1.0f, 0.0f);
				break;
			case 2:
				lineColor = new Color(0.0f, 1.0f, 0.0f);
				break;
			case 3:
				lineColor = new Color(0.0f, 1.0f, 1.0f);
				break;
			case 4:
				lineColor = new Color(1.0f, 0.0f, 1.0f);
				break;
			default:
				lineColor = Color.red;
				break;
		}
		GetComponent<LineRenderer>().SetColors(lineColor, lineColor);
		GetComponent<LineRenderer>().SetColors(Color.clear, Color.clear);
	}

	void Update() {
		if (patternDimension == player.w) {
			GetComponent<LineRenderer>().SetColors(lineColor, lineColor);
		}

		else if (patternDimension != player.w) {
			GetComponent<LineRenderer>().SetColors(Color.clear, Color.clear);
		}
	}

}
