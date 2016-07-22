using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	// Score HUD
	public Text scoreText;

	private int score;

	public const int MIN_SCORE = 0;

	void Start () {
		UpdateScore (MIN_SCORE);
	}

	/*
	 * Get/set the score.
	 */
	public int Score {
		get { return score; }
		set { UpdateScore (value); }
	}

	/*
	 * Update the score and the score HUD.
	 */
	void UpdateScore (int newScore) {
		score = newScore;
		scoreText.text = "SCORE " + score;
	}
}
