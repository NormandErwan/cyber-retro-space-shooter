using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public GameObject munition;

	public float munitionFireForce = 500f;

	/**
	 * Instantiate with a force a munition in the direction of the weapon. 
	 */
	public void Fire () {
		GameObject concreteMunition = Instantiate (munition);
		concreteMunition.transform.position = transform.position;
		concreteMunition.transform.rotation = transform.rotation;
		concreteMunition.GetComponent<Rigidbody> ().AddForce (transform.forward * munitionFireForce);
	}
}
