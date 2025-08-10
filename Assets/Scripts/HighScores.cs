using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine.UIElements;


[Serializable]
public class HighScores : MonoBehaviour
{
    public string playerName;
    public int score;
    //public HighScoreEntry[] highScoreEntryList;
    List<HighScoreEntry> highScores = new List<HighScoreEntry>();
    List<HighScoreEntry> emptyHighScores = new List<HighScoreEntry>();
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
        highScores.Add(new HighScoreEntry { playerName = "SungGak", score = 3700 });
        highScores.Add(new HighScoreEntry { playerName = "Pascal", score = 2500 });
        highScores.Add(new HighScoreEntry { playerName = "Isaac ", score = 4200 });
        highScores.Add(new HighScoreEntry { playerName = "JungEun", score = 1600 });
        highScores.Add(new HighScoreEntry { playerName = "SungGak", score = 7900 });
        highScores.Add(new HighScoreEntry { playerName = "SungGak", score = 4500 });
        highScores.Add(new HighScoreEntry { playerName = "Pascal", score = 2700 });
        highScores.Add(new HighScoreEntry { playerName = "Isaac", score = 4100 });
        highScores.Add(new HighScoreEntry { playerName = "Pascal", score = 6000 });
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

    public void saveHighScores2()
    {
        HighScoreData2 highScoreData = new HighScoreData2 { highScoreEntryList2 = highScores };
        string json = JsonUtility.ToJson(highScoreData, true);
        File.WriteAllText(Application.persistentDataPath + "/highscoreData2.json", json);
        Debug.Log("High scores saved to " + Application.persistentDataPath + "/highscoreData.json");
        Debug.Log("High scores count: " + highScores.Count);
    }

    void loadHighScores()
    {
        string filePath = Application.persistentDataPath + "/highscoreData.json";
        string json = File.ReadAllText(filePath);
        HighScoreData data = JsonUtility.FromJson<HighScoreData>(json);
        Debug.Log("Loaded high scores: " + data.highScoreEntryList.Length);
        //Debug.Log("Loaded high scores: " + data.highScoreEntryList.Length + " entries.");
        //highScores = data.highScoreEntryList.ToList();
        highScores = data.highScoreEntryList.ToList();
        //Debug.Log("highScores count after loading: " + highScores.Count);
        foreach (HighScoreEntry entry in highScores)
        {
            //highScores.Add(entry);        
            //Debug.Log("Player: " + entry.playerName + ", Score: " + entry.score);
        }

    }



    void loadHighScores2()
    {
        string filePath = Application.persistentDataPath + "/highscoreData2.json";
        string json = File.ReadAllText(filePath);
        HighScoreData2 data = JsonUtility.FromJson<HighScoreData2>(json);
        //List<HighScoreEntry> highScores = JsonUtility.FromJson<List<HighScoreEntry>>(json);
        Debug.Log("Loaded high scores: " + highScores.Count);
        //Debug.Log("Loaded high scores: " + data.highScoreEntryList.Length + " entries.");
        highScores = data.highScoreEntryList2;
        //highScores = data;
        //Debug.Log("highScores count after loading: " + highScores.Count);
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
public class HighScoreData{
    public HighScoreEntry[] highScoreEntryList;
}

[Serializable]
public class HighScoreData2
{
    public List<HighScoreEntry> highScoreEntryList2;
}
