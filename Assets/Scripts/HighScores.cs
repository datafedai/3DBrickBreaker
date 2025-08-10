using UnityEngine;
using System;
using System.Collections.Generic;


[Serializable]
public class HighScores : MonoBehaviour
{
    public string playerName;
    public int score;
    //public HighScoreEntry[] highScoreEntryList;
    List<HighScoreEntry> highScores = new List<HighScoreEntry>();

    public void AddHighScore(string playerName, int score)
    {
        highScores.Add(new HighScoreEntry {playerName = playerName, score = score});
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
        foreach (var each in highScores)
        {
            Debug.Log("Player: " + each.playerName + ", Score: " + each.score);
        }
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("HighScores Start");
        PopulateScores();
        AddHighScore("Anonimous", 5000);
        Debug.Log(highScores.Count + " high scores populated.");
        RetrieveHighScores();
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




