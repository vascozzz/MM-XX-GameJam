using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using InControl;

public class HighscoreController : MonoBehaviour
{
    [SerializeField] private Text[] scores;
    [SerializeField] private Text[] names;
    [SerializeField] private float changeSceneDelay = 15f;

    private float changeSceneTime;

    void Start ()
    {
        changeSceneTime = Time.time + changeSceneDelay;

        List<HighScoreManager.HighScoreEntry> highscores = HighScoreManager.Instance.HighScores;
        
        for (int i = 0; i < scores.Length; i++)
        {
            scores[i].text = highscores[i].score.ToString();
            names[i].text = highscores[i].name;
        }
	}

    void Update()
    {
        if (Time.time > changeSceneTime)
        {
            SceneManager.LoadScene("Game");
        }

        foreach(InputDevice device in InputManager.Devices)
        {
            if (device.Action1.WasPressed)
            {
                SceneManager.LoadScene("Game");
            }
        }
    }
}
