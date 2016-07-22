using UnityEngine;
using System.Collections;

public class LifeShieldManager : MonoBehaviour {

	// Life parameters.
	public float lifeCapacity = 1f;

	// Shield parameters.
	public float shieldMaxCapacity = 1f;
	public float shieldRechargeSecondsDelay = 1f;
	public float shieldRechargeRatePerSecond = 0.5f;

	// Called after the life or shield points has been updated.
	public delegate void LifeShieldUpdatedDelegate();
	public event LifeShieldUpdatedDelegate OnLifeShieldUpdated;

	private float lifePoints;
	private float shieldPoints, shieldCapacity, shieldPointsPercentage;
	private float shieldRechargeTimer = 0f;
	private bool lifeShieldUpdate = false;

	public const float MIN_LIFE_POINTS = 0f;
	public const float MIN_SHIELD_POINTS = 0f;
	private const float MIN_PERCENTAGE = 0f, MAX_PERCENTAGE = 100f;

	void Awake () {
		Configurate ();
	}

	void Update () {
		RechargeShield ();
	}

	void LateUpdate () {
		if (OnLifeShieldUpdated != null && lifeShieldUpdate == true) {
			lifeShieldUpdate = false;
			OnLifeShieldUpdated ();
		}
	}

	/*
	 * Getters / Setters.
	 */
	public float LifePoints {
		get { return lifePoints; }
		set { lifeCapacity = lifePoints = value; }
	}
	public float ShieldPoints {
		get { return shieldPoints; }
	}
	public float ShieldCapacity {
		get { return shieldCapacity; }
	}
	public float ShieldPointsPercentage {
		get { return shieldPointsPercentage; }
	}
	public float ShieldCapacityPercentage {
		set { AdjustShield (value); }
	}

	/*
	 * Init the life and shield points to their maximum capacity.
	 */
	void Configurate () {
		lifePoints = lifeCapacity;
		shieldPoints = shieldCapacity = shieldMaxCapacity;
		shieldPointsPercentage = MAX_PERCENTAGE;
	}

	/*
	 * Reduce shield points, then life points according to the damage amount of the hit, and notify the observer.
	 */
	public void Hit (float damageAmount) {
		if (lifePoints <= MIN_LIFE_POINTS) { // The observer has already been informed
			return;
		}

		shieldRechargeTimer = 0f;

		// Reduce the shield points if possible, if not reduce the life points
		float shieldDamages = Mathf.Min(shieldPoints, damageAmount);
		float newShieldPoints = shieldPoints - shieldDamages;
		AdjustShieldPoints (newShieldPoints);

		float lifeDamages = Mathf.Min(lifePoints, damageAmount - shieldDamages);
		lifePoints -= lifeDamages;

		lifeShieldUpdate = true;
	}

	/*
	 * Drops the life and shield points to zero.
	 */
	public void Die () {
		Hit (shieldPoints + lifePoints);
	}

	/*
	 * After a delay, recharge the shield at a shield rate. Should be called every frame by Update().
	 */
	void RechargeShield () {
		if (shieldPoints >= shieldCapacity 
			|| lifePoints <= MIN_LIFE_POINTS) { // Don't recharge if the player is dead
			return;
		} else if (shieldRechargeTimer < shieldRechargeSecondsDelay) {
			shieldRechargeTimer += Time.deltaTime;
			return;
		}

		float newShieldPoints = shieldPoints + Mathf.Min(shieldRechargeRatePerSecond * Time.deltaTime, shieldCapacity - shieldPoints);
		AdjustShieldPoints (newShieldPoints);

		lifeShieldUpdate = true;
	}

	/*
	 * Adjust the shield capacity and therefore the shield points.
	 */
	void AdjustShield (float shieldCapacityPercentage) {
		if (shieldCapacityPercentage < MIN_PERCENTAGE || shieldCapacityPercentage > MAX_PERCENTAGE) {
			Debug.LogError (name + " (LifeController::AjustShield): shieldCapacityPercentage should be set within the " +
				"interval of 0 to 100. Received " + shieldCapacityPercentage + ".");
			return;
		}

		// Reduce the points when reducing the capacity. When capacity is augmenting, the points will be recharged.
		float newShieldCapacity = shieldCapacityPercentage * shieldMaxCapacity / MAX_PERCENTAGE;
		if (newShieldCapacity < shieldPoints) { 
			float newShieldPoints = newShieldCapacity * shieldPoints / shieldCapacity;
			AdjustShieldPoints (newShieldPoints);
			shieldRechargeTimer = 0f;
		}
		shieldCapacity = newShieldCapacity;

		lifeShieldUpdate = true;
	}

	/*
	 * Adjust the shield points up to the limit of the shield capacity, after a hit or a recharge.
	 */
	void AdjustShieldPoints (float shieldPoints) {
		if (shieldPoints < MIN_SHIELD_POINTS || shieldPoints > shieldCapacity) {
			Debug.LogError (name + " (LifeController::AdjustShieldPoints): shieldPoints should be set within the " +
				"interval of 0 to shieldCapacity (" + shieldCapacity + "). Received " + shieldPoints + ".");
			return;
		}

		this.shieldPoints = shieldPoints;
		shieldPointsPercentage = shieldPoints * MAX_PERCENTAGE / shieldMaxCapacity;
	}
}
