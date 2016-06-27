using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public LifeController life;
	public WeaponController weapon;
	public EngineController engine;

	private UnityEvent lifeEvents;

	void Start () {
		lifeEvents = new UnityEvent();
		lifeEvents.AddListener (LifeEvent);
		life.AddObserver (lifeEvents);
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
	 * 
	 */
	void Move () {
		engine.Move ();
	}

	/*
	 * 
	 */
	void LifeEvent () {
		Debug.Log ("lifeevent " + life.ShieldPoints + " " + life.LifePoints);
		if (life.LifePoints <= 0) {
			Debug.Log ("Game Over");
		}
	}
}
