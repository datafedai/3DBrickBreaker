using UnityEngine;
using System;
//using Unity.Mathematics;

using UnityEngine.UI;
using UnityEngine.Events;
using Random = UnityEngine.Random;
//using UnityEngine.SceneManagement;
using System.Collections;
using Unity.VisualScripting;
//using System.Numerics;

public class ball : MonoBehaviour
{

    //public GameObject paddleObject;
    private Rigidbody ballRB;
    private bool gameOnPlaying;
    private bool gameOnPause;
    public float initialForce;
    private bool isBallLaunched;  // ball status if it is moving on Playing
    public bool isBallMissed;   // ball status if player missed it to hit
    private static int ballLives;
    //static private int destroyedBrickCount;

    public static ball Instance { get; private set; }
    public event Action BallBrickCollision;
    public event Action<int> BallLives;
    public event Action<GameObject> ballHitBrick;


    private float delayTime;
    private float timer;
    private bool timerStarted = false;

    public AudioSource collisionAudioSource;


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

        //inputActions = new InputSystem_Actions();

        ballRB = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        // collision audio
        collisionAudioSource.Play();

        //Debug.Log("collision collider: " + collision.collider);
        //Debug.Log("collision gameObject: " + collision.gameObject.name);
        //Debug.Log("gameObject: " + gameObject.name);
        if (collision.gameObject.name == "Brick(Clone)" || collision.gameObject.name == "Brick")
        {
            ballHitBrick?.Invoke(collision.gameObject);

            //Destroy(collision.gameObject);
            //collisionAudioSource.Play();
            //Debug.Log(collision.gameObject.name + " destroyed.");

            //Debug.Log(destroyedBrickCount + " bricks destroyed. before");
            //destroyedBrickCount++;
            BallBrickCollision?.Invoke();
            //Debug.Log(destroyedBrickCount + " bricks destroyed. after");
        }


