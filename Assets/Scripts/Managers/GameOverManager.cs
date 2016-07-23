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
			if ((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) || Input.GetKeyDown (KeyCode.Space)) {
				RestartLevel ();
			}
		}
	}

	/*
	 * Trigger the game over.
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
