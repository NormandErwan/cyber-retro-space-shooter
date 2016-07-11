using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Observable : MonoBehaviour {

	private UnityEvent observer;

	/*
	 * Add an observer which be notified of every hit.
	 */
	public void AddObserver (UnityEvent observer) {
		this.observer = observer;
	}

	/*
	 * Notify the observers of an attribute value change.
	 */
	protected void NotifyObservers () {
		if (observer == null) {
			Debug.LogWarning (name + ": no observer has been set, can't notify.");
		} else {
			observer.Invoke ();
		}
	}
}
