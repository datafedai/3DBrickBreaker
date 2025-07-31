using TMPro;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEditor.Experimental.GraphView;

using System; // for Action


public class GameManager : MonoBehaviour
{

    //public TextMeshProUGUI displayState; // for GameState text
    //public TextMeshProUGUI displayState2; // for special menu
    //public TextMeshProUGUI displayInstruction; // special menu scene text
    GameState currentGameState;
    static GameState newGameState; // game state after loding special menu scene 

    InputAction playGame;
    InputAction pauseGame;
    InputAction quitGame;
    //private bool canMovePaddle; // to control paddle move when paused

    //public GameObject ballObject;
    //Rigidbody ballRB;
    private const float gameOverThresholdY = -7f;

    public static GameManager Instance { get; private set; }
    public event Action OnGameStateChangedToPlaying;
    public event Action OnGameStateChangedToPaused;
    public event Action OnGameStateChangedToMenu;
    public event Action OnGameStateChangedToOver;

    public GameObject ballObject;
    ball ballScript;
    private bool isInvoked;


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




    private enum GameState
    {
        Menu,       // main menu: menu only scene
        Playing,    // playing game: can move paddle
        Paused,       // game paused: can't move paddle
        Over        // Experimental: game over when ball is lost downward
    }


    private void handleGameState()
    {
        switch (currentGameState)
        {
            case GameState.Menu:
                if (isInvoked)
                {
                    OnGameStateChangedToMenu?.Invoke();
                    isInvoked = false;
                }



                //Debug.Log("in Menu state");
                // enable Paddle Control
                //canMovePaddle = true;

                //display menu state
                //displayState.text = "Main Menu";

                //Debug.Log(SceneManager.GetActiveScene().name);

                // if in main game scene, check for space bar input and change game state accordingly
                if (SceneManager.GetActiveScene().name == "3_Scene")
                {
                    //Debug.Log("ding1");
                    if (playGame.triggered)
                    {
                        //Debug.Log("ding2");
                        isInvoked = true;
                        currentGameState = GameState.Playing;
                    }
                }
                // if in special temporary scene after quiting game, display appropriate instruction
                // and check user input to choose for Main Menu or Playing of the main game scene 

                /*
                else if (SceneManager.GetActiveScene().name == "Menu_Scene")
                {
                    Debug.Log("ding3");
                    //displayInstruction.text = "Press Space Bar to play again.\nPress Q to reurn to Main Menu.";
                    if (newGameState == GameState.Over)
                    {
                        Debug.Log("ding4");
                        //displayState2.text = "<color=Red>--Game Over--</color>";
                    }

                    if (playGame.triggered)
                    {
                        Debug.Log("ding5");
                        SceneManager.LoadScene("3_Scene");
                        currentGameState = GameState.Playing;
                    }
                    else if (quitGame.triggered)
                    {
                        Debug.Log("ding6");
                        SceneManager.LoadScene("3_Scene");
                        currentGameState = GameState.Menu;
                    }

                }
                */

                break;


            case GameState.Playing:
                if (isInvoked)
                {
                    OnGameStateChangedToPlaying?.Invoke();
                    isInvoked = false;
                }


                //Debug.Log(" in Playing state");
                // enable PaddleController
                //canMovePaddle = true;

                // display Playing
                //displayState.text = "Playing";

                // if ball gets lost, game over


                // if pauseGame triggered, (= ESC or P pressed)
                // pause game
                // change currentGameState to Paused

                if (pauseGame.triggered)
                {
                    isInvoked = true;
                    currentGameState = GameState.Paused;
                    //Debug.Log("pausing game");
                }

                /*
                                // Experimental: Game Over
                                Vector3 ballPos = ballObject.transform.position;
                                //Vector3 paddlePos = paddleObject.transform.position;
                                //Debug.Log("ballY:paddleY = " + ballPos.y + " : " + paddlePos.y);
                                if (ballPos.y < gameOverThresholdY)
                                {
                                    currentGameState = GameState.Over;
                                }
                */
                break;


            case GameState.Paused:
                if (isInvoked)
                {
                    OnGameStateChangedToPaused?.Invoke();
                    isInvoked = false;
                }



                //Debug.Log("in pause state");
                // display Paused text
                //displayState.text = "Paused";

                // pause ball
                //pauseBall();

                // if pauseGame triggered, (= ESC or P pressed)
                // resume playing
                // change currentGameState to Playing
                if (pauseGame.triggered)
                {
                    isInvoked = true;
                    currentGameState = GameState.Playing;
                    //Debug.Log("replaying game");
                }


                // if quitGame triggered,(= Q pressed)
                // go to Main Menu
                // change currentGameState to Menu
                if (quitGame.triggered)
                {
                    isInvoked = true;
                    newGameState = GameState.Over;
                    //Debug.Log("quitting game");
                    SceneManager.LoadScene("Menu_Scene");

                    //prevGameState = GameState.Paused;
                    //nextGameState = GameState.Playing; // a new state when space bar pressed from Menu_Scene
                }

                break;

            case GameState.Over:
                //Debug.Log("in over state");
                //Debug.Log("is invoked1: " + isInvoked);
                if (isInvoked)
                {
                    OnGameStateChangedToOver?.Invoke();
                    isInvoked = false;
                    //Debug.Log("hey");
                }
                //Debug.Log("is invoked2: " + isInvoked);
                // change scene
                //SceneManager.LoadScene("Menu_Scene");
                //prevGameState = GameState.Over;
                //currentGameState = GameState.Menu;
                //nextGameState = GameState.Playing; // a new state when space bar pressed from Menu_Scene

                if (quitGame.triggered)
                {
                    //newGameState = GameState.Over;
                    //Debug.Log("Going back to Main menu");
                    isInvoked = true;
                    newGameState = GameState.Menu;
                    SceneManager.LoadScene("3_Scene");

                    //prevGameState = GameState.Paused;
                    //nextGameState = GameState.Playing; // a new state when space bar pressed from Menu_Scene
                }

                if (playGame.triggered)
                {
                    //Debug.Log("Paying again");
                    isInvoked = true;
                    newGameState = GameState.Playing;
                    SceneManager.LoadScene("3_Scene");
                }

                break;


            default:
                break;

        }

    }

