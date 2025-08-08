using UnityEngine;

public class GameStateObserver : MonoBehaviour
{




    void OnEnable()
    {
        //Debug.Log("in OnEnable in Canvas");
        GameManager.Instance.OnGameStateChangedToMenu += OnConsoleMenu;        
        GameManager.Instance.OnGameStateChangedToPlaying += OnConsolePlaying;
        GameManager.Instance.OnGameStateChangedToPaused += OnConsolePaused;
        GameManager.Instance.OnGameStateChangedToWin += OnConsoleWin;
        GameManager.Instance.OnGameStateChangedToWinStats += OnConsoleWinStats;
        GameManager.Instance.OnGameStateChangedToLose += OnConsoleLose;
    }

    void OnDisable()
    {
        GameManager.Instance.OnGameStateChangedToMenu -= OnConsoleMenu;
        GameManager.Instance.OnGameStateChangedToPlaying -= OnConsolePlaying;
        GameManager.Instance.OnGameStateChangedToPaused -= OnConsolePaused;
        GameManager.Instance.OnGameStateChangedToWin -= OnConsoleWin;
        GameManager.Instance.OnGameStateChangedToWinStats -= OnConsoleWinStats;
        GameManager.Instance.OnGameStateChangedToLose -= OnConsoleLose;
    }



    void OnConsoleMenu()
    {
        Debug.Log("On Main Menu");
    }
    
    void OnConsolePlaying()
    {
        //Debug.Log("This is console output");
        Debug.Log("On Game Playing");
    }

    void OnConsolePaused()
    {
        Debug.Log("On Game Paused");
    }

    void OnConsoleWin()
    {
        Debug.Log("On Game Win");
    }

    void OnConsoleWinStats()
    {
        Debug.Log("On Game WinStats");
    }

    void OnConsoleLose()
    {
        Debug.Log("On Game Over");
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
