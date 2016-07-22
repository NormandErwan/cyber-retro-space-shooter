﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public abstract class SpaceObject : MonoBehaviour {

	public LifeShieldManager lifeShieldManager;

	protected ScoreManager scoreManager;

	private float damageFactor = 0.0005f;
	private UnityEvent lifeEvents;
	private Queue<float> collisionDamages = new Queue<float> ();

	protected virtual void Awake () {
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
		float damages = other.impulse.magnitude * damageFactor;
		collisionDamages.Enqueue (damages);
	}

	/*
	 * Takes the saved damages due to collision.
	 */
	protected virtual void OnCollisionExit (Collision other) {
		float damages = collisionDamages.Dequeue ();
		lifeShieldManager.Hit (damages);
	}
}
