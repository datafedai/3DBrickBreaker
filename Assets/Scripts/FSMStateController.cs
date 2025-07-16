using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.UI; // Needed for accessing UI elements like Text
using TMPro;



public class StateController : MonoBehaviour
{


    InputAction playGame; // input for space bar
    InputAction quitGame; // input for Q
    InputAction replayGame; // input for R from keyboard

    public TextMeshProUGUI displayText; // right side of canvas
    //public TextMeshProUGUI displayButtonName; // left side of canvas

    private PlayerState currentState; // current player state



    private enum PlayerState
    {
        Menu,       // main menu state
        Playing,    // playing state
        Over        // game over state
    }

    // handle player state depending on the current state and use input
    private void handlePlayerState()
    {
        switch (currentState)
        {
            case PlayerState.Menu:
                // display menu

                // get user input
                // and change state
                //Debug.Log("in Main Menu");
                if (playGame.triggered) // if space bar pressed, transition to 'playing' state
                {
                    //Debug.Log("SP pressed");
                    displayText.text = "Playing";
                    //displayButtonName.text = "Playing";
                    currentState = PlayerState.Playing;
                }
                break;

            case PlayerState.Playing:
                // playing

                // get user input
                // and change state
                //Debug.Log("in Playing");
                if (quitGame.triggered) // if q pressed, transition to 'game over' state
                {
                    //Debug.Log("Q pressed");
                    displayText.text = "Game Over";
                    //displayButtonName.text = "Game Over";
                    currentState = PlayerState.Over;
                }

                break;

            case PlayerState.Over:
                // game over

                // get user input
                // and change state
                //Debug.Log("in Game Over");
                if (replayGame.triggered) // if r pressed, transition to re'playing' state
                {
                    //Debug.Log("replaying game");
                    //Debug.Log("R pressed");
                    displayText.text = "Playing";
                    //displayButtonName.text = "Playing";
                    currentState = PlayerState.Playing;
                }
                else if (quitGame.triggered) // if q pressed, transition to 'main menu' state
                {
                    //Debug.Log("Q pressed");
                    displayText.text = "Main Menu";
                    //displayButtonName.text = "Main Menu";
                    currentState = PlayerState.Menu;
                }

                break;

            default:
                break;

        }

    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playGame = InputSystem.actions.FindAction("Confirm"); // space bar
        quitGame = InputSystem.actions.FindAction("Menu"); // q
        replayGame = InputSystem.actions.FindAction("Restart"); // r

        displayText.text = "Main Menu";
        //displayButtonName.text = "Main Menu";

        currentState = PlayerState.Menu;
    }



    // Update is called once per frame
    void Update()
    {
        //Debug.Log("current state: " + currentState);
        handlePlayerState();
    }
}
