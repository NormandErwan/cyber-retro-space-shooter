using UnityEngine;
using System.Collections;

public class UpperWorldBorders : Borders {

	/*
	 * Translate every object in the constrained layers, or try to kill it if it has life points, or destroy it.
	 */
	void OnTriggerExit (Collider other) {
		if (Utilities.IsInLayerMask (other.gameObject.layer, constrainedLayers)) {
			TranslateToOtherSide (transform, other.gameObject, bordersMin, bordersMax);
		} else if (other.gameObject.GetComponent<LifeShieldManager> ()) {
			other.gameObject.GetComponent<LifeShieldManager> ().Die ();
		} else {
			Destroy (other.gameObject);
		}
	}

	/*
	 * Set the world borders following the position and the scale of the object.
	 */
	public void ConfigurateBorders (Vector3 worldBordersLocalScale, float worldBordersMarginPercentage) {
		transform.localScale = worldBordersLocalScale * worldBordersMarginPercentage;
		base.ConfigurateBorders ();
	}
}
