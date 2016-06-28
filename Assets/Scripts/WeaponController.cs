using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponController : MonoBehaviour {

	public List<GameObject> fireSpawns;
	public GameObject munition;
	public float fireRate = 0.1f;

	private float nextFire = 0f;
	private int nextFireSpawn = 0;

	/*
	 * Fire considering the weapon fire rate.
	 */
	public void ContinuousFire () {
		if (Time.time > nextFire) {
			Fire ();
			nextFire = Time.time + fireRate;
		}
	}

	/*
	 * Fire immediatly without taking account of the weapon fire rate.
	 */
	public void DiscreteFire () {
		Fire ();
		nextFire = Time.time + fireRate;
	}

	/**
	 * Instantiate with a force a munition in the direction of the weapon. 
	 */
	void Fire () {
		Transform fireSpawn = fireSpawns [nextFireSpawn].transform;
		GameObject concreteMunition = Instantiate (munition, fireSpawn.position, fireSpawn.rotation) as GameObject;

		Vector3 munitionFireForce = transform.forward * concreteMunition.GetComponent<MunitionController> ().fireVelocity;
		concreteMunition.GetComponent<Rigidbody> ().AddForce (munitionFireForce, ForceMode.VelocityChange);

		nextFireSpawn = ++nextFireSpawn % fireSpawns.Count;
	}
}
