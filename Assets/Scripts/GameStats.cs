using UnityEngine;

public class GameStats : MonoBehaviour
{


    void OnEnable()
    {
        GameManager.Instance.OnGameStateChangedToWin += OnWin1;
        GameManager.Instance.OnGameStateChangedToWinStats += OnWinStats1;
        GameManager.Instance.OnGameStateChangedToLose += OnLose;
        //GameManager.Instance.OnGameStateChangedToLose += OnConsoleLose;
    }



    void OnDisable()
    {
        // Unsubscribe from events if necessary
        GameManager.Instance.OnGameStateChangedToWin -= OnWin1;
        GameManager.Instance.OnGameStateChangedToWinStats -= OnWinStats1;
        GameManager.Instance.OnGameStateChangedToLose -= OnLose;
    }

    void OnWin1()
    {
        Debug.Log("saving win stats");
    }

    void OnWinStats1()
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
        //Debug.Log("GameStats is being updated");
    }
}
