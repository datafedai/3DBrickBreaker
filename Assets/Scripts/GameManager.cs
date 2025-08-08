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

    //InputAction playGame;
    //InputAction pauseGame;
    //InputAction quitGame;
    public InputSystem_Actions inputActions;
    private Action<InputAction.CallbackContext> spacePerformedAction;
    //private Action<InputAction.CallbackContext> escPerformedAction;
    private Action<InputAction.CallbackContext> pEscPerformedAction;
    private Action<InputAction.CallbackContext> qPerformedAction;

    // cheat
    private Action<InputAction.CallbackContext> bPerformedAction;


    public static GameManager Instance { get; private set; }
    public event Action OnGameStateChangedToMenu;
    public event Action OnGameStateChangedToPlaying;
    public event Action OnGameStateChangedToPaused;
    public event Action OnGameStateChangedToWin;
    public event Action OnGameStateChangedToWinStats;
    public event Action OnGameStateChangedToLose;
    //public event Action WonGame;
    public event Action BrickSelfDestruct;

    public GameObject ballObject;
    ball ballScript;
    //public GameObject brickGeneratorObject;
    //BrickGenerator brickGeneratorScript;

    private enum GameState
    {
        Menu,       // main menu: menu only scene
        Playing,    // playing game: can move paddle
        Paused,       // game paused: can't move paddle
        Win,        // game won: destroyed all the bricks
        WinStats,   // game won stats: after winning, before going back to main menu or playing again
        Lose        // game over: when ball is missed 3 times or game is quitted.
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
        // win
        //ball.Instance.GameWon += YouWonGame;

        OnGameStateChangedToMenu += SwitchToMenuState;
        OnGameStateChangedToPlaying += SwitchToPlayingState;
        OnGameStateChangedToPaused += SwitchToPausedState;
        OnGameStateChangedToWin += SwitchToWinState;
        OnGameStateChangedToWinStats += SwitchToWinStatsState;
        OnGameStateChangedToLose += SwitchToLoseState;
        //inputActions.Player.Confirm.performed += spacePerformedAction = ctx => handleSpacePressed();
        //inputActions.Player.Restart.performed += escPerformedAction = ctx => handleEscOrPPressed();
        //inputActions.Player.Restart.performed += pPerformedAction = ctx => handleEscOrPPressed();
        //inputActions.Player.Menu.performed += qPerformedAction = ctx => handleQPressed();



        // on Menu: space bar
        inputActions.Menu.PlayGame.performed += spacePerformedAction = ctx => StartGameInMenu();

        // on Playing: P or ESC to pause, B to cheat
        inputActions.Playing.PauseGame.performed += pEscPerformedAction = ctx => PauseGame();
        inputActions.Playing.Cheat.performed += bPerformedAction = ctx => CheatGame_SelfDestruct();

        // on Pause: P or esc, Q to quit
        inputActions.Paused.ContinueGame.performed += pEscPerformedAction = ctx => ResumeGame();
        inputActions.Paused.QuitGame.performed += qPerformedAction = ctx => ForfeitGame();

        // on Lose: Q for main menu, space bar to play again
        inputActions.Lose.ReturnToMenu.performed += qPerformedAction = ctx => ReturnToMenuFromGameFinished();
        inputActions.Lose.PlayAgain.performed += spacePerformedAction = ctx => RestartGameInMenu();

        // on Win: Q for main menu, space bar to display game stats
        inputActions.Win.ReturnToMenu.performed += qPerformedAction = ctx => ReturnToMenuFromGameFinished();
        inputActions.Win.PlayAgain.performed += spacePerformedAction = ctx => DisplayGameStats();

        // on WinStats: space bar to play again
        inputActions.WinStats.PlayAgain.performed += spacePerformedAction = ctx => RestartGameInMenu();
        inputActions.WinStats.ReturnToMenu.performed += qPerformedAction = ctx => ReturnToMenuFromGameFinished();

        // enable input action Menu map
        inputActions.Enable(); // This enables menu controls

        // Play Again Button Clicks
        //canvas.Instance.ClickedYes += PlayAgainYes;
        //canvas.Instance.ClickedNo += PlayAgainNo;

    }
    void OnDisable()
    {
        // win
        //ball.Instance.GameWon -= YouWonGame;

        // Play Again Button Clicks
        //canvas.Instance.ClickedYes -= PlayAgainYes;
        //canvas.Instance.ClickedNo -= PlayAgainNo;

        inputActions.Disable();
        inputActions.Menu.PlayGame.performed -= spacePerformedAction = ctx => StartGameInMenu();
        inputActions.Playing.PauseGame.performed -= pEscPerformedAction = ctx => PauseGame();
        inputActions.Playing.Cheat.performed -= bPerformedAction = ctx => CheatGame_SelfDestruct();
        inputActions.Paused.ContinueGame.performed -= pEscPerformedAction = ctx => ResumeGame();
        inputActions.Paused.QuitGame.performed -= qPerformedAction = ctx => ForfeitGame();

        // on Lose: Q for main menu, space bar to play again
        inputActions.Lose.ReturnToMenu.performed -= qPerformedAction = ctx => ReturnToMenuFromGameFinished();
        inputActions.Lose.PlayAgain.performed -= spacePerformedAction = ctx => RestartGameInMenu();

        // on Win: Q for main menu, space bar to display game stats
        inputActions.Win.ReturnToMenu.performed -= qPerformedAction = ctx => ReturnToMenuFromGameFinished();
        inputActions.Win.PlayAgain.performed -= spacePerformedAction = ctx => DisplayGameStats();

        // on WinStats: space bar to play again
        inputActions.WinStats.PlayAgain.performed -= spacePerformedAction = ctx => RestartGameInMenu();
        inputActions.WinStats.ReturnToMenu.performed -= qPerformedAction = ctx => ReturnToMenuFromGameFinished();
        
        //inputActions.Player.Confirm.performed -= spacePerformedAction = ctx => handleSpacePressed();
        //inputActions.Player.Restart.performed -= escPerformedAction = ctx => handleEscOrPPressed();
        //inputActions.Player.Restart.performed -= pPerformedAction = ctx => handleEscOrPPressed();
        //inputActions.Player.Menu.performed -= qPerformedAction = ctx => handleQPressed();
    }


    void DisplayGameStats()
    {
        Debug.Log("Displaying game stats");
        if (SceneManager.GetActiveScene().name == "Win_Scene")
        {
            OnGameStateChangedToWinStats?.Invoke();
        }

    }


    void CheatGame_SelfDestruct()
    {
        Debug.Log("Invoking to destroy all bricks.");
        BrickSelfDestruct?.Invoke();

    }


    void CheatGame_FoundObjects()
    {
        Debug.Log("You will won! Destroying all bricks.");
        //DestroyAllBricks?.Invoke();
        //SceneManager.LoadScene("Win_Scene");
        GameObject[] foundBrickObject = GameObject.FindGameObjectsWithTag("BrickClone");
        //Debug.Log("Found " + foundObject.Length + " bricks in the scene.");
        if (foundBrickObject != null)
        {
            for (int i = 0; i < foundBrickObject.Length; i++)
            {
                //Debug.Log("Destroying object: " + foundObject[i].name + " : " + i);
                Destroy(foundBrickObject[i]);
            }
        }
        else
        {
            Debug.Log("Object not found!");
        }           
    }



    void SwitchToMenuState()
    {
        inputActions.Disable(); // Disables ALL other states
        inputActions.Menu.Enable(); // The only enables playing, so that only one state is active a time
    }

    void SwitchToPlayingState()
    {
        inputActions.Disable(); // Disables ALL other states
        inputActions.Playing.Enable(); // The only enables playing, so that only one state is active a time
    }

    void SwitchToPausedState()
    {
        inputActions.Disable(); // Disables ALL other states
        inputActions.Paused.Enable(); // The only enables playing, so that only one state is active a time
    }

    void SwitchToWinState()
    {
        inputActions.Disable(); // Disables ALL other states
        inputActions.Win.Enable(); // The only enables playing, so that only one state is active a time
        //inputActions.WinStats.Enable(); 
    }

    void SwitchToWinStatsState()
    {
        inputActions.Disable();
        inputActions.WinStats.Enable(); // The only enables playing, so that only one state is active a time
    }


    void SwitchToLoseState()
    {
        inputActions.Disable(); // Disables ALL other states
        inputActions.Lose.Enable(); // The only enables playing, so that only one state is active a time
    }



    void StartGameInMenu()
    {
        if (SceneManager.GetActiveScene().name == "Main_Scene")
        {
            //Debug.Log("in startgameinmenu 1");
            OnGameStateChangedToPlaying?.Invoke();
            currentGameState = GameState.Playing;
        }
        else if (SceneManager.GetActiveScene().name == "Win_Scene" || SceneManager.GetActiveScene().name == "Lose_Scene")
        {
            //Debug.Log("in startgameinmenu 2");
            SceneManager.LoadScene("Main_Scene");
            newGameState = GameState.Playing;
        }

    }



    void PauseGame()
    {
        OnGameStateChangedToPaused?.Invoke();
        currentGameState = GameState.Paused;
    }

    void ResumeGame()
    {
        OnGameStateChangedToPlaying?.Invoke();
        currentGameState = GameState.Playing;
    }

    void ForfeitGame()
    {
        //Debug.Log("Q pressed 2");
        OnGameStateChangedToLose?.Invoke();
        //currentGameState = GameState.Over;
        SceneManager.LoadScene("Lose_Scene");
    }


    void RestartGameInMenu()
    {
        OnGameStateChangedToPlaying?.Invoke();
        currentGameState = GameState.Playing;
        newGameState = GameState.Playing;
        SceneManager.LoadScene("Main_Scene");
    }

    void ReturnToMenuFromGameFinished() // A finished game can either be Game Win or Game Over
    {
        //Debug.Log("Q pressed 1");
        if (SceneManager.GetActiveScene().name == "Win_Scene" || SceneManager.GetActiveScene().name == "Lose_Scene")
        {
            newGameState = GameState.Menu;
            SceneManager.LoadScene("Main_Scene");
        }
    }


    void findBrickClone()
    {
        GameObject[] foundObject = GameObject.FindGameObjectsWithTag("BrickClone");
        if(foundObject != null)
        {
            for (int i = 0; i < foundObject.Length; i++)
            {
                Debug.Log("Found object: " + foundObject[i].name);
            }
            //Debug.Log("Found the object: " + foundObject[0].name);
        }
        else
        {
            Debug.Log("Object not found!");
        }        
    }

    int remainingBricksCount()
    {
        GameObject[] foundObject = GameObject.FindGameObjectsWithTag("BrickClone");
        //Debug.Log("Found " + foundObject.Length + " bricks in the scene.");
        return foundObject.Length;
    }
    


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        //Debug.Log("starting... a new scene");
        //Debug.Log("new game state: " + newGameState);
        //Debug.Log("current state: " + currentGameState);

        //isInvoked = true;
        //Debug.Log("isInvoked set to true on Start");
        //ballRB = ballObject.GetComponent<Rigidbody>();
        /*
                playGame = InputSystem.actions.FindAction("Confirm");   // space bar
                pauseGame = InputSystem.actions.FindAction("Restart");  // ESC or P
                quitGame = InputSystem.actions.FindAction("Menu");      // Q
        */

        // When newGameState and currentGameState are declared, they get the value of the 1st enum, Menu
        // currentGameState = GameState.Menu, newGameState = GameState.Menu, 
        // so don't need to assign GameState.Menu at Start()

        //Debug.Log("current scene: " + SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "Main_Scene")
        {
            // initial game start
            if (newGameState == GameState.Menu)
            {
                currentGameState = GameState.Menu;
                //Debug.Log("Invoking OnGameStateChangedToMenu");
                OnGameStateChangedToMenu?.Invoke();
            }
            // restart from Win_Scene or Lose_Scene
            else if (newGameState == GameState.Playing)
            {
                //Debug.Log("Invoking OnGameStateChangedToPlaying");
                OnGameStateChangedToPlaying?.Invoke();
                currentGameState = GameState.Playing;
            }
            /*
            else if (newGameState == GameState.Playing)
            {
                currentGameState = GameState.Playing;
                OnGameStateChangedToPlaying?.Invoke();
            }*/
        }
        else if (SceneManager.GetActiveScene().name == "Lose_Scene")
        {
            OnGameStateChangedToLose?.Invoke();
            currentGameState = GameState.Lose;
        }
        else if (SceneManager.GetActiveScene().name == "Win_Scene")
        {
            //WonGame?.Invoke();
            OnGameStateChangedToWin?.Invoke();
            //currentGameState = GameState.Over;
            //Debug.Log("current game state in Win_Scene: " + currentGameState);
            //Debug.Log("new game state in Win_Scene: " + newGameState);
        }

        //Debug.Log("newGameState: " + newGameState);
        //Debug.Log("currentGameState: " + currentGameState);

        ballScript = ballObject.GetComponent<ball>();


    }

    // Update is called once per frame
    void Update()
    {
        // find brick clones
        //findBrickClone();

        //Debug.Log(ballObject.transform.position.y);
        //Debug.Log("current state: " + currentGameState);

        // if ball is missed while game is not over, that is on playing
        if (currentGameState != GameState.Lose && ballScript.isBallMissed)
        {
            if (ballScript.canContinueToPlay())
            {
                ballScript.resetIsBallLaunched();
                currentGameState = GameState.Playing;
            }
            else
            {
                currentGameState = GameState.Lose;
                SceneManager.LoadScene("Lose_Scene");
                //Debug.Log("Total " + ballScript.getDestroyedBricksCount() + " brickes destroyed.");
            }

        }

        // check if all the bricks are destroyed

        //if (ballScript.getDestroyedBricksCount() == 105)
        if (remainingBricksCount() == 0)
        {
            //Debug.Log("You won!");
            //OnGameStateChangedToPlaying?.Invoke();
            SceneManager.LoadScene("Win_Scene");
        }




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
