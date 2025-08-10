using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


[Serializable]
public class HighScores : MonoBehaviour
{
    public string playerName;
    public int score;
    //public HighScoreEntry[] highScoreEntryList;
    List<HighScoreEntry> highScores = new List<HighScoreEntry>();
    public static HighScores Instance { get; private set; }

    private void Awake()
    {
        // Check if an instance already exists and it's not this one.
        if (Instance != null && Instance != this)
        {
            // Destroy the duplicate instance.
            //Debug.Log("destroying..." + gameObject.name);
            Destroy(gameObject);
        }
        else
        {
            // Assign this instance as the Singleton.
            Instance = this;
            //Debug.Log("this is " + this);
            // Optionally, prevent the Singleton from being destroyed on scene changes.
            // DontDestroyOnLoad(gameObject);
        }
    }


    public void AddHighScore(string playerName, int score)
    {
        highScores.Add(new HighScoreEntry { playerName = playerName, score = score });
    }

    void PopulateScores()
    {
        // populate
        highScores.Add(new HighScoreEntry { playerName = "SungGak", score = 7500 });
        highScores.Add(new HighScoreEntry { playerName = "Pascal", score = 2500 });
        highScores.Add(new HighScoreEntry { playerName = "Isaac", score = 5500 });
        highScores.Add(new HighScoreEntry { playerName = "JungEun", score = 9500 });
        highScores.Add(new HighScoreEntry { playerName = "SungGak", score = 7500 });
        highScores.Add(new HighScoreEntry { playerName = "SungGak", score = 4500 });
        highScores.Add(new HighScoreEntry { playerName = "Pascal", score = 2500 });
        highScores.Add(new HighScoreEntry { playerName = "Isaac", score = 4500 });
        highScores.Add(new HighScoreEntry { playerName = "Pascal", score = 9500 });
    }

    void RetrieveHighScores()
    {
        foreach (HighScoreEntry each in highScores)
        {
            Debug.Log("Player: " + each.playerName + ", Score: " + each.score);
        }
    }

    public void saveHighScores()
    {
        HighScoreData highScoreData = new HighScoreData { highScoreEntryList = highScores.ToArray() };
        string json = JsonUtility.ToJson(highScoreData, true);
        File.WriteAllText(Application.persistentDataPath + "/highscoreData.json", json);
        Debug.Log("High scores saved to " + Application.persistentDataPath + "/highscoreData.json");
        Debug.Log("High scores count: " + highScores.Count);
    }

    void loadHighScores()
    {
        string filePath = Application.persistentDataPath + "/highscoreData.json";
        string json = File.ReadAllText(filePath);
        HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
        Debug.Log("Loaded high scores: " + data.highScoreEntryList.Length);
        highScores = data.highScoreEntryList.ToList();
        foreach (HighScoreEntry entry in highScores)
        {
            Debug.Log("Player: " + entry.playerName + ", Score: " + entry.score);
        }
    }





    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("HighScores Start");
        //PopulateScores();
        //AddHighScore("Anonimous", 5000);
        //Debug.Log(highScores.Count + " high scores populated.");
        //RetrieveHighScores();

        //saveHighScores();
        loadHighScores();
        //Debug.Log(highScores.Count + " high scores loaded.");
    }

    // Update is called once per frame
    void Update()
    {

    }
}

[Serializable]
public class HighScoreEntry
{
    public string playerName;
    public int score;
}


[Serializable]
public class HighScoreData
{
    public HighScoreEntry[] highScoreEntryList;
}

