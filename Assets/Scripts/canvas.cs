using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Microsoft.Unity.VisualStudio.Editor;
using Image = UnityEngine.UI.Image;


public class canvas : MonoBehaviour
{

    public TextMeshProUGUI displayState; // for GameState text
    public TextMeshProUGUI displayState2; // for special menu
    public TextMeshProUGUI displayInstruction; // special menu scene text
    public TextMeshProUGUI displayScore; // current score
    public TextMeshProUGUI displayLives; // lives left
    public Sprite[] lifeSprites;
    public Image[] displayLivesImage;





    void OnEnable()
    {
        //Debug.Log("in OnEnable in Canvas");
        GameManager.Instance.OnGameStateChangedToPlaying += OnPlaying;
        GameManager.Instance.OnGameStateChangedToPaused += OnPause;
        GameManager.Instance.OnGameStateChangedToMenu += OnMenu;
        GameManager.Instance.OnGameStateChangedToOver += OnOver;

        // score update
        ball.Instance.BrickDestroyed += UpdateScore;
        ball.Instance.BallLives += UpdateBallLives;
        //ball.Instance.BallLives += UpdateLives;     
    }

    void OnDisable()
    {
        GameManager.Instance.OnGameStateChangedToPlaying -= OnPlaying;
        GameManager.Instance.OnGameStateChangedToPaused -= OnPause;
        GameManager.Instance.OnGameStateChangedToMenu -= OnMenu;
        GameManager.Instance.OnGameStateChangedToOver -= OnOver;

        // score
        ball.Instance.BrickDestroyed -= UpdateScore;
        ball.Instance.BallLives -= UpdateBallLives;
        //ball.Instance.BallLives -= UpdateLives;
    }


    void UpdateScore(int bricksDestroyed)
    {
        //Debug.Log("score updated");
        displayScore.text = (100*bricksDestroyed).ToString();
    }

    void UpdateBallLives(int lives)
    {
        //Debug.Log("in updateBallLives");
        displayLives.text = lives.ToString();

        // populate image UI elements with solid white ball sprites
        displayLivesImage[0].sprite = lifeSprites[1];   // solid white
        displayLivesImage[1].sprite = lifeSprites[1];   // solid white
        displayLivesImage[2].sprite = lifeSprites[1];   // solid white

        // replace solid circle with empty circle when ball is out for playing
        for (int i = 0; i < 3 - lives; i++)
        {
            displayLivesImage[i].sprite = lifeSprites[0];   // white lined circle
        }

    }

    void OnMenu()
    {
        displayState.text = "Main Menu";
        displayInstruction.text = "Press Space Bar to play";
        //Debug.Log(" I am on Menu");
        //Debug.Log(SceneManager.GetActiveScene().name);
    }

    void OnPlaying()
    {
        //Debug.Log("setting gameOnPlaying to true");
        //Debug.Log("gameOnPlaying: " + gameOnPlaying);
        //Debug.Log("gameOnpause: " + gameOnPause);
        //Debug.Log(" I am on Playing");
        displayInstruction.text = "";
        displayState.text = "Playing";
    }

    void OnPause()
    {
        //Debug.Log("setting gameOnPause to true");
        //Debug.Log("gameOnPlaying: " + gameOnPlaying);
        //Debug.Log("gameOnpause: " + gameOnPause);
        //Debug.Log("I am on Pause");

        displayState.text = "Paused";
    }

    void OnOver()
    {
        displayState.text = "Game Over";
        //displayState2.text = "<color=Red>Special</color>\n<size=70><color=Blue>Scene</color></size>";
        displayState2.text = "Score: " + (100 * ball.Instance.getDestroyedBricksCount()).ToString();

        //displayInstruction.text = "Press Space Bar to restart game";
        displayInstruction.text = "Press Space Bar to play again.\nPress Q to return to Main Menu";
        //Debug.Log(" I am on Game Over");
        //SceneManager.LoadScene("Menu_Scene");
        //Debug.Log(SceneManager.GetActiveScene().name);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //displayState2.text = "<color=Red>Special</color>\n<size=70><color=Blue>Scene</color></size>";
        //displayInstruction.text = "Press Space Bar for Playing.\nPress Q for the initial Main Menu.";
        

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);

    }
    
}
