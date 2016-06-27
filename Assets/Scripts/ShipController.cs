using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public abstract class ShipController : MonoBehaviour {

	public LifeController life;
	public WeaponController weapon;
	public EngineController engine;
	public Text HUDInfos;

	private UnityEvent lifeEvents;

	void Start () {
		lifeEvents = new UnityEvent();
		lifeEvents.AddListener (LifeObserver);
		life.AddObserver (lifeEvents);

		LifeObserver(); // Init the LifeController infos in the HUD
	}

	void Update () {
		WeaponFire ();
	}

	void FixedUpdate () {
		Move ();
	}

	/*
	 * Fire with the weapon.
	 */
	protected abstract void WeaponFire ();

	/*
	 * Move the ship.
	 */
	protected abstract void Move ();

	/*
	 * Manage the notifications of the LifeController.
	 */
	protected abstract void LifeObserver ();
}
