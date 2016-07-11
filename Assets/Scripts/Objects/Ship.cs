using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public abstract class Ship : SpaceObject {

	public WeaponController weapon;
	public EngineController engine;

	private UnityEvent engineEvents;

	protected override void Start () {
		base.Start ();

		engineEvents = new UnityEvent();
		engineEvents.AddListener (EngineObserver);
		engine.AddObserver (engineEvents);

		EngineObserver (); // Init the EngineController info in the HUD
	}

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

	/*
	 * Manage the notifications of the EngineController.
	 */
	protected abstract void EngineObserver ();
}
