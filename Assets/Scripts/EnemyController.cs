using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class EnemyController : ShipController {

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
		HUDInfos.text = "Life: " + life.LifePoints.ToString("F1") + " Shield: " + life.ShieldPoints.ToString("F1") + " (Enemy)";
		if (life.LifePoints <= 0) {
			Destroy (gameObject);
		}
	}
}
