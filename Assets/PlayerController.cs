using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public WeaponController weapon = null;
	public float weaponFireRate = 0.1f;

	private float lastFire;

	void Start () {
		lastFire = Time.time;
	}

	void Update () {
		if (Input.GetKeyUp(KeyCode.Space) || (Input.GetKey(KeyCode.Space) && Time.time > lastFire+weaponFireRate)) {
			weapon.Fire();
			lastFire = Time.time + weaponFireRate;
		}
	}
}
