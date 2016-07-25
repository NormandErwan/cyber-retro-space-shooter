using UnityEngine;
using System.Collections;

public class MunitionController : MonoBehaviour {

	public float damageFactor = 1f;
	public float fireVelocity = 5f;
	public float destroyBelowVelocity = 1f;

	public GameObject explosionPrefab;
	public float explosionTime = 1f;

	private bool isExploding = false;

	void FixedUpdate () {
		DestroyBelowVelocity ();
	}

	/*
	 * Destroy the munition if it's velocity is below the threshold 'destroyBelowVelocity'.
	 */
	void DestroyBelowVelocity () {
		float velocityMagnitude = gameObject.GetComponent<Rigidbody> ().velocity.magnitude;
		if (!isExploding && velocityMagnitude < destroyBelowVelocity 
			&& velocityMagnitude != 0) { // Don't destroy it at it's creation, before the force has been applied by the weapon
			Destroy (gameObject);
		}
	}

	/*
	 * Hit every object with a LifeController and inflict damages.
	 */
	void OnCollisionEnter (Collision colision) {
		LifeShieldManager otherLife = colision.gameObject.GetComponent<LifeShieldManager> ();
		if (otherLife != null) {
			otherLife.Hit (damageFactor);
		}

		isExploding = true;
		StartCoroutine ("Explosion");
	}

	IEnumerator Explosion () {
		GetComponent<Rigidbody> ().drag = 10f;
		GetComponent<Collider> ().enabled = false;
		transform.FindChild("Model").gameObject.SetActive(false);

		GameObject explosion = (GameObject)Instantiate (explosionPrefab, transform.position, transform.rotation);
		explosion.transform.parent = this.transform;
		explosion.transform.localScale = Vector3.one;

		yield return new WaitForSeconds(explosionTime);
		Destroy (gameObject);

		yield return null;
	}
}
