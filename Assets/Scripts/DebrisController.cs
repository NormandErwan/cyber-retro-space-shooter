using UnityEngine;
using System.Collections;

public class DebrisController : SpaceObjectController {

	public float minRandomScale;
	public float maxRandomScale;
	public float scaleVariability;
	public float maxRandomSpeed;
	public float maxRandomTumble;

	protected override void Start () {
		base.Start();
		DebrisConfiguration ();
	}

	void DebrisConfiguration () {
		Rigidbody rigidbody = GetComponent<Rigidbody> ();

		// Random scale
		Vector3 randomScale = Vector3.one * Random.value * (maxRandomScale - minRandomScale)
							+ (Random.insideUnitSphere * 2 * scaleVariability - Vector3.one)
							+ Vector3.one * minRandomScale;
		transform.localScale = randomScale;

		// Random life points
		life.LifePoints *= randomScale.magnitude;

		// Random mass
		rigidbody.mass *= randomScale.magnitude;

		// Random velocity
		rigidbody.AddForce (Random.insideUnitSphere * maxRandomSpeed * rigidbody.mass, ForceMode.Impulse);
		rigidbody.AddTorque (Random.insideUnitSphere * maxRandomTumble * rigidbody.mass, ForceMode.Impulse);
	}

	/*
	 * Manage the notifications of the LifeController.
	 */
	protected override void LifeObserver () {
		/*if (life.LifePoints == 0f) {
			Destroy (gameObject);
		}*/
	}
}
