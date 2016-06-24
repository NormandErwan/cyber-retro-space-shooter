using UnityEngine;
using System.Collections;

public class LifeController : MonoBehaviour {

	public float lifePoints = 1.0f;

	public void Hit (float damageAmount) {
		lifePoints -= damageAmount;

		if (lifePoints <= 0f) {
			Destroy (this.gameObject);
		}
	}
}
