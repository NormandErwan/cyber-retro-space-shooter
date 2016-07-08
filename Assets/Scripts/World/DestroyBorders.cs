using UnityEngine;
using System.Collections;

public class DestroyBorders : MonoBehaviour {

	public WorldBorders worldBorders;

	void Start () {
		SetupBorders ();
	}

	void SetupBorders () {
		BoxCollider boxCollider = GetComponent<BoxCollider> ();
		boxCollider.size = Vector3.one * worldBorders.borderMarginsPercentage;
	}

	void OnTriggerExit (Collider other) {
		Destroy (other.gameObject);
	}

	void OnDrawGizmos () {
		SetupBorders ();
	}
}
