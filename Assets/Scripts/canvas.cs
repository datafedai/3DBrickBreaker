using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Microsoft.Unity.VisualStudio.Editor;
using Image = UnityEngine.UI.Image;
using UnityEngine.UI; // For UI elements like Button and Text
using System;
using System.Linq; // for Action


public class canvas : MonoBehaviour
{

    public TextMeshProUGUI displayState; // for GameState text
    public TextMeshProUGUI displayState2; // for special menu
    public TextMeshProUGUI displayInstruction; // special menu scene text
    public TextMeshProUGUI displayScore; // current score
    public TextMeshProUGUI displayHighScores; // high score
    public TextMeshProUGUI displayLives; // lives left

    // to display ball lives
    public Sprite[] lifeSprites;
    public Image[] displayLivesImage;

    // panel1(right side): for buttons
    public GameObject gameMenuPanel; // Reference to the panel containing the dialog
    public TextMeshProUGUI titleText; // Or public Text questionText;
    public Button playButton;
    public Button exitButton;

    // panel2(left side): for text and playhead
    public GameObject gameMenuPanelText;
    public TextMeshProUGUI titleText2;
    public Image playheadImage1;
    public TextMeshProUGUI playText;
    public Image playheadImage2;
    public TextMeshProUGUI exitText;
    public Image playheadImage3;
    public TextMeshProUGUI settingText;

    // settings panel displayed after clicking Settings on the left side menu
    public GameObject settingsPanel; // Reference to the settings panel
    public TMP_InputField myInputField;
    private static string playerName = "Anonymous"; // Default player name
    public TextMeshProUGUI outputText;
    private int playheadIndex = 0; // Index to track the current position of the playhead

    // high scores panels displayed after winning
    public GameObject highScoresPanel1; // Reference to the high scores panel
    public GameObject highScoresPanel2; // Reference to the high scores panel

    // Singleton instance
    public static canvas Instance { get; private set; }
    //public event Action ClickedYes;
    //public event Action ClickedNo;



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


    void OnEnable()
    {
        //Debug.Log("in OnEnable in Canvas");
        GameManager.Instance.OnGameStateChangedToPlaying += OnPlaying;
        GameManager.Instance.OnGameStateChangedToPaused += OnPause;
        GameManager.Instance.OnGameStateChangedToMenu += OnMenu;
        GameManager.Instance.OnGameStateChangedToWin += OnWin;
        GameManager.Instance.OnGameStateChangedToWinStats += OnWinStats;
        GameManager.Instance.OnGameStateChangedToLose += OnLose;
        GameManager.Instance.OnGameMenuPanel += OnGameMenuPanel;
        GameManager.Instance.ArrowUp += MoveArrowUp;
        GameManager.Instance.ArrowDown += MoveArrowDown;

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
        GameManager.Instance.OnGameStateChangedToWin -= OnWin;
        GameManager.Instance.OnGameStateChangedToWinStats -= OnWinStats;
        GameManager.Instance.OnGameStateChangedToLose -= OnLose;
        GameManager.Instance.OnGameMenuPanel -= OnGameMenuPanel;
        GameManager.Instance.ArrowUp -= MoveArrowUp;
        GameManager.Instance.ArrowDown -= MoveArrowDown;
        //GameManager.Instance.WonGame -= YouWonGame;
        // score
        ball.Instance.BrickDestroyed -= UpdateScore;
        ball.Instance.BallLives -= UpdateBallLives;
        //ball.Instance.BallLives -= UpdateLives;
    }



    void MoveArrowUp()
    {
        //Debug.Log("in MoveArrowUp in Canvas");
        //Debug.Log("playheadIndex: " + playheadIndex);
        //playheadImage1.transform.position += new Vector3(0f, 85f, 0f);
        //playText.color = Color.white;
        //exitText.color = Color.black;
        if (playheadIndex > 0)
        {
            playheadIndex--;
        }
        else
        {
            playheadIndex = 0; // If already at the top, stay at 0  
        }


        playheadIndex = playheadIndex % 3; // Toggle between 0 and 1
                                           //Debug.Log("playheadIndex after decrement: " + playheadIndex);

        movePlayhead(playheadIndex);

    }

    void MoveArrowDown()
    {
        //Debug.Log("in MoveArrowDown in Canvas");
        //Debug.Log("playheadIndex: " + playheadIndex);
        //playheadImage1.transform.position += new Vector3(0f, -85f, 0f);
        //playText.color = Color.black;
        //exitText.color = Color.white;
        //playheadIndex++;

        if (playheadIndex < 2)
        {
            playheadIndex++; // Toggle between 0 and 1
        }
        else
        {
            playheadIndex = 2; // If already at the bottom, stay at 2
        }

        playheadIndex = playheadIndex % 3; // Toggle between 0 and 1
                                           //Debug.Log("playheadIndex after increment: " + playheadIndex);

        movePlayhead(playheadIndex);

    }



