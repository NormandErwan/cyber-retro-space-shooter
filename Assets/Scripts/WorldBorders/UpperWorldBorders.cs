using UnityEngine;
using System.Collections;

public class UpperWorldBorders : MonoBehaviour {

	public WorldBorders worldBorders;
	public LayerMask constrainedLayers;

	Vector3 bordersMin, bordersMax;

	void Start () {
		ConfigurateBorders ();
	}

	void ConfigurateBorders () {
		bordersMin = - transform.localScale / 2 + transform.position;
		bordersMax = transform.localScale / 2 + transform.position;

		transform.localScale = worldBorders.transform.localScale * worldBorders.borderMarginsPercentage;
	}

	void OnTriggerExit (Collider other) {
		if (Utilities.IsInLayerMask (other.gameObject.layer, constrainedLayers)) {
			WorldBorders.TranslateToOtherSide (transform, other.gameObject, bordersMin, bordersMax);
		/*} else if (other.gameObject.GetComponent<LifeController> ()) {
			other.gameObject.GetComponent<LifeController> ().Die ();
		*/} else {
			Destroy (other.gameObject);
		}
	}

	void OnDrawGizmos () {
		ConfigurateBorders ();
	}
}
