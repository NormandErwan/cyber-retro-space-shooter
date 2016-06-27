using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public LifeController life;
	public WeaponController weapon;
	public EngineController engine;
	public Text HUDInfos;

	private UnityEvent lifeEvents;

	void Start () {
		lifeEvents = new UnityEvent();
		lifeEvents.AddListener (LifeObserver);
		life.AddObserver (lifeEvents);

		LifeObserver(); // Init the player's infos in the HUD
	}

	void Update () {
		WeaponFire ();
	}

	void FixedUpdate () {
		Move ();
	}

	/*
	 * Fire with the weapon when the user requests it.
	 */
	void WeaponFire () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			weapon.DiscreteFire ();
		}
		else if (Input.GetKey (KeyCode.Space)) {
			weapon.ContinuousFire ();
		}
	}

	/*
	 * Move the player.
	 */
	void Move () {
		engine.Move ();
	}

	/*
	 * Manage the notifications of the LifeController.
	 */
	void LifeObserver () {
		HUDInfos.text = "Life: " + life.LifePoints + " Shield: " + life.ShieldPoints.ToString("F1");
		if (life.LifePoints <= 0) {
			Debug.Log ("Game Over");
		}
	}
}
