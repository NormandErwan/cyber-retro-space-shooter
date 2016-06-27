using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class LifeController : MonoBehaviour {

	public float lifeCapacity = 1f;

	public float shieldCapacity = 1f;
	public float shieldRechargeDelay = 1f;
	public float shieldRechargeRatePerSecond = 0.1f;

	private float lifePoints;
	private float shieldPoints;
	private float lastHitTime = 0f;

	private UnityEvent observer;

	void Start () {
		lifePoints = lifeCapacity;
		shieldPoints = shieldCapacity;
	}

	void Update () {
		RechargeShield ();
	}

	/*
	 * Life points getter.
	 */
	public float LifePoints {
		get {
			return lifePoints;
		}
	}

	/*
	 * Shield points getter.
	 */
	public float ShieldPoints {
		get {
			return shieldPoints;
		}
	}

	/*
	 * Add an observer which be notified of every hit.
	 */
	public void AddObserver (UnityEvent observer) {
		this.observer = observer;
	}

	/*
	 * Reduce shield points, then life points according to the damage amount of the hit, and inform the observer.
	 */
	public void Hit (float damageAmount) {
		if (lifePoints <= 0f) { // The observer has already been informed
			return;
		}

		lastHitTime = Time.time;

		// Reduce shield points or life points
		if (shieldPoints > 0) {
			shieldPoints -= damageAmount;
			return;
		}
		lifePoints -= damageAmount;

		if (observer != null) {
			Debug.LogError (name + " (LifeController): no observer has been set.");
		} else {
			observer.Invoke ();
		}
	}

	/*
	 * After a 'shieldRechargeDelay', recharge the shield at a shield rate. Should be called every frame by Update().
	 */
	void RechargeShield () {
		if (lifePoints <= 0f || shieldPoints == shieldCapacity || lastHitTime + shieldRechargeDelay > Time.time) {
			return;
		}

		shieldPoints += shieldRechargeRatePerSecond * Time.deltaTime;
		shieldPoints = Mathf.Clamp(shieldPoints, 0f, shieldCapacity);
	}
}
