using UnityEngine;
using System.Collections;

public class MunitionController : MonoBehaviour {

	public float damageFactor = 1f;
	public float fireVelocity = 5f;
	public float destroyBelowVelocity = 1f;

	void FixedUpdate () {
		DestroyBelowVelocity ();
	}

	/*
	 * Destroy the munition if it's velocity is below the threshold 'destroyBelowVelocity'.
	 */
	void DestroyBelowVelocity () {
		float velocityMagnitude = gameObject.GetComponent<Rigidbody> ().velocity.magnitude;
		if (velocityMagnitude < destroyBelowVelocity 
			&& velocityMagnitude != 0) { // Don't destroy it at it's creation, before the force has been applied by the weapon
			//Destroy (gameObject);
		}
		Debug.Log (velocityMagnitude);
	}

	/*
	 * Hit every object with a LifeController and inflict damages.
	 */
	void OnCollisionEnter (Collision colision) {
		LifeController otherLife = colision.gameObject.GetComponent<LifeController> ();
		if (otherLife != null) {
			otherLife.Hit (damageFactor);
		}
		Destroy (gameObject);
	}
}
