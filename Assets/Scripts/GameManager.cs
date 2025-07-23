using TMPro;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEditor.Experimental.GraphView;


public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI displayState; // for GameState text
    public TextMeshProUGUI displayState2; // for special menu
    public TextMeshProUGUI displayInstruction; // special menu scene text
    private GameState currentGameState;
    private static GameState nextGameState; // A 'static' variable carries information through different scenes.
    private static GameState prevGameState; // previous game state

    InputAction playGame;
    InputAction pauseGame;
    InputAction quitGame;
    private bool canMovePaddle; // to control paddle move when paused

    public GameObject ballObject;
    Rigidbody ballRB;
    private const float gameOverThresholdY = -7f;

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
                // enable Paddle Control
                canMovePaddle = true;

                //display menu state
                displayState.text = "Main Menu";

                //Debug.Log(SceneManager.GetActiveScene().name);

                // if in main game scene, check for space bar input and change game state accordingly
                if (SceneManager.GetActiveScene().name == "3_Scene")
                {
                    if (playGame.triggered)
                    {
                        currentGameState = GameState.Playing;
                    }
                }
                // if in special temporary scene after quiting game, display appropriate instruction
                // and check user input to choose for Main Menu or Playing of the main game scene 
                else if (SceneManager.GetActiveScene().name == "Menu_Scene")
                {

                    displayInstruction.text = "Press Space Bar to play again.\nPress Q to reurn to Main Menu.";
                    if (prevGameState == GameState.Over)
                    {
                        displayState2.text = "<color=Red>--Game Over--</color>";
                    }

                    if (playGame.triggered)
                    {
                        SceneManager.LoadScene("3_Scene");
                        nextGameState = GameState.Playing;
                    }
                    else if (quitGame.triggered)
                    {
                        SceneManager.LoadScene("3_Scene");
                        nextGameState = GameState.Menu;
                    }

                }

                break;


            case GameState.Playing:
                // make sure that ball is not paused
                unpauseBall();

                // enable PaddleController
                canMovePaddle = true;

                // display Playing
                displayState.text = "Playing";

                // if ball gets lost, game over


                // if pauseGame triggered, (= ESC or P pressed)
                // pause game
                // change currentGameState to Paused
                if (pauseGame.triggered)
                {
                    currentGameState = GameState.Paused;
                    pauseBall();
                }

                // Experimental: Game Over
                Vector3 ballPos = ballObject.transform.position;
                //Vector3 paddlePos = paddleObject.transform.position;
                //Debug.Log("ballY:paddleY = " + ballPos.y + " : " + paddlePos.y);
                if (ballPos.y < gameOverThresholdY)
                {
                    currentGameState = GameState.Over;
                }

                break;


            case GameState.Paused:
                // disable PaddleController
                canMovePaddle = false;

                // display Paused text
                displayState.text = "Paused";

                // pause ball
                pauseBall();

                // if pauseGame triggered, (= ESC or P pressed)
                // resume playing
                // change currentGameState to Playing
                if (pauseGame.triggered)
                {
                    currentGameState = GameState.Playing;
                    //unpauseBall();
                }


                // if quitGame triggered,(= Q pressed)
                // go to Main Menu
                // change currentGameState to Menu
                if (quitGame.triggered)
                {
                    SceneManager.LoadScene("Menu_Scene");
                    currentGameState = GameState.Menu;
                    prevGameState = GameState.Paused;
                    nextGameState = GameState.Playing; // a new state when space bar pressed from Menu_Scene
                }

                break;

            case GameState.Over:
                //Debug.Log("I am over");

                // change scene
                SceneManager.LoadScene("Menu_Scene");
                prevGameState = GameState.Over;
                currentGameState = GameState.Menu;
                nextGameState = GameState.Playing; // a new state when space bar pressed from Menu_Scene

                /*
                                //SceneManager.LoadScene("Menu_Scene");
                                if (quitGame.triggered)
                                {
                                    SceneManager.LoadScene("Menu_Scene");
                                    currentGameState = GameState.Menu;
                                    newGameState = GameState.Playing; // a new state when space bar pressed from Menu_Scene
                                }

                */
                break;


            default:
                break;

        }

    }



    public bool getCanMovePaddle()
    {
        return canMovePaddle;
    }


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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballRB = ballObject.GetComponent<Rigidbody>();

        playGame = InputSystem.actions.FindAction("Confirm");   // space bar
        pauseGame = InputSystem.actions.FindAction("Restart");  // ESC or P
        quitGame = InputSystem.actions.FindAction("Menu");      // Q

        //currentGameState = GameState.Menu;
        displayState.text = "Main Menu";
        displayState2.text = "<color=Red>Special</color>\n<size=70><color=Blue>Scene</color></size>";
        displayInstruction.text = "Press Space Bar for Playing.\nPress Q for the initial Main Menu.";

        canMovePaddle = true;

        // When newGameState and currentGameState are declared, they get the valueof the 1st enum, Menu
        // currentGameState = GameState.Menu, newGameState = GameState.Menu, 
        // so don't need to assign GameState.Menu at Start()

        if (SceneManager.GetActiveScene().name == "3_Scene" && nextGameState == GameState.Playing) // if loaded from the new scene
        {
            currentGameState = GameState.Playing;
        }

        /*
        else if (SceneManager.GetActiveScene().name == "Menu_Scene" && prevGameState == GameState.Over)
        {
            displayState2.text = "---Game Over---";
        }*/

        //Debug.Log("prevGameState: " + prevGameState);
        //Debug.Log("currentGameState: " + currentGameState);
        //Debug.Log("newGameState: " + newGameState);
    }

    // Update is called once per frame
    void Update()
    {
        handleGameState();
    }
}
