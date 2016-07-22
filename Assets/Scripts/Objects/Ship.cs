using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public abstract class Ship : SpaceObject {

	public WeaponManager weapon;
	public EngineManager engine;

	private UnityEvent engineEvents;

	protected override void Start () {
		base.Start ();

		engineEvents = new UnityEvent();
		engineEvents.AddListener (EngineObserver);
		engine.AddObserver (engineEvents);

		EngineObserver (); // Init the EngineController info in the HUD
	}

	protected virtual void FixedUpdate () {
		Move ();
	}

	/*
	 * Move the ship.
	 */
	protected abstract void Move ();

	/*
	 * Manage the notifications of the EngineController.
	 */
	protected abstract void EngineObserver ();
}