    void movePlayhead(int playheadIndex)
    {
        switch (playheadIndex)
        {
            case 0:
                playheadImage1.enabled = true;
                playheadImage2.enabled = false;
                playheadImage3.enabled = false;
                //playText.color = Color.red;
                //exitText.color = Color.white;
                break;

            case 1:
                playheadImage1.enabled = false;
                playheadImage2.enabled = true;
                playheadImage3.enabled = false;
                //playText.color = Color.white;
                //exitText.color = Color.red;
                break;

            case 2:
                playheadImage1.enabled = false;
                playheadImage2.enabled = false;
                playheadImage3.enabled = true;
                //playText.color = Color.white;
                //exitText.color = Color.red;
                break;

            default:
                break;
        }

        // Alternative way to handle the playhead index
        /*
        if (playheadIndex == 0)
        {
            playheadImage1.enabled = true;
            playheadImage2.enabled = false;
            playheadImage3.enabled = false;
            //playText.color = Color.red;
            //exitText.color = Color.white;
        }
        else if (playheadIndex == 1)
        {
            playheadImage1.enabled = false;
            playheadImage2.enabled = true;
            playheadImage3.enabled = false;
            //playText.color = Color.white;
            //exitText.color = Color.red;
        }
        else
        {
            playheadImage1.enabled = false;
            playheadImage2.enabled = false;
            playheadImage3.enabled = true;
            //playText.color = Color.white;
            //exitText.color = Color.red;
        }
        */
    }


    void OnGameMenuPanel()
    {
        Debug.Log("in OnGameMenuPanel in Canvas");

        titleText.text = "Brick Breaker 3D";

        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(() =>
        {
            Debug.Log("Play button clicked");
            GameManager.Instance.StartGameInMenu();
            gameMenuPanel.SetActive(false); // Hide the menu panel
        });

        exitButton.onClick.RemoveAllListeners();
        exitButton.onClick.AddListener(() =>
        {
            Debug.Log("Exit button clicked");

            if (Application.isEditor)
            {
                //UnityEditor.EditorApplication.isPlaying = false; 
                UnityEditor.EditorApplication.ExitPlaymode(); // Stop play mode in the editor
            }
            else
            {
                Debug.Log("Exiting application");
                Application.Quit(); // Quit the application if not in the editor
            }
        });

    }

    // left side menu with red arrow
    public void ExecuteSelection()
    {
        Debug.Log("in ExecuteSelection in Canvas");
        if (playheadIndex == 0)
        {
            Debug.Log("Play selected");
            GameManager.Instance.StartGameInMenu();
            gameMenuPanel.SetActive(false); // Hide the menu panel
        }
        else if (playheadIndex == 1)
        {
            Debug.Log("Exit selected");
            if (Application.isEditor)
            {
                //UnityEditor.EditorApplication.isPlaying = false; // Stop play mode in the editor
                UnityEditor.EditorApplication.ExitPlaymode(); // Stop play mode in the editor
            }
            else
            {
                Debug.Log("Exiting application");
                Application.Quit(); // Quit the application
            }
        }
        else if (playheadIndex == 2)
        {
            Debug.Log("Settings selected");
            settingsPanel.SetActive(true); // Show the settings panel

            // get user input for player name
            outputText.text = "Enter your name and press Space Bar";
            Debug.Log("You enetered: " + myInputField.text);

            while (string.IsNullOrEmpty(myInputField.text))
            {
                return; // wait until user enters name
            }

            myInputField.text = myInputField.text.Trim();
            if (string.IsNullOrEmpty(myInputField.text))
            {
                myInputField.text = "Anonymous"; // default name
            }

            settingsPanel.SetActive(false);
            Debug.Log(myInputField.text);
            displayState.text = "Player: " + myInputField.text;
            playerName = myInputField.text; // Update the static playerName variable
        }

    }


    void UpdateScore(int bricksDestroyed)
    {
        //Debug.Log("score updated");
        displayScore.text = (100 * bricksDestroyed).ToString();
    }

