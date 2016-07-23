using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public abstract class SpaceObject : MonoBehaviour {

	public LifeShieldManager lifeShieldManager;

	protected ScoreManager scoreManager;
	protected Rigidbody rigidBody;

	private float damageFactor = 0.01f;
	private UnityEvent lifeEvents;

	protected virtual void Awake () {
		rigidBody = GetComponent<Rigidbody> ();

		lifeShieldManager.OnLifeShieldUpdated += OnLifeShieldUpdated;
	}

	protected virtual void Start () {
		scoreManager = GameObject.FindGameObjectWithTag ("ScoreManager").GetComponent<ScoreManager> ();

		OnLifeShieldUpdated(); // Init the LifeController info in the HUD
	}

	/*
	 * Manage the notifications of the LifeController.
	 */
	protected abstract void OnLifeShieldUpdated ();


	/*
	 * Save the damages due to collision.
	 */
	protected virtual void OnCollisionEnter (Collision other) {
		rigidBody.AddForce (other.impulse, ForceMode.Impulse);

		float damages = other.impulse.magnitude * damageFactor;
		lifeShieldManager.Hit (damages);
	}
}
