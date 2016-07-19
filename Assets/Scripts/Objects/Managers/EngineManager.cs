using UnityEngine;
using System.Collections;

public class EngineManager : Observable {

	public float minSpeed, maxSpeed;

	private float speed, speedPercentage;
	private float lastRealSpeedPercentage;
	private Rigidbody rigidBody;

	private const float MIN_PERCENTAGE = 0, MAX_PERCENTAGE = 100;

	void Awake () {
		ConfigurateSpeed ();
	}

	void Start () {
		rigidBody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate () {
		// Real speed is changing
		float realSpeedPercentage = RealSpeedPercentage();
		if (realSpeedPercentage != lastRealSpeedPercentage) {
			realSpeedPercentage = lastRealSpeedPercentage;
			NotifyObservers ();
		}
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
		float realSpeed = Mathf.Round(rigidBody.velocity.magnitude) * rigidBody.mass;
		float realSpeedPercentage = (realSpeed - minSpeed) / (maxSpeed - minSpeed) * MAX_PERCENTAGE;
		realSpeedPercentage = Mathf.Max(realSpeedPercentage, MIN_PERCENTAGE);
		return realSpeedPercentage;
	}

	public void Move () {
		rigidBody.AddForce (transform.forward * speed, ForceMode.Force);
	}
}
