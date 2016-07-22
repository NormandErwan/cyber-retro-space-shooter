using UnityEngine;
using System.Collections;

public class UpperWorldBorders : MonoBehaviour {

	public WorldBorders worldBorders;
	public LayerMask constrainedLayers;

	Vector3 bordersMin, bordersMax;

	void Start () {
		ConfigurateBorders ();
	}

	/*
	 * Adjust the borders in the editor in order to visualize them.
	 */
	void OnDrawGizmos () {
		ConfigurateBorders ();
	}

	/*
	 * Set the world borders following the position and the scale of the object.
	 */
	void ConfigurateBorders () {
		bordersMin = - transform.localScale / 2 + transform.position;
		bordersMax = transform.localScale / 2 + transform.position;

		transform.localScale = worldBorders.transform.localScale * worldBorders.borderMarginsPercentage;
	}

	/*
	 * Translate every constrained object, or try to kill it if it has life points, or destroy it.
	 */
	void OnTriggerExit (Collider other) {
		if (Utilities.IsInLayerMask (other.gameObject.layer, constrainedLayers)) {
			WorldBorders.TranslateToOtherSide (transform, other.gameObject, bordersMin, bordersMax);
		} else if (other.gameObject.GetComponent<LifeShieldManager> ()) {
			other.gameObject.GetComponent<LifeShieldManager> ().Die ();
		} else {
			Destroy (other.gameObject);
		}
	}
}