    void UpdateBallLives(int lives)
    {
        //Debug.Log("in updateBallLives");
        //displayLives.text = lives.ToString();
        displayLives.text = "";

        // populate image UI elements with solid white ball sprites
        displayLivesImage[0].sprite = lifeSprites[1];   // solid white
        displayLivesImage[1].sprite = lifeSprites[1];   // solid white
        displayLivesImage[2].sprite = lifeSprites[1];   // solid white

        // replace solid circle with empty circle when ball is out for playing
        for (int i = 2; i > lives - 1; i--)
        {
            //Debug.Log(i);
            displayLivesImage[i].sprite = lifeSprites[0];   // white lined circle or none
        }

    }

    void OnMenu()
    {
        //Debug.Log(" I am on Menu");
        displayState.text = "Main Menu";
        displayInstruction.text = "Left Menu:\nUse Up and Down arrows to choose an option.";
        displayInstruction.text += "\nPress Space Bar to confirm your choice.";
        displayInstruction.text += "\n\nRight Menu:\nUse mouse to click on buttons.";

        //Debug.Log(SceneManager.GetActiveScene().name);
    }

    void OnPlaying()
    {
        if (SceneManager.GetActiveScene().name == "Main_Scene")
        {
            //Debug.Log("in OnPlaying, setting mainPanel to false");
            gameMenuPanel.SetActive(false);
            gameMenuPanelText.SetActive(false);
        }
        else
        {
            //Debug.Log("in OnPlaying, setting mainPanel to true");
            gameMenuPanel.SetActive(true);
            gameMenuPanelText.SetActive(true);
        }



        //Debug.Log("setting gameOnPlaying to true");
        //Debug.Log("gameOnPlaying: " + gameOnPlaying);
        //Debug.Log("gameOnpause: " + gameOnPause);
        //Debug.Log(" I am on Playing");
        displayInstruction.text = "";
        displayState.text = playerName + " is playing";
    }

    void OnPause()
    {
        //Debug.Log("setting gameOnPause to true");
        //Debug.Log("gameOnPlaying: " + gameOnPlaying);
        //Debug.Log("gameOnpause: " + gameOnPause);
        //Debug.Log("I am on Pause");

        displayState.text = "Paused";
    }

    void OnLose()
    {
        displayState.text = "Game Over";
        //displayState2.text = "<color=Red>Special</color>\n<size=70><color=Blue>Scene</color></size>";
        displayState2.text = "Play Again?";
        displayScore.text = "Score: " + (100 * ball.Instance.getDestroyedBricksCount()).ToString();

        //displayInstruction.text = "Press Space Bar to restart game";
        displayInstruction.text = "Press Space Bar to play again.\nPress Q to return to Main Menu";
        //Debug.Log(" I am on Game Over");
        //SceneManager.LoadScene("Menu_Scene");
        //Debug.Log(SceneManager.GetActiveScene().name);
        int finalScore = 100 * ball.Instance.getDestroyedBricksCount();
        saveScore(finalScore);
    }

    void saveScore(int finalScore)
    {
        // Save the final score to high scores
        //string playerName = myInputField.text; // Get the player name from the input field
        HighScores.Instance.AddHighScore(playerName, finalScore); // added bonus score for ball lives
        HighScores.Instance.saveHighScoresList();
    }
    void OnWin()
    {

        //Debug.Log("You have won game!");
        //SceneManager.LoadScene("Over_Scene");
        displayState.text = "Congratulations!";
        displayState2.text = "<color=Blue>You</color>\n<size=70><color=Red>W   N!</color></size>";
        //displayState2.text = "Play Again?";
        //displayScore.text = "Score: " + (100 * ball.Instance.getDestroyedBricksCount()).ToString();
        displayHighScores.text = "";

        //displayInstruction.text = "Press Space Bar to restart game";
        displayInstruction.text = "Press Space to see high scores.\nPress Q to return to Main Menu";

        // experimental
        int playerScore = 100 * ball.Instance.getDestroyedBricksCount();
        int ballLivesLeft = 1 + ball.Instance.getBallLives();
        //Debug.Log("Player Score: " + playerScore);
        //Debug.Log("ball lives: " + ballLivesLeft);
        //Debug.Log(ball.Instance.getBallLives() + playerScore);
        int finalScore = playerScore + 300 * ballLivesLeft;
        displayScore.text = "Score: " + finalScore.ToString();

        // Save the final score to high scores
        saveScore(finalScore);

        //string playerName = myInputField.text; // Get the player name from the input field
        //HighScores.Instance.AddHighScore(playerName, finalScore); // added bonus score for ball lives
        //HighScores.Instance.saveHighScoresList();
    }

