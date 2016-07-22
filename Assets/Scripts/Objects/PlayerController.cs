using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : Ship {

	public UISliderText healthUI;
	public UISliderText shieldUI;
	public UISliderText engineUI;
	public Slider engineShieldSlider;
	public GameOverManager gameOverManager;

	public int objectAvoidanceFactorScore = 1;
	public LayerMask objectAvoidanceMask;

	private RectTransform shieldSliderRect, engineSliderRect;
	private float engineShieldSliderRectHeight;
	private float engineShieldSliderStep;
	//private Dictionary<GameObject, int> objectAvoidanceDic = new Dictionary<GameObject, int> ();

	private const float MIN_PERCENTAGE = 0f, MAX_PERCENTAGE = 100f;
	private readonly float ENGINE_SHIELD_SLIDER_STEP_PERCENTAGE = 20f;

	protected override void Start () {
		base.Start ();

		shieldSliderRect = shieldUI.slider.GetComponent<RectTransform> ();
		engineSliderRect = engineUI.slider.GetComponent<RectTransform> ();
		engineShieldSliderRectHeight = engineShieldSlider.GetComponent<RectTransform> ().rect.height;

		ConfigureHUD ();
		AdjustEngineShield (engineShieldSlider.value); // Force a first update of the HUD
	}

	void ConfigureHUD () {
		healthUI.slider.minValue = LifeShieldManager.MIN_LIFE_POINTS;
		healthUI.slider.maxValue = life.lifeCapacity;

		shieldUI.slider.minValue = LifeShieldManager.MIN_SHIELD_POINTS;
		shieldUI.slider.maxValue = life.ShieldCapacity;

		engineUI.slider.minValue = EngineManager.MIN_PERCENTAGE;
		engineUI.slider.maxValue = engine.SpeedPercentage;

		// NOTE: set the engineShieldSlider to MIN_PERCENTAGE in the editor because Unity adjust the percentages 
		// at the first frame and it will reset the shield
		engineShieldSlider.minValue = MIN_PERCENTAGE;
		engineShieldSlider.maxValue = Mathf.Round (MAX_PERCENTAGE * ENGINE_SHIELD_SLIDER_STEP_PERCENTAGE / engineShieldSliderRectHeight);
		engineShieldSliderStep = MAX_PERCENTAGE / engineShieldSlider.maxValue;
	}

	public void AdjustEngineShield (float percentage) {
		// Calculate the shield and engine slider percentages
		float shieldCapacityPercentage = (engineShieldSlider.maxValue - percentage) * engineShieldSliderStep;
		float speedPercentage = percentage * engineShieldSliderStep;

		// Adjust the slider heights
		shieldSliderRect.sizeDelta = new Vector2 (engineSliderRect.sizeDelta.x, engineShieldSliderRectHeight * shieldCapacityPercentage / MAX_PERCENTAGE);
		engineSliderRect.sizeDelta = new Vector2 (engineSliderRect.sizeDelta.x, engineShieldSliderRectHeight * speedPercentage / MAX_PERCENTAGE);

		// Adjust the slider values
		life.ShieldCapacityPercentage = shieldCapacityPercentage;
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
		shieldUI.slider.maxValue = life.ShieldCapacity;

		float realSpeedPercentage = engine.RealSpeedPercentage ();
		engineUI.valueText.text = "%" + realSpeedPercentage.ToString ("000");
		engineUI.slider.value = realSpeedPercentage;
		engineUI.slider.maxValue = engine.SpeedPercentage;
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
		if (life.LifePoints > LifeShieldManager.MIN_LIFE_POINTS) {
			if (Utilities.IsInLayerMask (other.gameObject.layer, objectAvoidanceMask)) {
				scoreManager.Score += objectAvoidanceFactorScore; 
				//scoreManager.Score += objectAvoidanceDic [other.gameObject];
				//objectAvoidanceDic.Remove (other.gameObject);
			}
		}
	}
}
