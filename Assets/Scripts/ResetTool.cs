using UnityEngine;
using System.Collections;

public class ResetTool : MonoBehaviour {

    public GameObject target;

    void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.tag.Equals("Tool") && !other.transform.parent)
        {
			StartCoroutine (MoveToolToTargetDelayed (other.gameObject));
        }
    }

	void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag.Equals("Tool") && !other.transform.parent && other.GetComponent<Rigidbody>().velocity.magnitude < 1f)
		{
			MoveToolToTarget (other.gameObject);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag.Equals("Tool") && !other.transform.parent)
		{
			StartCoroutine (MoveToolToTargetDelayed (other.gameObject));
		}
	}

	IEnumerator MoveToolToTargetDelayed(GameObject tool)
	{
		yield return new WaitForSeconds (3f);

		tool.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + .5f, target.transform.position.z);
		tool.GetComponent<Rigidbody>().velocity = Vector3.zero;
		tool.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
	}

	void MoveToolToTarget(GameObject tool)
	{
		tool.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + .5f, target.transform.position.z);
		tool.GetComponent<Rigidbody>().velocity = Vector3.zero;
		tool.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
	}
}
