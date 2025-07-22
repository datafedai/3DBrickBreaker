using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public TextMeshProUGUI displayText; // for GameState text
    public TextMeshProUGUI displayText2; // special menu scene text
    private GameState currentGameState;
    private static GameState newGameState; // 1st enum value is assigned by default

    InputAction playGame;
    InputAction pauseGame;
    InputAction quitGame;
    private bool canMovePaddle; // to control paddle move when paused


    private enum GameState
    {
        Menu,       // main menu: menu only scene
        Playing,    // playing game: can move paddle
        Paused,       // game paused: can't move paddle

    }


    private void handleGameState()
    {
        switch (currentGameState)
        {
            case GameState.Menu:

                // enable PaddleController
                canMovePaddle = true;

                //display a scene for menu
                displayText.text = "Main Menu";
                //displayTextMenuScene.text = "Special Scene";

                Debug.Log(SceneManager.GetActiveScene().name);

                // if playGame triggered, (= Space Bar pressed)
                // launch game
                // change currentGameState to Playing

                Debug.Log(playGame.triggered);

                if (SceneManager.GetActiveScene().name == "3_Scene")
                {
                    if (playGame.triggered)
                    {
                        currentGameState = GameState.Playing;
                    }
                }
                else if (SceneManager.GetActiveScene().name == "Menu_Scene")
                {
                    displayText.text = "Press Space Bar for Playing.\nPress Q for the initial Main Menu.";
                    if (playGame.triggered)
                    {
                        SceneManager.LoadScene("3_Scene");
                        newGameState = GameState.Playing;
                    }
                    else if (quitGame.triggered)
                    {
                        SceneManager.LoadScene("3_Scene");
                        newGameState = GameState.Menu;
                    }
                }




                break;


            case GameState.Playing:
                // load playing scene

                // enable PaddleController
                canMovePaddle = true;

                // display Playing
                displayText.text = "Playing";

                // if ball gets lost, game over


                // if pauseGame triggered, (= ESC or P pressed)
                // pause game
                // change currentGameState to Paused
                if (pauseGame.triggered)
                {
                    currentGameState = GameState.Paused;
                }

                break;


            case GameState.Paused:
                // disable PaddleController
                canMovePaddle = false;

                // display Paused text
                displayText.text = "Paused";



                // if pauseGame triggered, (= ESC or P pressed)
                // resume playing
                // change currentGameState to Playing
                if (pauseGame.triggered)
                {
                    currentGameState = GameState.Playing;
                }


                // if quitGame triggered,(= Q pressed)
                // go to Main Menu
                // change currentGameState to Menu
                if (quitGame.triggered)
                {
                    SceneManager.LoadScene("Menu_Scene");
                    currentGameState = GameState.Menu;
                    newGameState = GameState.Playing; // a new state when space bar pressed from Menu_Scene
                }

                break;


            default:
                break;

        }

    }



    public bool getCanMovePaddle()
    {
        return canMovePaddle;
    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playGame = InputSystem.actions.FindAction("Confirm");   // space bar
        pauseGame = InputSystem.actions.FindAction("Restart");  // ESC or P
        quitGame = InputSystem.actions.FindAction("Menu");      // Q

        //currentGameState = GameState.Menu;
        displayText.text = "Main Menu";
        displayText2.text = "Special\nMenu Scene";

        canMovePaddle = true;

        // When newGameState and currentGameState are declared, they get the valueof the 1st enum, Menu
        // currentGameState = GameState.Menu, newGameState = GameState.Menu, 
        // so don't need to assign GameState.Menu at Start()
        if (SceneManager.GetActiveScene().name == "3_Scene" && newGameState == GameState.Playing) // if loaded from the new scene
        {
            currentGameState = GameState.Playing;
        }

        Debug.Log("newGameState: " + newGameState);
        Debug.Log("currentGameState: " + currentGameState);

    }

    // Update is called once per frame
    void Update()
    {
        handleGameState();
    }
}