    void OnWinStats()
    {
        highScoresPanel1.SetActive(true);
        highScoresPanel2.SetActive(true);

        GameObject middleCircle = GameObject.FindGameObjectWithTag("winCircle");
        if (middleCircle != null)
        {
            middleCircle.SetActive(false); // Hide the middle circle
        }
        else
        {
            Debug.LogWarning("Middle circle not found!");
        }


        displayHighScores.text = "Score Rankings\n\n";
        HighScores.Instance.getHighScores().Sort((x, y) => y.score.CompareTo(x.score)); // Sort high scores in descending order


        displayHighScoresPanel1(); // Display high scores in panel on left
        displayHighScoresPanel2(); // Display high scores in panel in the middle



        //Debug.Log("You have won game!");
        //SceneManager.LoadScene("Over_Scene");
        displayState.text = "";
        //displayState2.text = "<color=Blue>You</color>\n<size=70><color=Red>W   N!</color></size>";
        displayState2.text = "";
        displayScore.text = "";
        //displayHighScores.text += "1. " + GameManager.Instance.GetHighScore(0) + "\n";
        //displayHighScores.text += "2. " + GameManager.Instance.GetHighScore(1) + "\n";
        //displayInstruction.text = "Press Space Bar to restart game";
        displayInstruction.text = "Press Space to play again.\nPress Q to return to Main Menu";


        //GameObject[] ranks = GameObject.FindGameObjectsWithTag("rank");
        /*
        foreach (GameObject rank in ranks)
        {
            Debug.Log("Found rank object: " + rank.name);
            //rank.SetActive(false); // Hide the rank objects
            TextMeshProUGUI[] allTextMeshPros = rank.GetComponentsInChildren<TextMeshProUGUI>();
            Debug.Log("score: " + allTextMeshPros[0].name);
            Debug.Log("name: " + allTextMeshPros[1].name);


            foreach (TextMeshProUGUI each in allTextMeshPros)
            {
                Debug.Log("Found TextMeshProUGUI: " + each.name);
                //each.text = "";
                if (each.name == "score")
                {
                    each.text = "888";
                }
                else if (each.name == "name")
                {
                    each.text = "SungGak";
                }

            } 

        }   
        */

    }




    void displayHighScoresPanel1()
    {
        // panel1: left

        // gameobject rank

        GameObject[] ranks = GameObject.FindGameObjectsWithTag("rank");
        int index = 0;

        foreach (HighScoreEntry entry in HighScores.Instance.getHighScores().Take(5))  // Display only top 10 scores
        {
            TextMeshProUGUI[] allTextMeshPros = ranks[index].GetComponentsInChildren<TextMeshProUGUI>();

            int score = entry.score;
            string playerName = entry.playerName;
            //Debug.Log("Player: " + playerName + ", Score: " + score);
            //Debug.Log("High Scores ....");
            //displayHighScores.text += entry.score + "\t\t " + entry.playerName + "\n";

            allTextMeshPros[0].text = score.ToString(); // Set the score text
            allTextMeshPros[1].text = playerName; // Set the player name text

            index++;
        }

    }


    void displayHighScoresPanel2()
    {

        // panel2: middle
        foreach (HighScoreEntry entry in HighScores.Instance.getHighScores().Take(5))  // Display only top 10 scores
        {

            int score = entry.score;
            string playerName = entry.playerName;
            //Debug.Log("Player: " + playerName + ", Score: " + score);
            //Debug.Log("High Scores ....");
            displayHighScores.text += entry.score + "\t\t " + entry.playerName + "\n";

        }

    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        highScoresPanel1.SetActive(false); // Hide the high scores panel at the start
        highScoresPanel2.SetActive(false); // Hide the high scores panel at the start

        //GameObject settingsPanel = GameObject.FindGameObjectWithTag("settingsPanel");
        settingsPanel.SetActive(false); // hide the settings panel


        playheadImage1.enabled = true;
        //playText.color = Color.red;
        playheadImage2.enabled = false;
        playheadImage3.enabled = false;
        //displayState2.text = "<color=Red>Special</color>\n<size=70><color=Blue>Scene</color></size>";
        //displayInstruction.text = "Press Space Bar for Playing.\nPress Q for the initial Main Menu.";
        //buttonPanel.SetActive(true);
        // Add listeners to the buttons
        //ClickedYes?.Invoke();
        //ClickedNo?.Invoke();
        //yesButton.onClick.AddListener(YesClicked);
        //noButton.onClick.AddListener(NoClicked);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(SceneManager.GetActiveScene().name);
        //Debug.Log("playheadIndex: " + playheadIndex);
        //Debug.Log("ball lives: " + ball.Instance.getBallLives());
    }

}
