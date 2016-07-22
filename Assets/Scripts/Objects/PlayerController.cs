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

	// The player scores when he/she gets close to space objects without hitting them.
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

	/*
	 * Initialize the player's HUD elements.
	 */
	void ConfigureHUD () {
		healthUI.slider.minValue = LifeShieldManager.MIN_LIFE_POINTS;
		healthUI.slider.maxValue = lifeShieldManager.lifeCapacity;

		shieldUI.slider.minValue = LifeShieldManager.MIN_SHIELD_POINTS;
		shieldUI.slider.maxValue = lifeShieldManager.ShieldCapacity;

		engineUI.slider.minValue = EngineManager.MIN_PERCENTAGE;
		engineUI.slider.maxValue = engine.SpeedPercentage;

		// NOTE: set the engineShieldSlider to MIN_PERCENTAGE in the editor because Unity adjust the percentages 
		// at the first frame and it will reset the shield
		engineShieldSlider.minValue = MIN_PERCENTAGE;
		engineShieldSlider.maxValue = Mathf.Round (MAX_PERCENTAGE * ENGINE_SHIELD_SLIDER_STEP_PERCENTAGE / engineShieldSliderRectHeight);
		engineShieldSliderStep = MAX_PERCENTAGE / engineShieldSlider.maxValue;
	}

	/*
	 * Distributes the power between the shield capacity and the engine's speed.
	 */
	public void AdjustEngineShield (float percentage) {
		// Calculate the shield and engine slider percentages
		float shieldCapacityPercentage = (engineShieldSlider.maxValue - percentage) * engineShieldSliderStep;
		float speedPercentage = percentage * engineShieldSliderStep;

		// Adjust the slider heights
		shieldSliderRect.sizeDelta = new Vector2 (engineSliderRect.sizeDelta.x, engineShieldSliderRectHeight * shieldCapacityPercentage / MAX_PERCENTAGE);
		engineSliderRect.sizeDelta = new Vector2 (engineSliderRect.sizeDelta.x, engineShieldSliderRectHeight * speedPercentage / MAX_PERCENTAGE);

		// Adjust the slider values
		lifeShieldManager.ShieldCapacityPercentage = shieldCapacityPercentage;
		engine.SpeedPercentage = speedPercentage;
	}

	/*
	 * Move the player's ship.
	 */
	protected override void Move () {
		if (lifeShieldManager.LifePoints > LifeShieldManager.MIN_LIFE_POINTS) {
			engine.Move ();
		}
	}

	/*
	 * Update the HUD when the life or the shield have been changed, or invoke the game over when life points drop to zero.
	 */
	protected override void OnLifeShieldUpdated () {
		UpdateHUD ();

		if (lifeShieldManager.LifePoints <= LifeShieldManager.MIN_LIFE_POINTS) {
			gameOverManager.GameOver ();
		}
	}

	/*
	 * Update the HUD when the speed has been changed.
	 */
	protected override void OnSpeedUpdated () {
		UpdateHUD ();
	}

	/*
	 *  Update the player's HUD information.
	 */
	void UpdateHUD () {
		float lifePoints = lifeShieldManager.LifePoints;
		healthUI.valueText.text = lifePoints.ToString("000");
		healthUI.slider.value = lifePoints;

		shieldUI.valueText.text = "%" + lifeShieldManager.ShieldPointsPercentage.ToString("000");
		shieldUI.slider.value = lifeShieldManager.ShieldPoints;
		shieldUI.slider.maxValue = lifeShieldManager.ShieldCapacity;

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
		if (lifeShieldManager.LifePoints > LifeShieldManager.MIN_LIFE_POINTS) {
			if (Utilities.IsInLayerMask (other.gameObject.layer, objectAvoidanceMask)) {
				scoreManager.Score += objectAvoidanceFactorScore; 
				//scoreManager.Score += objectAvoidanceDic [other.gameObject];
				//objectAvoidanceDic.Remove (other.gameObject);
			}
		}
	}
}
