using UnityEngine;
using System.Collections;

public class Bud : MonoBehaviour {

	GameObject manager;

	int depth = 0;

	bool isLeaf;        //Tells whether this bud will grow into a leaf or branch
	bool didGrow;

	static int ID = 0;

	// Use this for initialization
	void Start() {
		didGrow = false;

		//name the bud
		this.gameObject.name = "Bud_" + ID;
		ID++;
	}

	// Update is called once per frame
	void Update() {

	}

	void OnDestroy() {
		if(manager != null && !didGrow) {
			if(isLeaf)
				manager.GetComponent<BonsaiManager>().removeLeaf();
			else
				manager.GetComponent<BonsaiManager>().removeBranch();
		}

		if(transform.parent != null) {
			if(transform.parent.GetComponent<Branch>() != null) {
				transform.parent.GetComponent<Branch>().registerBudRemoved();

				if(didGrow)
					transform.parent.GetComponent<Branch>().registerGrowthEnded();
			}
		}
	}

	/*
	 * Grows this bud into either a branch or leaf
	 */
	public void processGrowthCycle() {
		if(isLeaf) {
			GameObject newLeaf = Instantiate(Resources.Load("Bonsai/LeafPrefab"), Vector3.zero, Quaternion.identity, transform.parent) as GameObject;
			//Set transform
			newLeaf.transform.localPosition = transform.localPosition;
			newLeaf.transform.localRotation = transform.localRotation;
			newLeaf.transform.Rotate(-90, 0, 0);

			//Initialize Variables
			newLeaf.GetComponent<Leaf>().setManager(manager);

			//Set w position
			newLeaf.GetComponent<HyperColliderManager>().setW(transform.GetChild(0).GetComponent<HyperObject>().w);

			//Register Leaf Added
			transform.parent.GetComponent<Branch>().registerLeafAdded();

			//Start extension
			newLeaf.GetComponent<Leaf>().initiateExtension();
		}
		else {
			GameObject newBranch = Instantiate(Resources.Load("Bonsai/BranchPrefab"), Vector3.zero, Quaternion.identity, transform.parent) as GameObject;
			newBranch.transform.localPosition = transform.localPosition;
			newBranch.transform.localRotation = transform.localRotation;

			//Initialize Variables
			newBranch.GetComponent<Branch>().initializeBranch(true, depth, manager, 
				transform.GetChild(0).GetComponent<HyperObject>().w);

			//Register Branch Added
			transform.parent.GetComponent<Branch>().registerBranchAdded();

			//Start extension
			newBranch.GetComponent<Branch>().initiateExtension();
		}

		didGrow = true;

		Destroy(this.gameObject);
	}

	public void initiateExtension() {
		StartCoroutine(extendBud());
	}

	public IEnumerator extendBud() {
		Vector3 initialScale = transform.localScale;

		for(float t = 0; t < 1; t += Time.deltaTime) {
			transform.localScale = new Vector3(Mathf.Lerp(0.0f, initialScale.x, t), 
				Mathf.Lerp(0.0f, initialScale.y, t), 
				Mathf.Lerp(0.0f, initialScale.z, t));
			yield return null;
		}
	}

	/*
	 * Sets the type of this bud
	 */
	public void setisLeaf(bool isLeaf) {
		this.isLeaf = isLeaf;
	}

	/*
	 * Sets the depth of this bud on the tree
	 */
	public void setDepth(int newDepth) {
		depth = newDepth;
	}

	/*
	 * Sets the bonsai manager this bud answers to
	 */
	public void setManager(GameObject newManager) {
		manager = newManager;
	}
}
