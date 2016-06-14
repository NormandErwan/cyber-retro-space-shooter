using UnityEngine;
using System.Collections;

public class WorldBorders : MonoBehaviour {

	private Vector3 bordersMin, bordersMax;

	void Start () {
		bordersMin = - transform.localScale / 2 + transform.position;
		bordersMax = transform.localScale / 2 + transform.position;
	}

	/*
	 * Constraint every object visible by the player in the box, by translating objects at the other side when they 
	 * exit the box.
	 */
	void OnTriggerExit(Collider other) {
		if (other.tag == "FollowWorldBorders") {
			Vector3 translation = new Vector3 (0, 0, 0);
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
}
