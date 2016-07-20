using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour {

	public Text scoreText;

	private int score;

	void Start () {
		score = 0;
		UpdateHUD ();
	}

	public int Score {
		get { return score; }
		set { 
			score = value;
			UpdateHUD ();
		}
	}

	void UpdateHUD () {
		scoreText.text = "SCORE " + score;
	}
}
