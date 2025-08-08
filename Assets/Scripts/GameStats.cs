using UnityEngine;

public class GameStats : MonoBehaviour
{


    void OnEnable()
    {
        GameManager.Instance.OnGameStateChangedToWin += OnWin;
        GameManager.Instance.OnGameStateChangedToWinStats += OnWinStats;
        GameManager.Instance.OnGameStateChangedToLose += OnLose;
        //GameManager.Instance.OnGameStateChangedToLose += OnConsoleLose;
    }



    void OnDisable()
    {
        // Unsubscribe from events if necessary
        GameManager.Instance.OnGameStateChangedToWin -= OnWin;
        GameManager.Instance.OnGameStateChangedToWinStats -= OnWinStats;
        GameManager.Instance.OnGameStateChangedToLose -= OnLose;
    }

    void OnWin()
    {
        Debug.Log("saving win stats");
    }

    void OnWinStats()
    {
        Debug.Log("displaying win stats");
    }


    void OnLose()
    {
        Debug.Log("saving lose stats");
    }





    // art is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("GameStats started");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
