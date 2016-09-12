using UnityEngine;
using System.Collections;

public class ResetTool : MonoBehaviour {

    public GameObject target;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Tool"))
        {
            other.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z);
            other.GetComponent<Rigidbody>().velocity = Vector3.zero;
            other.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        }
    }
}