        else if (collision.collider.name == "Paddle")
        {
            //collisionAudioSource.Play();

            paddle pd = collision.collider.GetComponent<paddle>();
            //Debug.Log("Ball hit Paddle");
            Vector2 paddleMoveValue = pd.getMoveValue();    // +1 for right arrow, -1 for left arrow

            //Debug.Log("ball velocity1: " + ballRB.linearVelocity);
            //Debug.Log("paddle move value: " + paddleMoveValue.x);
            //Debug.Log("ball linear velocity: " + ballRB.linearVelocity);
            //Debug.Log("paddle move value: " + paddleMoveValue.x);
            //Debug.Log("paddle move speed: " + paddleCont.moveSpeed);
            //float paddleSpeedInfluence = pd.moveSpeed * 0.1f;   // default paddle speed: 20, 

            float extraBallSpeedByPaddleMovement = pd.moveSpeed * 0.1f;   // default paddle speed: 20, 
            // if the paddle is moving while ball hits it, 
            // the horizontal speed of the ball is added by -2 or +2 by default
            ballRB.linearVelocity += new Vector3(extraBallSpeedByPaddleMovement * paddleMoveValue.x, 0f, 0f);

            //Debug.Log(ballRB.linearVelocityY);
            //Debug.Log("ball velocity2: " + ballRB.linearVelocity);
        }
        /*
        else if (collision.collider.name == "LWall" || collision.collider.name == "RWall")
        {
            collisionAudioSource.Play();
        }
        */
    }



    void OnEnable()
    {
        //Debug.Log("in OnEnable in Ball");
        GameManager.Instance.OnGameStateChangedToPlaying += OnPlaying;
        GameManager.Instance.OnGameStateChangedToPaused += OnPause;

        // Play Again Button Clicks
        //canvas.Instance.ClickedYes += PlayAgainYes;
        //canvas.Instance.ClickedNo += PlayAgainNo;
    }

    void OnDisable()
    {
        //Debug.Log("in OnDisable in Ball");
        GameManager.Instance.OnGameStateChangedToPlaying -= OnPlaying;
        GameManager.Instance.OnGameStateChangedToPaused -= OnPause;

        // Play Again Button Clicks
        //canvas.Instance.ClickedYes -= PlayAgainYes;
        //canvas.Instance.ClickedNo -= PlayAgainNo;        
    }

    void PlayAgainYes()
    {
        Debug.Log("Yes Button Clicked ball");
    }

    void PlayAgainNo()
    {

        Debug.Log("No Button Clicked ball");
    }

    void OnPlaying()
    {
        //Debug.Log("setting gameOnPlaying to true");
        //Debug.Log("gameOnPlaying: " + gameOnPlaying);
        //Debug.Log("gameOnpause: " + gameOnPause);
        Time.timeScale = 1;
        gameOnPlaying = true;
        gameOnPause = false;
        isBallLaunched = false;
    }

    void OnPause()
    {
        //Debug.Log("setting gameOnPause to true");


        gameOnPause = true;
        gameOnPlaying = false;

        //Debug.Log("gameOnpause: " + gameOnPause);
        //Debug.Log("gameOnPlaying: " + gameOnPlaying);
    }

    void launchBall()
    {
        //Debug.Log("launching ball");
        //Vector3 launchDirection = new Vector3(1f, 0.01f, 0f).normalized;  // test, straight down
        Vector3 launchDirection = new Vector3(Random.Range(-1f, 1f), -1f, 0f).normalized;  // unit vector with varing direction
        ballRB.AddForce(launchDirection * initialForce, ForceMode.Impulse);
    }

    void pauseGame()
    {
        Time.timeScale = 0;
    }


    // not used, delete
    public int getDestroyedBricksCount()
    {
        return 0; // destroyedBrickCount;
    }

    public int getBallLives()
    {
        return ballLives;
    }

    public bool canContinueToPlay()
    {
        if (ballLives > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void resetIsBallLaunched()
    {
        isBallLaunched = false;
    }


    void Restart()
    {
        //ballLives--;
        //BallLives?.Invoke(ballLives);
        //initializeBall();
        //Debug.Log("Restarted");
        //ballLives--;
        BallLives?.Invoke(ballLives);


        isBallLaunched = false;
        isBallMissed = false;
        //initialForce = 10f;

        ballRB.transform.position = new Vector3(0f, 5f, 0f);
        ballRB.linearVelocity = Vector3.zero;
        //Debug.Log("gameOnPlaying: " + gameOnPlaying);
        //Debug.Log("gameOnpause: " + gameOnPause);

        initTimer();
    }

    void initTimer()
    {
        timerStarted = true;
        timer = 0f;
        delayTime = 2f;
    }


    bool isBallNotWithinGameSpace()
    {
        //Debug.Log(transform.position);

        // ball missed by paddle
        if (transform.position.y < -5f)
        {
            //Debug.Log("hey");
            return true;
        }
        // ball escaped through between wall and ceiling
        else if (transform.position.x < -15f || transform.position.x > 15f)
        {
            //Debug.Log("hey2");
            return true;
        }
        // ball is still inside playing space
        else
        {
            //Debug.Log("hey3");
            return false;
        }
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ballRB.linearVelocity = Vector3.zero;
        ballLives = 3;
        BallLives?.Invoke(ballLives);

        isBallLaunched = false;
        isBallMissed = false;
        initialForce = 10f;
        //Debug.Log("gameOnPlaying: " + gameOnPlaying);
        //Debug.Log("gameOnpause: " + gameOnPause);

        //destroyedBrickCount = 0;
        timerStarted = true;
        delayTime = 1f;
        timer = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        /*
        Vector3 newPos = transform.position + new Vector3(moveDistance, 0, 0);
        newPos.x = Mathf.Clamp(newPos.x, -wall, wall);
        transform.position = newPos;
        */

        // ball velocity vector, to prevent ball from being horizontally bouncing forever
        /*
        Vector3 ballV = ballRB.linearVelocity;
        if (ballV.y < 0)    // when ball is moving downward
        {
            ballV.y = Mathf.Clamp(ballRB.linearVelocity.y, -30, -5);
            ballRB.linearVelocity = ballV;
        }
        else if (ballV.y > 0)   // when ball is moving upward
        {
            ballV.y = Mathf.Clamp(ballRB.linearVelocity.y, 5, 30);
            ballRB.linearVelocity = ballV;
        }
        */


        //Debug.Log("timer:1 " + timer);
        if (gameOnPlaying && !isBallLaunched)
        {
            //Debug.Log("timer:2 " + timer);
            if (timerStarted)
            {
                timer += Time.deltaTime;
                //Debug.Log("timer:3 " + timer);
                if (timer > delayTime)
                {
                    launchBall();
                    ballLives--;
                    BallLives?.Invoke(ballLives);
                    isBallLaunched = true;
                    timerStarted = false;
                }
            }
        }

        if (gameOnPause)
        {
            //Debug.Log("game paused");
            pauseGame();

        }

        //Vector3 newPos = transform.position;
        //newPos.x = Mathf.Clamp(newPos.x, -14f, 14f);
        //transform.position = newPos;

        if (isBallNotWithinGameSpace()) // check if ball is not inside game playing space
        {
            if (ballLives > 0)
            {
                Restart();
            }
            else
            {
                isBallMissed = true; // ball missed or escaped
                //gameOnPlaying = true;
            }
            //Debug.Log("The ball is missed. Game Over");

        }

    }
}
