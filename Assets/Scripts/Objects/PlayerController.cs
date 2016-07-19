using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : Ship {

	public Text healthValueText;
	public Slider healthSlider;
	public Text shieldValueText;
	public Slider shieldSlider;
	public Text engineValueText;
	public Slider engineShieldSlider;

	public int objectAvoidanceFactorScore = 1;
	public LayerMask objectAvoidanceMask;

	private float engineShieldSliderStep;
	//private Dictionary<GameObject, int> objectAvoidanceDic = new Dictionary<GameObject, int> ();

	private const float MIN_PERCENTAGE = 0f, MAX_PERCENTAGE = 100f;
	private readonly float SLIDER_FILL_BAR_HEIGTH = 20f;

	protected override void Start () {
		base.Start ();

		ConfigureHUD ();
		UpdateHUD ();
	}

	void ConfigureHUD () {
		healthSlider.minValue = LifeShieldManager.MIN_LIFE_POINTS;
		healthSlider.maxValue = life.lifeCapacity;

		shieldSlider.minValue = LifeShieldManager.MIN_SHIELD_POINTS;
		shieldSlider.maxValue = life.shieldMaxCapacity;

		engineShieldSlider.minValue = MIN_PERCENTAGE;
		engineShieldSlider.maxValue = Mathf.Round (MAX_PERCENTAGE * SLIDER_FILL_BAR_HEIGTH / engineShieldSlider.GetComponent<RectTransform> ().rect.height);
		engineShieldSliderStep = MAX_PERCENTAGE / engineShieldSlider.maxValue;
	}

	public void AdjustShieldEngine (float value) {
		float shieldCapacityPercentage = (engineShieldSlider.maxValue - value) * engineShieldSliderStep;
		life.ShieldCapacityPercentage = shieldCapacityPercentage;

		float speedPercentage = value * engineShieldSliderStep;
		engine.SpeedPercentage = speedPercentage;
	}

	/*
	 * Fire with the weapon when the user requests it.
	 */
	protected override void WeaponFire () {
		if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown (KeyCode.Space)) {
			weapon.DiscreteFire ();
		}
		else if (Input.touchCount > 0 || Input.GetKey (KeyCode.Space)) {
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
		UpdateHUD ();

		if (life.LifePoints == LifeShieldManager.MIN_LIFE_POINTS) {
			Debug.Log ("Game Over");
		}
	}

	/*
	 * Manage the notifications of the EngineController.
	 */
	protected override void EngineObserver () {
		UpdateHUD ();
	}

	/*
	 *  Update the player's HUD information.
	 */
	void UpdateHUD () {
		float lifePoints = life.LifePoints;
		healthValueText.text = lifePoints.ToString("000");
		healthSlider.value = lifePoints;

		shieldValueText.text = "%" + life.ShieldPointsPercentage.ToString("000");
		shieldSlider.value = life.ShieldPoints;

		float speedPercentage = engine.RealSpeedPercentage ();
		engineValueText.text = "%" + speedPercentage.ToString ("000");
	}

	// TODO : the closer has been the player to the other collider the more point he/she gains
	/*void OnTriggerEnter (Collider other) {
		if (Utilities.IsInLayerMask (other.gameObject.layer, objectAvoidanceLayers)) {
			objectAvoidanceDic [other.gameObject] = 0;
		}
	}

	void OnTriggerStay (Collider other) {
		if (Utilities.IsInLayerMask (other.gameObject.layer, objectAvoidanceLayers)) {
			objectAvoidanceDic [other.gameObject] = objectAvoidanceFactorScore;
		}
	}*/

	void OnTriggerExit (Collider other) {
		if (Utilities.IsInLayerMask (other.gameObject.layer, objectAvoidanceMask)) {
			scoreManager.Score += objectAvoidanceFactorScore; 
			//scoreManager.Score += objectAvoidanceDic [other.gameObject];
			//objectAvoidanceDic.Remove (other.gameObject);
		}
	}
}
