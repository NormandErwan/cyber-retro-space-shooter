using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public abstract class SpaceObject : MonoBehaviour {

	public LifeShieldManager life;

	protected ScoreManager scoreManager;

	private UnityEvent lifeEvents;

	protected virtual void Start () {
		lifeEvents = new UnityEvent();
		lifeEvents.AddListener (LifeObserver);
		life.AddObserver (lifeEvents);
		
		scoreManager = GameObject.FindGameObjectWithTag ("ScoreManager").GetComponent<ScoreManager> ();

		LifeObserver(); // Init the LifeController info in the HUD
	}

	/*
	 * Manage the notifications of the LifeController.
	 */
	protected abstract void LifeObserver ();
}
