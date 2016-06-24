using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public GameObject munition;

	/**
	 * Instantiate with a force a munition in the direction of the weapon. 
	 */
	public void Fire () {
		GameObject concreteMunition = Instantiate (munition, transform.position, transform.rotation) as GameObject;

		Vector3 munitionFireForce = transform.forward * concreteMunition.GetComponent<MunitionController> ().fireForce;
		concreteMunition.GetComponent<Rigidbody> ().AddForce (munitionFireForce);
	}
}
