using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : Ship {

	public UISliderText healthUI;
	public UISliderText shieldUI;
	public UISliderText engineUI;
	public GameOverManager gameOverManager;

	public int objectAvoidanceFactorScore = 1;
	public LayerMask objectAvoidanceMask;

	private float engineUISliderStep;
	//private Dictionary<GameObject, int> objectAvoidanceDic = new Dictionary<GameObject, int> ();

	private const float MIN_PERCENTAGE = 0f, MAX_PERCENTAGE = 100f;
	private readonly float SLIDER_FILL_BAR_HEIGTH = 20f;

	protected override void Start () {
		base.Start ();

		ConfigureHUD ();
		UpdateHUD ();
	}

	void ConfigureHUD () {
		healthUI.slider.minValue = LifeShieldManager.MIN_LIFE_POINTS;
		healthUI.slider.maxValue = life.lifeCapacity;

		shieldUI.slider.minValue = LifeShieldManager.MIN_SHIELD_POINTS;
		shieldUI.slider.maxValue = life.shieldMaxCapacity;

		engineUI.slider.minValue = MIN_PERCENTAGE;
		engineUI.slider.maxValue = Mathf.Round (MAX_PERCENTAGE * SLIDER_FILL_BAR_HEIGTH / engineUI.slider.GetComponent<RectTransform> ().rect.height);
		engineUISliderStep = MAX_PERCENTAGE / engineUI.slider.maxValue;
	}

	public void AdjustShieldEngine (float value) {
		float shieldCapacityPercentage = (engineUI.slider.maxValue - value) * engineUISliderStep;
		life.ShieldCapacityPercentage = shieldCapacityPercentage;

		float speedPercentage = value * engineUISliderStep;
		engine.SpeedPercentage = speedPercentage;
	}

	/*
	 * Fire with the weapon when the user requests it.
	 * TODO: use for debug on PC only, to remove.
	 */
	protected override void WeaponFire () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			weapon.DiscreteFire ();
		}
		else if (Input.GetKey (KeyCode.Space)) {
			weapon.ContinuousFire ();
		}
	}

	/*
	 * Move the player's ship.
	 */
	protected override void Move () {
		if (life.LifePoints > LifeShieldManager.MIN_LIFE_POINTS) {
			engine.Move ();
		}
	}

	/*
	 * Manage the notifications of the LifeController.
	 */
	protected override void LifeObserver () {
		UpdateHUD ();

		if (life.LifePoints <= LifeShieldManager.MIN_LIFE_POINTS) {
			gameOverManager.GameOver ();
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
		healthUI.valueText.text = lifePoints.ToString("000");
		healthUI.slider.value = lifePoints;

		shieldUI.valueText.text = "%" + life.ShieldPointsPercentage.ToString("000");
		shieldUI.slider.value = life.ShieldPoints;

		float speedPercentage = engine.RealSpeedPercentage ();
		engineUI.valueText.text = "%" + speedPercentage.ToString ("000");
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
