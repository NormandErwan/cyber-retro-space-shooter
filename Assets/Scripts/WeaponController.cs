using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public GameObject munition;
	public float fireRate = 0.1f;

	private float nextFire = 0f;

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
		GameObject concreteMunition = Instantiate (munition, transform.position, transform.rotation) as GameObject;

		Vector3 munitionFireForce = transform.forward * concreteMunition.GetComponent<MunitionController> ().fireVelocity;
		concreteMunition.GetComponent<Rigidbody> ().AddForce (munitionFireForce, ForceMode.VelocityChange);
	}
}
