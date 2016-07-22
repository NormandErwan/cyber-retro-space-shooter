using UnityEngine;
using System.Collections;

public class EngineManager : Observable {

	public float minSpeed, maxSpeed, accelerationFactor, brakeFactor;

	private float speed, speedPercentage;
	private Rigidbody rigidBody;

	public const float MIN_PERCENTAGE = 0, MAX_PERCENTAGE = 100;

	void Awake () {
		rigidBody = GetComponent<Rigidbody> ();
		
		ConfigurateSpeed ();
	}

	void ConfigurateSpeed () {
		if (minSpeed > maxSpeed) {
			Debug.LogError (name + " (EngineController): Min Speed is greater than Max Speed. Please set Min Speed lower than Max Speed.");
		}

		speed = minSpeed;
		speedPercentage = MIN_PERCENTAGE;
	}

	public float SpeedPercentage {
		get { return speedPercentage; }
		set { AdjustSpeed (value); }
	}

	void AdjustSpeed (float speedPercentage) {
		if (speedPercentage < MIN_PERCENTAGE || speedPercentage > MAX_PERCENTAGE) {
			Debug.LogError (name + " (EngineController::AjustSpeed): speedPercentage should be set within the interval of 0 to 100.");
			return;
		}

		this.speedPercentage = speedPercentage;
		speed = speedPercentage * (maxSpeed - minSpeed) / MAX_PERCENTAGE + minSpeed;

		NotifyObservers ();
	}

	public float RealSpeedPercentage () {
		float realSpeed = rigidBody.velocity.magnitude / Time.fixedDeltaTime;
		float realSpeedPercentage = (realSpeed - minSpeed) / (maxSpeed - minSpeed) * MAX_PERCENTAGE;
		realSpeedPercentage = Mathf.Max(realSpeedPercentage, MIN_PERCENTAGE);
		return realSpeedPercentage;
	}

	public void Move () { 
		// TODO : compare with percentages not speeds
		float realSpeed = rigidBody.velocity.magnitude / Time.fixedDeltaTime;
		if (realSpeed == speed) {
			return;
		}
		
		Vector3 engineForce = transform.forward * speed * Time.fixedDeltaTime;
		if (realSpeed < speed) {
			rigidBody.AddForce (engineForce * accelerationFactor, ForceMode.Force);
		} else if (realSpeed > speed) {
			rigidBody.AddForce (-engineForce * brakeFactor, ForceMode.Force);
		}
		NotifyObservers ();
	}
}
