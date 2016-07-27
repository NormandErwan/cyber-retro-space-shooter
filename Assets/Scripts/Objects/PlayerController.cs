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

	private GvrHead gvrHead;
	private bool activated = true, gameOver = false;

	//private Dictionary<GameObject, int> objectAvoidanceDic = new Dictionary<GameObject, int> ();

	protected override void Awake () {
		base.Awake ();

		playerHUDManager = GetComponent<PlayerHUDManager> ();
		gameOverManager = GameObject.FindGameObjectWithTag ("GameOverManager").GetComponent<GameOverManager> ();
		gvrHead = GetComponent<GvrHead> ();
	}

	void Update () {
		if (lifeShieldManager.LifePoints > LifeShieldManager.MIN_LIFE_POINTS) {
			activated = false;
		}
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
		if (activated) {
			rigidBody.velocity = Quaternion.FromToRotation (rigidBody.velocity, transform.forward) * rigidBody.velocity;
		}
	}

	/*
	 * Move the player's ship.
	 */
	protected override void Move () {
		if (activated) {
			engine.Move ();
		}
	}

	/*
	 * Update the HUD when the life or the shield have been changed, or invoke the game over when life points drop to zero.
	 */
	protected override void OnLifeShieldUpdated () {
		playerHUDManager.UpdateHUD ();

		if (!activated && gameOver) {
			gameOver = true;

			System.Action<GameObject> explodeCallback = (GameObject explosion) => {
				Destroy (explosion);
			};
			StartCoroutine (explosionManager.Explode (explodeCallback));

			gameOverManager.GameOver ();
		}
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
		if (activated) {
			if (Utilities.IsInLayerMask (other.gameObject.layer, objectAvoidanceMask)) {
				scoreManager.Score += objectAvoidanceFactorScore; 
				//scoreManager.Score += objectAvoidanceDic [other.gameObject];
				//objectAvoidanceDic.Remove (other.gameObject);
			}
		}
	}

	/*
	 * Start the ship when the level launch.
	 */
	public void Launch () {
		activated = true;

		gvrHead.enabled = true;
		gvrHead.OnHeadUpdated += OnOrientationChanged;
	}

	/*
	 * Make the ship idle.
	 */
	public void Deactivate () {
		activated = false;

		gvrHead.enabled = false;
	}
}
