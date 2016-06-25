using UnityEngine;
using System.Collections;

public class LifeController : MonoBehaviour {

	public float lifePoints = 1f;

	public float shieldCapacity = 1f;
	public float shieldRechargeDelay = 1f;
	public float shieldRechargeRatePerSecond = 0.1f;

	private float lastHitTime = 0f;
	private float shieldPoints = 1f;

	void Start () {
		shieldPoints = shieldCapacity;
	}

	void Update () {
		RechargeShield ();
	}

	/*
	 * 
	 */
	public void Hit (float damageAmount) {
		lastHitTime = Time.time;

		if (shieldPoints > 0) {
			shieldPoints -= damageAmount;
			return;
		}
		lifePoints -= damageAmount;

		if (lifePoints <= 0f) {
			Destroy (this.gameObject);
		}
	}

	/*
	 * 
	 */
	void RechargeShield () {
		if (shieldPoints == shieldCapacity || lastHitTime + shieldRechargeDelay > Time.time) {
			return;
		}

		shieldPoints += shieldRechargeRatePerSecond * Time.deltaTime;
		shieldPoints = Mathf.Clamp(shieldPoints, 0f, shieldCapacity);
	}
}
