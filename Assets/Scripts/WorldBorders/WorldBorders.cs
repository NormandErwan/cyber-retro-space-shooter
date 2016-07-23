using UnityEngine;
using System.Collections;

public class WorldBorders : Borders {

	public float bordersMarginPercentage;
	public UpperWorldBorders upperWorldBorders;

	private WorldBorderGrid worldBorderGrid;

	void Awake () {
		worldBorderGrid = GetComponent<WorldBorderGrid> ();
	}

	void Start () {
		ConfigurateBorders ();
		upperWorldBorders.ConfigurateBorders (transform.localScale, bordersMarginPercentage);
		//worldBorderGrid.SetupBorders (bordersMarginPercentage);
	}

	/*
	 * Adjust the borders in the editor in order to visualize them.
	 */
	void OnDrawGizmos () {
		upperWorldBorders.ConfigurateBorders (transform.localScale, bordersMarginPercentage);
	}

	/*
	 * Constraint every object visible by the player in the box, by translating objects at the other side when they 
	 * exit the box.
	 */
	void OnTriggerExit (Collider other) {
		if (Utilities.IsInLayerMask (other.gameObject.layer, constrainedLayers)) {
			TranslateToOtherSide (transform, other.gameObject, bordersMin, bordersMax);
		}
	}
}
