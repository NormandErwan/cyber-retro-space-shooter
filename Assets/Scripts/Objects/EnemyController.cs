using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class EnemyController : Ship {

	public int scoreValue = 1;

	/*
	 * Fire with the weapon.
	 */
	protected override void WeaponFire () {
		// TODO
	}

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
		if (life.LifePoints == LifeController.MIN_LIFE_POINTS) {
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
