using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHUDManager : MonoBehaviour {

	public UISliderText healthUI;
	public UISliderText shieldUI;
	public UISliderText engineUI;
	public Slider engineShieldSlider;
	public Text UItext;

	private LifeShieldManager lifeShieldManager;
	private WeaponManager weapon;
	private EngineManager engine;
	private RectTransform shieldSliderRect, engineSliderRect;
	private float engineShieldSliderRectHeight;
	private float engineShieldSliderStep;

	private const float MIN_PERCENTAGE = 0f, MAX_PERCENTAGE = 100f;
	private readonly float ENGINE_SHIELD_SLIDER_STEP_PERCENTAGE = 20f;

	void Awake () {
		lifeShieldManager = GetComponent<LifeShieldManager> ();
		weapon = GetComponent<WeaponManager> ();
		engine = GetComponent<EngineManager> ();
	}

	void Start () {
		shieldSliderRect = shieldUI.slider.GetComponent<RectTransform> ();
		engineSliderRect = engineUI.slider.GetComponent<RectTransform> ();
		engineShieldSliderRectHeight = engineShieldSlider.GetComponent<RectTransform> ().rect.height;

		ConfigureHUD ();
		AdjustEngineShield (engineShieldSlider.value); // Force a first update of the HUD
	}

	void Update () {
		string text = "";

		bool fire = false, adjustedEngineShield = false;
		for (int i = 0; i < Input.touchCount; i++) {
			Touch touch = Input.GetTouch (i);

			text += i + "(" + touch.phase + " " + touch.deltaPosition + " " + touch.deltaTime + " " + touch.deltaPosition.magnitude / touch.deltaTime + " " +")" + " ";

			if (!fire) {
				if (touch.phase == TouchPhase.Began) {
					weapon.DiscreteFire ();
					fire = true;
				} else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary) {
					weapon.ContinuousFire ();
					fire = true;
				}
			}

			if (!adjustedEngineShield) {
				if (touch.phase == TouchPhase.Moved) {
					float value = (touch.deltaPosition.y > 0f) ? engineShieldSlider.value++ : engineShieldSlider.value--;
					AdjustEngineShield (value);
					adjustedEngineShield = true;
				}
			}
		}
		UItext.text = text;
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
	 *  Update the player's HUD information.
	 */
	public void UpdateHUD () {
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

	/*
	 * Distributes the power between the shield capacity and the engine's speed.
	 */
	public void AdjustEngineShield (float engineShieldSliderValue) {
		// Calculate the shield and engine slider percentages
		float shieldCapacityPercentage = (engineShieldSlider.maxValue - engineShieldSliderValue) * engineShieldSliderStep;
		float speedPercentage = engineShieldSliderValue * engineShieldSliderStep;

		// Adjust the slider heights
		shieldSliderRect.sizeDelta = new Vector2 (engineSliderRect.sizeDelta.x, engineShieldSliderRectHeight * shieldCapacityPercentage / MAX_PERCENTAGE);
		engineSliderRect.sizeDelta = new Vector2 (engineSliderRect.sizeDelta.x, engineShieldSliderRectHeight * speedPercentage / MAX_PERCENTAGE);

		// Adjust the slider values
		lifeShieldManager.ShieldCapacityPercentage = shieldCapacityPercentage;
		engine.SpeedPercentage = speedPercentage;
	}
}
