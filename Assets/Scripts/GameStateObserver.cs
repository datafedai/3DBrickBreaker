using UnityEngine;

public class GameStateObserver : MonoBehaviour
{




    void OnEnable()
    {
        //Debug.Log("in OnEnable in Canvas");
        GameManager.Instance.OnGameStateChangedToPlaying += OnConsolePlaying;
        GameManager.Instance.OnGameStateChangedToPaused += OnConsolePaused;
        GameManager.Instance.OnGameStateChangedToMenu += OnConsoleMenu;
        GameManager.Instance.OnGameStateChangedToOver += OnConsoleOver;
    }

    void OnDisable()
    {
        GameManager.Instance.OnGameStateChangedToPlaying -= OnConsolePlaying;
        GameManager.Instance.OnGameStateChangedToPaused -= OnConsolePaused;
        GameManager.Instance.OnGameStateChangedToMenu -= OnConsoleMenu;
        GameManager.Instance.OnGameStateChangedToOver -= OnConsoleOver;
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


    void OnConsoleMenu()
    {
        Debug.Log("On Main Menu");
    }

    void OnConsoleOver()
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
