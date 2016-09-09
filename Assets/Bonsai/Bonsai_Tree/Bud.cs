using UnityEngine;
using System.Collections;

public class Bud : MonoBehaviour {

	GameObject manager;

	int w;

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
			//newLeaf.GetComponent<Leaf>().setDepth(depth);
			newLeaf.GetComponent<Leaf>().setManager(manager);
			newLeaf.GetComponent<Leaf>().checkIfLeafSatisfiesContract();

			//Set w position
			newLeaf.GetComponent<HyperColliderManager>().setW(transform.GetChild(0).GetComponent<HyperObject>().w);

			//Register Leaf Added
			transform.parent.GetComponent<Branch>().registerLeafAdded();
		}
		else {
			GameObject newBranch = Instantiate(Resources.Load("Bonsai/BranchPrefab"), Vector3.zero, Quaternion.identity, transform.parent) as GameObject;
			newBranch.transform.localPosition = transform.localPosition;
			newBranch.transform.localRotation = transform.localRotation;
			//newBranch.transform.Rotate(-90, 0, 0);

			//Initialize Variables
			newBranch.GetComponent<Branch>().setDepth(depth);
			newBranch.GetComponent<Branch>().setManager(manager);
			newBranch.GetComponent<Branch>().checkIfBranchSatisfiesContract();

			//Set w position
			newBranch.GetComponent<HyperColliderManager>().setW(transform.GetChild(0).GetComponent<HyperObject>().w);

			//Register Branch Added
			transform.parent.GetComponent<Branch>().registerBranchAdded();
		}
		didGrow = true;

		Destroy(this.gameObject);
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
	 * Sets the w position of this bud and adjusts the color accordingly
	 */
	public void setWPosition(int newW) {
		w = newW;

		//Change Material value
		switch(w) {
			case 0:     //red
				setVisualColor(new Color(1.0f, 0.0f, 0.0f, 0.5f));
				break;
			case 1:     //orange
				setVisualColor(new Color(1.0f, 0.5f, 0.0f, 0.5f));
				break;
			case 2:     //yellow
				setVisualColor(new Color(1.0f, 1.0f, 0.0f, 0.5f));
				break;
			case 3:     //green
				setVisualColor(new Color(0.0f, 1.0f, 0.0f, 0.5f));
				break;
			case 4:     //blue
				setVisualColor(new Color(0.0f, 1.0f, 1.0f, 0.5f));
				break;
			case 5:     //indigo
				setVisualColor(new Color(0.0f, 0.0f, 1.0f, 0.5f));
				break;
			case 6:     //violet
				setVisualColor(new Color(1.0f, 0.0f, 1.0f, 0.5f));
				break;
		}
	}

	/*
	 * Changes the material color of the visual components of this bud
	 */
	void setVisualColor(Color c) {
		transform.GetChild(0).GetComponent<MeshRenderer>().material.color = c;
	}

	/*
	 * Sets the bonsai manager this bud answers to
	 */
	public void setManager(GameObject newManager) {
		manager = newManager;
	}
}
