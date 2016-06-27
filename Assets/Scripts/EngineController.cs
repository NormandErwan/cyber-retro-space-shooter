using UnityEngine;
using System.Collections;

public class EngineController : MonoBehaviour {

	public float minSpeed, maxSpeed;

	private float speed;

	void Start () {
		SetupSpeed ();
	}

	public void AjustSpeed (float speedPercentage) {
		if (speedPercentage < 0 || speedPercentage > 100) {
			Debug.LogError (name + " (EngineController::AjustSpeed): speedPercentage should be set within the interval of 0 to 100.");
			return;
		}
		speed = speedPercentage / 100 * (maxSpeed - minSpeed) + minSpeed;
	}

	public void Move () {
		GetComponent<Rigidbody> ().AddForce (transform.forward * speed);
	}

	void SetupSpeed () {
		if (minSpeed > maxSpeed) {
			Debug.LogError (name + " (EngineController): Min Speed is greater than Max Speed. Please set Min Speed lower than Max Speed.");
		}

		speed = minSpeed;
	}
}
