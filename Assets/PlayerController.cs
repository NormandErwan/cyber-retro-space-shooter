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
		// Discrete or continuous fire
		if (Input.GetKeyDown(KeyCode.Space) || (Input.GetKey(KeyCode.Space) && Time.time > nextFire)) {
			weapon.Fire();
			nextFire = Time.time + weaponFireRate;
		}
	}
}
