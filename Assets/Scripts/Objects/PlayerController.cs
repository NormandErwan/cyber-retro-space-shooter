using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : Ship {

	public GameObject dieExplosionPrefab;
	public float explosionTime = 1f;

	// The player scores when he/she gets close to space objects without hitting them.
	public int objectAvoidanceFactorScore = 1;
	public LayerMask objectAvoidanceMask;

	protected PlayerHUDManager playerHUDManager;
	protected GameOverManager gameOverManager;

	//private Dictionary<GameObject, int> objectAvoidanceDic = new Dictionary<GameObject, int> ();

	protected override void Awake () {
		base.Awake ();

		playerHUDManager = GetComponent<PlayerHUDManager> ();
		gameOverManager = GameObject.FindGameObjectWithTag ("GameOverManager").GetComponent<GameOverManager> ();
	}

	protected override void Start () {
		base.Start ();

		GetComponent<GvrHead> ().OnHeadUpdated += OnOrientationChanged;
	}

	/*
	 * Takes damages due to collision and play the hit animation.
	 */
	protected override void OnCollisionEnter (Collision other) {
		base.OnCollisionEnter (other);

		gameOverManager.PlayerHit();
	}

	/*
	 * Keep the velocity vector aligned with the forward vector.
	 */
	void OnOrientationChanged (GameObject player) {
		rigidBody.velocity = Quaternion.FromToRotation (rigidBody.velocity, transform.forward) * rigidBody.velocity;
	}

	/*
	 * Move the player's ship.
	 */
	protected override void Move () {
		if (lifeShieldManager.LifePoints > LifeShieldManager.MIN_LIFE_POINTS) {
			engine.Move ();
		}
	}

	/*
	 * Update the HUD when the life or the shield have been changed, or invoke the game over when life points drop to zero.
	 */
	protected override void OnLifeShieldUpdated () {
		playerHUDManager.UpdateHUD ();

		if (lifeShieldManager.LifePoints <= LifeShieldManager.MIN_LIFE_POINTS) {
			StartCoroutine ("Die");
		}
	}

	/*
	 * Instantiate the explosion, hide the model and trigger the game over.
	 */
	IEnumerator Die () {
		GameObject explosion = (GameObject)Instantiate (dieExplosionPrefab, transform.position, transform.rotation);
		explosion.transform.parent = this.transform;
		explosion.transform.localScale = Vector3.one;

		GetComponent<BoxCollider> ().enabled = false;
		transform.FindChild("PlayerModel").gameObject.SetActive(false);

		gameOverManager.GameOver ();

		yield return new WaitForSeconds(explosionTime);
		explosion.SetActive(false);

		yield return null;
	}

	/*
	 * Update the HUD when the speed has been changed.
	 */
	protected override void OnSpeedUpdated () {
		playerHUDManager.UpdateHUD ();
	}

	// TODO : the closer has been the player to the other collider the more point he/she gains
	/*void OnTriggerEnter (Collider other) {
		if (Utilities.IsInLayerMask (other.gameObject.layer, objectAvoidanceLayers)) {
			objectAvoidanceDic [other.gameObject] = 0;
		}
	}

	void OnTriggerStay (Collider other) {
		if (Utilities.IsInLayerMask (other.gameObject.layer, objectAvoidanceLayers)) {
			objectAvoidanceDic [other.gameObject] = objectAvoidanceFactorScore;
		}
	}*/

	void OnTriggerExit (Collider other) {
		if (lifeShieldManager.LifePoints > LifeShieldManager.MIN_LIFE_POINTS) {
			if (Utilities.IsInLayerMask (other.gameObject.layer, objectAvoidanceMask)) {
				scoreManager.Score += objectAvoidanceFactorScore; 
				//scoreManager.Score += objectAvoidanceDic [other.gameObject];
				//objectAvoidanceDic.Remove (other.gameObject);
			}
		}
	}
}
