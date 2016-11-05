using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class HighScoreManager : MonoBehaviour
{
    [Serializable]
    public class HighScoreEntry
    {
        public int score;
        public string name;
    }

    [HideInInspector]
    public static HighScoreManager Instance = null;

    [SerializeField]
    private string filename = "highscores.dat";

    [SerializeField]
    public int highscoresLen = 10;

    private List<HighScoreEntry> highScores;

    public List<HighScoreEntry> HighScores
    {
        get { return highScores; }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        LoadHighScores();
    }

	private void SaveHighScores()
    {
        FileStream fs = new FileStream(filename, FileMode.Create);
        BinaryFormatter formatter = new BinaryFormatter();

        try
        {
            formatter.Serialize(fs, highScores);
        }
        catch (SerializationException e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            fs.Close();
        }
    }

    private void LoadHighScores()
    {
        if (!File.Exists(filename))
        {
            Debug.Log("Failed to load high scores.");

            highScores = new List<HighScoreEntry>();
            return;
        }

        FileStream fs = new FileStream(filename, FileMode.Open);

        try
        {
            BinaryFormatter formatter = new BinaryFormatter();
            highScores = (List<HighScoreEntry>)formatter.Deserialize(fs);
        }
        catch (SerializationException e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            fs.Close();
        }
    }

    public void AddScore(int score, string name)
    {
        highScores.Add(new HighScoreEntry { score = score, name = name });

        // reverse sort, highest score on top
        highScores.Sort((x, y) => y.score.CompareTo(x.score));

        // truncate number of scores saved
        highScores = highScores.GetRange(0, Mathf.Min(highScores.Count, highscoresLen));

        // save to disk
        SaveHighScores();
    }
}
