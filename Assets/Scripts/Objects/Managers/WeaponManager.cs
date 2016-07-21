using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponManager : MonoBehaviour {

	public GameObject[] weapons;
	public GameObject munition;
	public float fireRate = 0.1f;
	public float fireEffectsTime = 0.1f;

	private float nextFireTimer = 0f;
	private int nextWeaponIndex = 0;

	/*
	 * Fire considering the weapon fire rate.
	 */
	public void ContinuousFire () {
		if (Time.time > nextFireTimer) {
			Fire ();
			nextFireTimer = Time.time + fireRate;
		}
	}

	/*
	 * Fire immediatly without taking account of the weapon fire rate.
	 */
	public void DiscreteFire () {
		Fire ();
		nextFireTimer = Time.time + fireRate;
	}

	/*
	 * Instantiate with a force a munition in the direction of the weapon. 
	 */
	void Fire () {
		GameObject weapon = weapons [nextWeaponIndex];
		nextWeaponIndex = ++nextWeaponIndex % weapons.Length;

		GameObject concreteMunition = Instantiate<GameObject> (munition);
		concreteMunition.transform.SetParent (weapon.transform);
		concreteMunition.transform.localPosition = munition.transform.position;
		concreteMunition.transform.localRotation = Quaternion.identity;
		concreteMunition.transform.SetParent (null);

		Vector3 munitionFireForce = GetComponent<Rigidbody> ().velocity 
			+ weapon.transform.forward * concreteMunition.GetComponent<MunitionController> ().fireVelocity;
		concreteMunition.GetComponent<Rigidbody> ().AddForce (munitionFireForce, ForceMode.VelocityChange);

		//StartCoroutine (DisableFireEffects (nextWeaponIndex));
	}

	/*
	 * Disable the fire effects after a delay.
	 */
	IEnumerator DisableFireEffects (int weaponIndex) {
		yield return new WaitForSeconds(fireEffectsTime);
	}
}
