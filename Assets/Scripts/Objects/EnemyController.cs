using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class EnemyController : Ship {

	public int scoreValue = 1;

	/*
	 * Move the ship.
	 */
	protected override void Move () {
		// TODO
	}

	/*
	 * Manage the notifications of the LifeController.
	 */
	protected override void LifeObserver () {
		if (lifeShieldManager.LifePoints == LifeShieldManager.MIN_LIFE_POINTS) {
			scoreManager.Score += scoreValue;
			Destroy (gameObject);
		}
	}

	/*
	 * Manage the notifications of the EngineController.
	 */
	protected override void EngineObserver () {
	}
}
