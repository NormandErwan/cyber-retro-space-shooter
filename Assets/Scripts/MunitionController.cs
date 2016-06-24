using UnityEngine;
using System.Collections;

public class MunitionController : MonoBehaviour {

	public float damageFactor = 1f;
	public float fireForce = 100f;

	/*
	 * Hit every object with a LifeController and inflict damages.
	 */
	void OnCollisionEnter (Collision colision) {
		LifeController otherLife = colision.gameObject.GetComponent<LifeController> ();
		if (otherLife != null) {
			otherLife.Hit (damageFactor);
		}
		Destroy (this.gameObject);
	}
}
