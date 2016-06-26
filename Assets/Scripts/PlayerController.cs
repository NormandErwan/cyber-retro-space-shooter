using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public WeaponController weapon = null;

	void Update () {
		WeaponFire ();
	}

	/*
	 * Fire with the weapon when the user requests it.
	 */
	void WeaponFire () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			weapon.DiscreteFire ();
		}
		else if (Input.GetKey (KeyCode.Space)) {
			weapon.ContinuousFire ();
		}
	}
}
