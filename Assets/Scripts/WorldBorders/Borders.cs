using UnityEngine;
using System.Collections;

public class Borders : MonoBehaviour {

	public LayerMask constrainedLayers;

	protected Vector3 bordersMin, bordersMax;

	/*
	 * Set the world borders following the position and the scale of the object.
	 */
	public void ConfigurateBorders () {
		bordersMin = - transform.localScale / 2 + transform.position;
		bordersMax = transform.localScale / 2 + transform.position;
	}

	/*
	 * Translate an object to the other side of the borders.
	 */
	protected void TranslateToOtherSide(Transform transform, GameObject other, Vector3 bordersMin, Vector3 bordersMax) {
		Vector3 translation = Vector3.zero;
		Vector3 otherPosition = other.transform.position;

		for (int i = 0; i <= 2; i++) { // For each x,y,z axis
			if (otherPosition [i] > bordersMax [i]) {
				translation [i] -= transform.localScale [i];
			} else if (otherPosition [i] < bordersMin [i]) {
				translation [i] += transform.localScale [i];
			}
		}
		other.transform.Translate (translation, Space.World);
	}
}
