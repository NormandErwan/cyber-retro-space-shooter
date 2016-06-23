using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour {

	public GameObject munition;

	public float munitionFireForce = 500f;

	public void Fire () {
		GameObject concreteMunition = Instantiate (munition);
		concreteMunition.transform.position = transform.position;
		concreteMunition.GetComponent<Rigidbody> ().AddForce (transform.forward * munitionFireForce);
	}
}
