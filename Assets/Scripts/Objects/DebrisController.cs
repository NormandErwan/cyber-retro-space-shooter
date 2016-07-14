using UnityEngine;
using System.Collections;

public class DebrisController : SpaceObject {

	public float minRandomScale;
	public float maxRandomScale;
	public float scaleAxisVariability;
	public float speedVariablity;
	public float maxRandomTumble;

	public int scoreValue = 1;

	private Vector3 speedForce;

	public Vector3 SpeedForce {
		get { return speedForce; }
		set { speedForce = value; }
	}

	public void ConfigurateDebris () {
		Rigidbody rigidbody = GetComponent<Rigidbody> ();

		// Random scale
		Transform parent = transform.parent;
		transform.parent = null;

		Vector3 randomScale = Vector3.one * Random.value * (maxRandomScale - minRandomScale) + Vector3.one * minRandomScale // Random axis-uniform scale
							+ (Random.insideUnitSphere * 2 * scaleAxisVariability - Vector3.one); // Random vector3 between (-1,-1,-1) to (1,1,1)
		transform.localScale = randomScale;

		transform.parent = parent;

		// Random life points
		life.LifePoints *= Mathf.Max(Mathf.RoundToInt(randomScale.magnitude), life.LifePoints);

		// Random mass
		rigidbody.mass *= randomScale.magnitude;

		// Random velocity
		rigidbody.AddForce (speedForce + Random.insideUnitSphere * speedVariablity, ForceMode.VelocityChange);
		rigidbody.AddTorque (Random.insideUnitSphere * maxRandomTumble * rigidbody.mass, ForceMode.Impulse);
	}

	/*
	 * Manage the notifications of the LifeController.
	 */
	protected override void LifeObserver () {
		if (life.LifePoints == LifeShieldManager.MIN_LIFE_POINTS) {
			scoreManager.Score += scoreValue;
			Destroy (gameObject);
		}
	}
}
