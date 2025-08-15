using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine.UIElements;


[Serializable]
public class HighScoreEntry
{
    public string playerName;
    public int score;
}

[Serializable]
public class HighScoreDataArray{
    public HighScoreEntry[] highScoreEntryList;
}

[Serializable]
public class HighScoreDataList
{
    public List<HighScoreEntry> highScoreEntryList;
}



[Serializable]
public class HighScores : MonoBehaviour
{
    public string playerName;
    public int score;
    //public HighScoreEntry[] highScoreEntryList;

    List<HighScoreEntry> emptyHighScores = new List<HighScoreEntry>();
    public static HighScores Instance { get; private set; }
    List<HighScoreEntry> highScores = new List<HighScoreEntry>();

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


    public List<HighScoreEntry> getHighScores()
    {
        //loadHighScores();
        return highScores;
    }


    public void AddHighScore(string playerName, int score)
    {
        highScores.Add(new HighScoreEntry { playerName = playerName, score = score });
    }




    void PopulateScores()
    {
        // populate
        highScores.Add(new HighScoreEntry { playerName = "SungGak", score = 1700 });
        highScores.Add(new HighScoreEntry { playerName = "Pascal", score = 2500 });
        highScores.Add(new HighScoreEntry { playerName = "Isaac ", score = 1200 });
        highScores.Add(new HighScoreEntry { playerName = "JungEun", score = 1600 });
    }

    void RetrieveHighScores()
    {
        foreach (HighScoreEntry each in highScores)
        {
            Debug.Log("Player: " + each.playerName + ", Score: " + each.score);
        }
    }

    public void saveHighScoresArray()
    {
        HighScoreDataArray highScoreData = new HighScoreDataArray { highScoreEntryList = highScores.ToArray() };
        string json = JsonUtility.ToJson(highScoreData, true);
        File.WriteAllText(Application.persistentDataPath + "/highscoreData.json", json);
        Debug.Log("High scores saved to " + Application.persistentDataPath + "/highscoreData.json");
        Debug.Log("High scores count: " + highScores.Count);
    }

    public void saveHighScoresList()
    {
        // sort high scores in descending order
        highScores.Sort((x, y) => y.score.CompareTo(x.score)); // Sort high scores in descending order
        Debug.Log("saving scores...");
        //Debug.Log(highScores[0].score);
        //Debug.Log(highScores[1].score);
        //Debug.Log(highScores[2].score); 
        //Debug.Log(highScores[3].score);
   
        HighScoreDataList highScoreData = new HighScoreDataList { highScoreEntryList = highScores };
        string json = JsonUtility.ToJson(highScoreData, true);
        File.WriteAllText(Application.persistentDataPath + "/highscoreData.json", json);
        //Debug.Log("High scores saved to " + Application.persistentDataPath + "/highscoreData2.json");
        //Debug.Log("High scores count: " + highScores.Count);
    }

    void loadHighScoresArray()
    {
        string filePath = Application.persistentDataPath + "/highscoreData.json";
        string json = File.ReadAllText(filePath);
        HighScoreDataArray data = JsonUtility.FromJson<HighScoreDataArray>(json);
        //Debug.Log("Loaded high scores: " + data.highScoreEntryList.Length);
        //Debug.Log("Loaded high scores: " + data.highScoreEntryList.Length + " entries.");
        //highScores = data.highScoreEntryList.ToList();
        highScores = data.highScoreEntryList.ToList();
        //Debug.Log("highScores count after loading: " + highScores.Count);
        //Debug.Log(highScores[0].score);
        //Debug.Log(highScores[1].score);
        //Debug.Log(highScores[2].score); 
        //Debug.Log(highScores[3].score);
        /*
        foreach (HighScoreEntry entry in highScores)
        {
            highScores.Add(entry);
            Debug.Log("Player: " + entry.playerName + ", Score: " + entry.score);
        }
        */

    }



    public void loadHighScoresList()
    {
        string filePath = Application.persistentDataPath + "/highscoreData.json";
        string json = File.ReadAllText(filePath);
        HighScoreDataList data = JsonUtility.FromJson<HighScoreDataList>(json);
        //List<HighScoreEntry> highScores = JsonUtility.FromJson<List<HighScoreEntry>>(json);
        //Debug.Log("Loaded high scores: " + highScores.Count);
        //Debug.Log("Loaded high scores: " + data.highScoreEntryList.Length + " entries.");
        highScores = data.highScoreEntryList;
        //highScores = data;
        //Debug.Log("highScores count after loading: " + highScores.Count);
        Debug.Log("loading scores...");
        //Debug.Log(highScores[0].score);
        //Debug.Log(highScores[1].score);
        //Debug.Log(highScores[2].score); 
        //Debug.Log(highScores[3].score);
        foreach (HighScoreEntry entry in highScores)
        {
            //highScores.Add(entry);        
            //Debug.Log("Player: " + entry.playerName + ", Score: " + entry.score);
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

        //saveHighScoresList();
        loadHighScoresList();
        //Debug.Log(highScores.Count + " high scores loaded.");
    }

    // Update is called once per frame
    void Update()
    {

    }
}

