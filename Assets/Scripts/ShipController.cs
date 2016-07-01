using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public abstract class ShipController : SpaceObjectController {

	public WeaponController weapon;
	public EngineController engine;
	public Text HUDInfos;

	protected virtual void Update () {
		WeaponFire ();
	}

	protected virtual void FixedUpdate () {
		Move ();
	}

	/*
	 * Move the ship.
	 */
	protected abstract void Move ();

	/*
	 * Fire with the weapon.
	 */
	protected abstract void WeaponFire ();
}
