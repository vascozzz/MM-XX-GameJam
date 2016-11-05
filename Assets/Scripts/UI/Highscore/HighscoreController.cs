using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HighscoreController : MonoBehaviour {

	void Start ()
    {
        HighScoreManager.Instance.AddScore(0, "-");
        HighScoreManager.Instance.AddScore(0, "-");
        HighScoreManager.Instance.AddScore(0, "-");
        HighScoreManager.Instance.AddScore(0, "-");
        HighScoreManager.Instance.AddScore(0, "-");
        HighScoreManager.Instance.AddScore(0, "-");
        HighScoreManager.Instance.AddScore(0, "-");
        HighScoreManager.Instance.AddScore(0, "-");
        HighScoreManager.Instance.AddScore(0, "-");
        HighScoreManager.Instance.AddScore(0, "-");
        List<HighScoreManager.HighScoreEntry> highscores = HighScoreManager.Instance.HighScores;
        print(highscores.Count);
	}
	
    
}
