using UnityEngine;
using System.Collections;

public class GameOverManager : MonoBehaviour {

	public Animator gameOverAnimator;

	public void GameOver () {
		gameOverAnimator.SetTrigger ("GameOver");
	}
}
