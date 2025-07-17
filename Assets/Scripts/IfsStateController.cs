using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine.UI; // Needed for accessing UI elements like Text
using TMPro;


public class PlayerController : MonoBehaviour
{

    InputAction playGame;
    InputAction quitGame;
    InputAction replayGame;
    private int currentGameState; // 0: main menu, 1: playing, 2: game over


    private TextMeshProUGUI displayText;
    private TextMeshProUGUI displayButtonName;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playGame = InputSystem.actions.FindAction("Confirm"); // space bar
        quitGame = InputSystem.actions.FindAction("Menu"); // q
        replayGame = InputSystem.actions.FindAction("Restart"); // r

        currentGameState = 0;
        displayText.text = "Main Menu";
        displayButtonName.text = "Main Menu";

    }

    // Update is called once per frame
    void Update()
    {

        // three states: main menu, playing, game over
        if (currentGameState == 0) // state: main menu
        {
            if (playGame.triggered) // space bar pressed
            {
                displayText.text = "Playing";
                displayButtonName.text = "Playing";
                currentGameState = 1; // PLAYING                
            }

        }
        else if (currentGameState == 1) // state: playing
        {
            //Debug.Log("quitting game");
            if (quitGame.triggered) // q pressed
            {
                displayText.text = "Game Over";
                displayButtonName.text = "Game Over";
                currentGameState = 2; // GAME OVER                
            }

        }
        else if (currentGameState == 2) // state: game over
        {
            if (replayGame.triggered) // r pressed
            {
                //Debug.Log("replaying game");
                displayText.text = "Playing";
                displayButtonName.text = "Playing";
                currentGameState = 1; // playing                
            }
            else if (quitGame.triggered) // q pressed
            {
                displayText.text = "Main Menu";
                displayButtonName.text = "Main Menu";
                currentGameState = 0; // main menu
            }

        }
    }
}
