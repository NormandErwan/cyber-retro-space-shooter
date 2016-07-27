using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameOverManager : MonoBehaviour {

	public Animator playerHUDAnimator;
	public float canRestartLevelDelay;

	private bool gameOver = false;
	private float restartLevelTimer = 0f;

	void Update () {
		TapToRestartAfterDelay ();
	}

	/*
	 * Allow to restart the level by a tap after the delay.
	 */
	void TapToRestartAfterDelay () {
		if (gameOver) {
			if (restartLevelTimer < canRestartLevelDelay) {
				restartLevelTimer += Time.deltaTime;

				if (restartLevelTimer >= canRestartLevelDelay) {
					playerHUDAnimator.SetTrigger ("TapToRestart");
				}
				return;
			}

			// Get a tap anywhere on the screen
			if (Utilities.IsTapOnScreen()) {
				RestartLevel ();
			}
		}
	}

	/*
	 * Play the player hit animation.
	 */
	public void PlayerHit () {
		if (!gameOver) {
			playerHUDAnimator.SetTrigger ("PlayerHit");
		}
	}

	/*
	 * Trigger the game over and play the game over animation.
	 */
	public void GameOver () {
		playerHUDAnimator.SetTrigger ("GameOver");
		gameOver = true;
	}

	/*
	 * Restart the level.
	 */
	void RestartLevel () {
		SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
	}
}
