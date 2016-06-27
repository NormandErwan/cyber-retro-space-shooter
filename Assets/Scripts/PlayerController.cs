using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public WeaponController weapon;
	public EngineController engine;

	void Update () {
		WeaponFire ();
	}

	void FixedUpdate () {
		Move ();
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

	void Move () {
		engine.Move ();
	}
}
