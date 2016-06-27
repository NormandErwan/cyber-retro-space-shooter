using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class PlayerController : ShipController {

	/*
	 * Fire with the weapon when the user requests it.
	 */
	protected override void WeaponFire () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			weapon.DiscreteFire ();
		}
		else if (Input.GetKey (KeyCode.Space)) {
			weapon.ContinuousFire ();
		}
	}

	/*
	 * Move the player's ship.
	 */
	protected override void Move () {
		engine.Move ();
	}

	/*
	 * Manage the notifications of the LifeController.
	 */
	protected override void LifeObserver () {
		HUDInfos.text = "Life: " + life.LifePoints.ToString("F1") + " Shield: " + life.ShieldPoints.ToString("F1") + " (Player)";
		if (life.LifePoints <= 0) {
			Debug.Log ("Game Over");
		}
	}
}
