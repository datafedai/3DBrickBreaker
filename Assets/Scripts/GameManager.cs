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
    GameState currentGameState;
    static GameState newGameState; // game state after loading special menu scene 

    InputAction playGame;
    InputAction pauseGame;
    InputAction quitGame;
    public InputSystem_Actions inputActions;
    private Action<InputAction.CallbackContext> spacePerformedAction;
    private Action<InputAction.CallbackContext> escPerformedAction;
    private Action<InputAction.CallbackContext> pPerformedAction;
    private Action<InputAction.CallbackContext> qPerformedAction;


    public static GameManager Instance { get; private set; }
    public event Action OnGameStateChangedToPlaying;
    public event Action OnGameStateChangedToPaused;
    public event Action OnGameStateChangedToMenu;
    public event Action OnGameStateChangedToOver;

    public GameObject ballObject;
    ball ballScript;
    //private bool isInvoked;

    private enum GameState
    {
        Menu,       // main menu: menu only scene
        Playing,    // playing game: can move paddle
        Paused,       // game paused: can't move paddle
        Over        // Experimental: game over when ball is lost downward
    }


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

        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        //inputActions.Player.Confirm.performed += spacePerformedAction = ctx => handleSpacePressed();
        //inputActions.Player.Restart.performed += escPerformedAction = ctx => handleEscOrPPressed();
        //inputActions.Player.Restart.performed += pPerformedAction = ctx => handleEscOrPPressed();
        //inputActions.Player.Menu.performed += qPerformedAction = ctx => handleQPressed();

        // on Menu: space bar
        inputActions.Menu.PlayGame.performed += spacePerformedAction = ctx => handleSpacePressed();

        // on Playing: P (not both P and ESC)
        inputActions.Playing.PauseGame.performed += pPerformedAction = ctx => handleEscOrPPressed();

        // on Pause: esc, Q
        inputActions.Paused.ContinueGame.performed += escPerformedAction = ctx => handleEscOrPPressed(); 
        inputActions.Paused.QuitGame.performed += qPerformedAction = ctx => handleQPressed();

        // on Over: Q, space bar
        inputActions.Over.ReturnToMenu.performed += qPerformedAction = ctx => handleQPressed();
        inputActions.Over.PlayAgain.performed += spacePerformedAction = ctx => handleSpacePressed();

        // enable input action map
        inputActions.Enable();
    }
    void OnDisable()
    {
        inputActions.Disable();
        inputActions.Menu.PlayGame.performed -= spacePerformedAction = ctx => handleSpacePressed();
        inputActions.Playing.PauseGame.performed -= pPerformedAction = ctx => handleEscOrPPressed();
        inputActions.Paused.ContinueGame.performed -= escPerformedAction = ctx => handleEscOrPPressed();
        inputActions.Paused.QuitGame.performed -= qPerformedAction = ctx => handleQPressed();
        inputActions.Over.ReturnToMenu.performed -= qPerformedAction = ctx => handleQPressed();
        inputActions.Over.PlayAgain.performed -= spacePerformedAction = ctx => handleSpacePressed();

        //inputActions.Player.Confirm.performed -= spacePerformedAction = ctx => handleSpacePressed();
        //inputActions.Player.Restart.performed -= escPerformedAction = ctx => handleEscOrPPressed();
        //inputActions.Player.Restart.performed -= pPerformedAction = ctx => handleEscOrPPressed();
        //inputActions.Player.Menu.performed -= qPerformedAction = ctx => handleQPressed();
    }

    void handleSpacePressed()
    {
        Debug.Log("Space Bar pressed");

        switch (currentGameState)
        {
            case GameState.Menu:
                if (SceneManager.GetActiveScene().name == "Main_Scene")
                {
                    Debug.Log("playing from main menu");
                    OnGameStateChangedToPlaying?.Invoke();
                    currentGameState = GameState.Playing;
                }
                break;

            case GameState.Over:
                Debug.Log("changing scenes: from Over to Main");
                OnGameStateChangedToPlaying?.Invoke();
                currentGameState = GameState.Playing;
                newGameState = GameState.Playing;
                SceneManager.LoadScene("main_Scene");
                break;

            default:
                break;
        }


    }


    void handleEscOrPPressed()
    {
        Debug.Log("ESC or P pressed");
        switch (currentGameState)
        {
            case GameState.Playing:
                //
                Debug.Log("pausing game from playing");
                OnGameStateChangedToPaused?.Invoke();
                currentGameState = GameState.Paused;
                break;

            case GameState.Paused:
                //
                Debug.Log("replaying game from pause");
                OnGameStateChangedToPlaying?.Invoke();
                currentGameState = GameState.Playing;

                break;

            default:
                break;
        }
    }


    void handleQPressed()
    {
        Debug.Log("Q pressed");
        switch (currentGameState)
        {
            case GameState.Paused:
                //
                Debug.Log("quitting game from pasue");
                //OnGameStateChangedToOver?.Invoke();
                //currentGameState = GameState.Over;
                SceneManager.LoadScene("Over_Scene");
                break;

            case GameState.Over:
                //
                Debug.Log("returning to main menu");
                //OnGameStateChangedToMenu?.Invoke();
                newGameState = GameState.Menu;
                SceneManager.LoadScene("Main_Scene");
                break;

            default:
                break;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("starting... a new scene");
        Debug.Log("current state: " + currentGameState);

        //isInvoked = true;
        //Debug.Log("isInvoked set to true on Start");
        //ballRB = ballObject.GetComponent<Rigidbody>();
        /*
                playGame = InputSystem.actions.FindAction("Confirm");   // space bar
                pauseGame = InputSystem.actions.FindAction("Restart");  // ESC or P
                quitGame = InputSystem.actions.FindAction("Menu");      // Q
        */

        // When newGameState and currentGameState are declared, they get the valueof the 1st enum, Menu
        // currentGameState = GameState.Menu, newGameState = GameState.Menu, 
        // so don't need to assign GameState.Menu at Start()

        Debug.Log("current scene: " + SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "Main_Scene")
        {
            if (newGameState == GameState.Playing)
            {
                currentGameState = GameState.Playing;
                OnGameStateChangedToPlaying?.Invoke();
            }
            else if(newGameState == GameState.Menu)
            {
                currentGameState = GameState.Menu;
                OnGameStateChangedToMenu?.Invoke();
            }

        }
        else if (SceneManager.GetActiveScene().name == "Over_Scene")
        {
            OnGameStateChangedToOver?.Invoke();
            currentGameState = GameState.Over;
        }

        //Debug.Log("newGameState: " + newGameState);
        //Debug.Log("currentGameState: " + currentGameState);

        ballScript = ballObject.GetComponent<ball>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(ballObject.transform.position.y);
        //Debug.Log("current state: " + currentGameState);

        // if ball is missed while game is not over, that is on playing
        if (currentGameState != GameState.Over && ballScript.isBallMissed)
        {
            if (ballScript.canContinueToPlay())
            {
                ballScript.resetIsBallLaunched();
                currentGameState = GameState.Playing;
            }
            else
            {
                currentGameState = GameState.Over;
                SceneManager.LoadScene("Over_Scene");
                Debug.Log("Total " + ballScript.getDestroyedBricksCount() + " brickes destroyed.");
            }

        }

        // check if all the bricks are destroyed
        if (ballScript.getDestroyedBricksCount() == 105)
        {
            Debug.Log("You won!");
            SceneManager.LoadScene("Main_Scene");
        }

        //handleGameState();
    }
}




