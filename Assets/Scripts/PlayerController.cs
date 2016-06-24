using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public WeaponController weapon = null;
	public float weaponFireRate = 0.1f;

	private float nextFire;

	void Start () {
		nextFire = weaponFireRate;
	}

	void Update () {
		WeaponFire ();
	}

	/*
	 * Fire with the weapon when the user requests it.
	 */
	void WeaponFire () {
		if (Input.GetKeyDown (KeyCode.Space) // Discrete fire
			|| (Input.GetKey (KeyCode.Space) && Time.time > nextFire)) // Continuous fire
		{
			weapon.Fire ();
			nextFire = Time.time + weaponFireRate;
		}
	}
}
