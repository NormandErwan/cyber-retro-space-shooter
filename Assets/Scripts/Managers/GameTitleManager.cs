using UnityEngine;
using System.Collections;

public class GameTitleManager : MonoBehaviour {

	public GameObject gameTitleCanvas;
	public GameObject playerHUDCanvas;
	public PlayerController player;
	public float startGameTextDelay;

	private Animator gameTitleAnimator;
	private float startGameTextTimer = 0f;

	void Awake () {
		gameTitleAnimator = GetComponent<Animator> ();
	}

	void Start () {
		SetupGameTitle ();
	}

	void Update () {
		TapToStart ();
	}

	/*
	 * Show the game title canvas, deactive the HUD and make idle the player.
	 */
	void SetupGameTitle () {
		player.Deactivate ();

		playerHUDCanvas.SetActive (false);
		gameTitleCanvas.SetActive (true); // Declenche ShowGameTitleClip animation
	}

	/*
	 * Display the tap to start animation after a delay, and launch the level if there is a tap.
	 */
	void TapToStart () {
		if (startGameTextTimer < startGameTextDelay) {
			startGameTextTimer += Time.deltaTime;

			if (startGameTextTimer >= startGameTextDelay) {
				gameTitleAnimator.SetTrigger ("TapToStart");
			}
			return;
		}

		// Get a tap anywhere on the screen=
		if (Utilities.IsTapOnScreen()) {
			gameTitleAnimator.SetTrigger ("LaunchLevel");
		}
	}

	public void LaunchLevel () {
		gameTitleCanvas.SetActive (false);
		playerHUDCanvas.SetActive (true);
		
		player.Launch ();
	}
}