/*
    private void handleGameState()
    {
        switch (currentGameState)
        {
            case GameState.Menu:
                // To make sure Invoke() is activated only once per key stroke
                if (isInvoked)
                {
                    OnGameStateChangedToMenu?.Invoke();
                    isInvoked = false;
                }

                //Debug.Log("in Menu state");
                //Debug.Log(SceneManager.GetActiveScene().name);

                // if in main game scene, check for space bar input and change game state accordingly
                if (SceneManager.GetActiveScene().name == "3_Scene")
                {
                    //Debug.Log("ding1");
                    if (playGame.triggered)
                    {
                        //Debug.Log("ding2");
                        currentGameState = GameState.Playing;
                        isInvoked = true;
                    }
                }

                break;


            case GameState.Playing:
                if (isInvoked)
                {
                    OnGameStateChangedToPlaying?.Invoke();
                    isInvoked = false;
                }

                //Debug.Log(" in Playing state");

                // if pauseGame triggered, (= ESC or P pressed)
                // pause game
                // change currentGameState to Paused
                if (pauseGame.triggered)
                {
                    currentGameState = GameState.Paused;
                    isInvoked = true;
                    //Debug.Log("pausing game");
                }

                break;


            case GameState.Paused:
                if (isInvoked)
                {
                    OnGameStateChangedToPaused?.Invoke();
                    isInvoked = false;
                }

                //Debug.Log("in pause state");

                // if pauseGame triggered, (= ESC or P pressed)
                // resume playing
                // change currentGameState to Playing
                if (pauseGame.triggered)
                {
                    currentGameState = GameState.Playing;
                    isInvoked = true;
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


                if (quitGame.triggered)
                {
                    //newGameState = GameState.Over;
                    //Debug.Log("Going back to Main menu");
                    isInvoked = true;
                    newGameState = GameState.Menu;
                    SceneManager.LoadScene("3_Scene");
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
*/
