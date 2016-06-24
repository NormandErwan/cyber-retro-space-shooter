using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public GameObject munition;

	/**
	 * Instantiate with a force a munition in the direction of the weapon. 
	 */
	public void Fire () {
		GameObject concreteMunition = Instantiate (munition);
		concreteMunition.transform.position = transform.position;
		concreteMunition.transform.rotation = transform.rotation;

		float munitionFireForce = concreteMunition.GetComponent<MunitionController> ().fireForce;
		concreteMunition.GetComponent<Rigidbody> ().AddForce (transform.forward * munitionFireForce);
	}
}
