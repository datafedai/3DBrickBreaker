using UnityEngine;
using System;

using UnityEngine.UI;
using UnityEngine.Events;
using Random = UnityEngine.Random;
//using UnityEngine.SceneManagement;
using System.Collections;

public class ball : MonoBehaviour
{

    //public GameObject paddleObject;
    private Rigidbody ballRB;
    private bool gameOnPlaying;
    private bool gameOnPause;
    public float initialForce;
    private bool isBallLaunched;
    public bool isBallMissed;
    private int ballLives;
    static private int destroyedBrickCount;

    public static ball Instance { get; private set; }
    public event Action<int> BrickDestroyed;
    public event Action<int> BallLives;
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
        //Debug.Log("collision collider: " + collision.collider);
        //Debug.Log("collision gameObject: " + collision.gameObject.name);
        //Debug.Log("gameObject: " + gameObject.name);
        if (collision.gameObject.name == "Brick(Clone)" || collision.gameObject.name == "Brick")
        {
            Destroy(collision.gameObject);
            collisionAudioSource.Play();
            //Debug.Log(collision.gameObject.name + " destroyed.");

            destroyedBrickCount++;
            BrickDestroyed?.Invoke(destroyedBrickCount);

            //Debug.Log(destroyedBrickCount + " bricks destroyed.");
        }


        else if (collision.collider.name == "Paddle")
        {
            collisionAudioSource.Play();
            
            paddle pd = collision.collider.GetComponent<paddle>();
            //Debug.Log("Ball hit Paddle");
            Vector2 paddleMoveValue = pd.getMoveValue();    // +1 for right arrow, -1 for left arrow
            //Debug.Log("ball velocity1: " + ballRB.linearVelocity);
            //Debug.Log("paddle move value: " + paddleMoveValue.x);
            //Debug.Log("ball linear velocity: " + ballRB.linearVelocity);
            //Debug.Log("paddle move value: " + paddleMoveValue.x);
            //Debug.Log("paddle move speed: " + paddleCont.moveSpeed);
            float paddleSpeedInfluence = pd.moveSpeed * 0.1f;   // default paddle speed: 20, 

            // if the paddle is moving while ball hits it, 
            // the horizontal speed of the ball is added by -2 or +2 by default
            ballRB.linearVelocity += new Vector3(paddleSpeedInfluence * paddleMoveValue.x, 0f, 0f);

            //Debug.Log(ballRB.linearVelocityY);
            //Debug.Log("ball velocity2: " + ballRB.linearVelocity);
        }
    }



    void OnEnable()
    {
        //Debug.Log("in OnEnable in Ball");
        GameManager.Instance.OnGameStateChangedToPlaying += OnPlaying;
        GameManager.Instance.OnGameStateChangedToPaused += OnPause;
    }

    void OnDisable()
    {
        //Debug.Log("in OnDisable in Ball");
        GameManager.Instance.OnGameStateChangedToPlaying -= OnPlaying;
        GameManager.Instance.OnGameStateChangedToPaused -= OnPause;
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


    public int getDestroyedBricksCount()
    {
        return destroyedBrickCount;
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
        initialForce = 10f;

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

        destroyedBrickCount = 0;
        timerStarted = true;
        delayTime = 1f;
        timer = 0f;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("timer:1 " + timer);
        if (!isBallLaunched && gameOnPlaying)
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

        if (ballRB.transform.position.y < -5f) // if ball is under paddle position
        {
            if (ballLives > 0)
            {
                Restart();
            }
            else
            {
                isBallMissed = true;
                //gameOnPlaying = true;
            }
            //Debug.Log("The ball is missed. Game Over");

        }

    }
}
