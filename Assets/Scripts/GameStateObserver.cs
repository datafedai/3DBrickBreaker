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
        Debug.Log("game on Playing");
    }

    void OnConsolePaused()
    {
        Debug.Log("game on Pause");
    }


    void OnConsoleMenu()
    {
        Debug.Log("game on Menu");
    }

    void OnConsoleOver()
    {
        Debug.Log("game on Over");
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
