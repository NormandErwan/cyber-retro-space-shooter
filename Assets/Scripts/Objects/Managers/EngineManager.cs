using UnityEngine;
using System.Collections;

public class EngineManager : MonoBehaviour {

	public float minSpeed, maxSpeed, accelerationFactor, brakeFactor;

	// Called after the speed has been updated.
	public delegate void SpeedUpdatedDelegate();
	public event SpeedUpdatedDelegate OnSpeedUpdated;

	private float speed, speedPercentage;
	private Rigidbody rigidBody;
	private bool speedUpdated = false;

	public const float MIN_PERCENTAGE = 0, MAX_PERCENTAGE = 100;

	void Awake () {
		rigidBody = GetComponent<Rigidbody> ();
		
		ConfigurateSpeed ();
	}

	void LateUpdate () {
		if (OnSpeedUpdated != null && speedUpdated == true) {
			speedUpdated = false;
			OnSpeedUpdated ();
		}
	}

	/*
	 * Getters / Setters.
	 */
	public float SpeedPercentage {
		get { return speedPercentage; }
		set { AdjustSpeed (value); }
	}

	/*
	 * Return the measured speed of the rigidbody.
	 */
	public float RealSpeedPercentage () {
		float realSpeed = rigidBody.velocity.magnitude / Time.fixedDeltaTime;
		float realSpeedPercentage = (realSpeed - minSpeed) / (maxSpeed - minSpeed) * MAX_PERCENTAGE;
		realSpeedPercentage = Mathf.Round(Mathf.Max(realSpeedPercentage, MIN_PERCENTAGE));
		return realSpeedPercentage;
	}

	/*
	 * Apply a force on the rigidbody to adjust its speed on the desired speed.
	 */
	public void Move () {
		float realSpeedPercentage = RealSpeedPercentage ();
		if (realSpeedPercentage == speedPercentage) {
			return;
		}

		Vector3 engineForce = transform.forward * speed * Time.fixedDeltaTime;
		if (realSpeedPercentage < speedPercentage) {
			rigidBody.AddForce (engineForce * accelerationFactor, ForceMode.Force);
		} else if (realSpeedPercentage > speedPercentage) {
			rigidBody.AddForce (-engineForce * brakeFactor, ForceMode.Force);
		}

		speedUpdated = true;
	}

	/*
	 * Init the speed at its minimum.
	 */
	void ConfigurateSpeed () {
		if (minSpeed > maxSpeed) {
			Debug.LogError (name + " (EngineController): Min Speed is greater than Max Speed. Please set Min Speed lower than Max Speed.");
		}

		speed = minSpeed;
		speedPercentage = MIN_PERCENTAGE;
	}

	/*
	 * Recompute the speed.
	 */
	void AdjustSpeed (float speedPercentage) {
		if (speedPercentage < MIN_PERCENTAGE || speedPercentage > MAX_PERCENTAGE) {
			Debug.LogError (name + " (EngineController::AjustSpeed): speedPercentage should be set within the interval of 0 to 100.");
			return;
		}

		this.speedPercentage = speedPercentage;
		speed = speedPercentage * (maxSpeed - minSpeed) / MAX_PERCENTAGE + minSpeed;

		speedUpdated = true;
	}
}