    /*
        public GameState getCurrentGameState()
        {
            return currentGameState;
        }
    */


    /*
        public bool getCanMovePaddle()
        {
            return canMovePaddle;
        }
    */
    /*
        void unpauseBall()
        {
            // Time.timeScale to 1
            Time.timeScale = 1;

            // apply gravity
            ballRB.useGravity = true;
        }


        void pauseBall()
        {
            // Time.timeScale to 1
            Time.timeScale = 0;

            // apply gravity
            ballRB.useGravity = false;
        }
    */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isInvoked = true;
        Debug.Log("isInvoked set to true on Start");
        //ballRB = ballObject.GetComponent<Rigidbody>();

        playGame = InputSystem.actions.FindAction("Confirm");   // space bar
        pauseGame = InputSystem.actions.FindAction("Restart");  // ESC or P
        quitGame = InputSystem.actions.FindAction("Menu");      // Q

        //currentGameState = GameState.Menu;
        /*
        displayState.text = "Main Menu";
        displayState2.text = "<color=Red>Special</color>\n<size=70><color=Blue>Scene</color></size>";
        displayInstruction.text = "Press Space Bar for Playing.\nPress Q for the initial Main Menu.";
        */


        //canMovePaddle = true;

        // When newGameState and currentGameState are declared, they get the valueof the 1st enum, Menu
        // currentGameState = GameState.Menu, newGameState = GameState.Menu, 
        // so don't need to assign GameState.Menu at Start()

        if (SceneManager.GetActiveScene().name == "3_Scene" && newGameState == GameState.Playing) // if loaded from the new scene
        {
            currentGameState = GameState.Playing;
            //Debug.Log("here1");
        }


        else if (SceneManager.GetActiveScene().name == "Menu_Scene" && newGameState == GameState.Over)
        {
            currentGameState = GameState.Over;
            //isInvoked = true;
            //Debug.Log("here2");
        }

        //Debug.Log("newGameState: " + newGameState);
        //Debug.Log("currentGameState: " + currentGameState);

        ballScript = ballObject.GetComponent<ball>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(ballObject.transform.position.y);
        if (currentGameState != GameState.Over && ballScript.isBallMissed)
        {
            currentGameState = GameState.Over;
            newGameState = GameState.Over;
            SceneManager.LoadScene("Menu_Scene");
            Debug.Log("in update if");
        }

        handleGameState();
    }
}
