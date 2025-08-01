using UnityEngine;
using System;

using UnityEngine.UI;
using UnityEngine.Events;
using Random = UnityEngine.Random;


public class ball : MonoBehaviour
{

    //public GameObject paddleObject;
    private Rigidbody ballRB;
    private bool gameOnPlaying;
    private bool gameOnPause;
    public float initialForce;
    private bool isBallLaunched;
    public bool isBallMissed;
    private int destroyedBrickCount;


    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("collision collider: " + collision.collider);
        //Debug.Log("collision gameObject: " + collision.gameObject.name);
        //Debug.Log("gameObject: " + gameObject.name);
        if (collision.gameObject.name == "Brick(Clone)" || collision.gameObject.name == "Brick")
        {
            Destroy(collision.gameObject);
            //Debug.Log(collision.gameObject.name + " destroyed.");
            destroyedBrickCount++;
            //Debug.Log(destroyedBrickCount + " bricks destroyed.");
        }


        else if (collision.collider.name == "Paddle")
        {

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
        /*
        else if (collision.collider.name == "LWall")
        {
            if (ballRB.linearVelocity.y < 0.1f && ballRB.linearVelocity.y > -0.1f)
            {
                //Debug.Log("Ball barely has verticcal movement");
                //Debug.Log(ballRB.linearVelocity.y);

            }
        } 
        */
    }
    void Awake()
    {
        ballRB = GetComponent<Rigidbody>();
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
    }

    void OnPause()
    {
        //Debug.Log("setting gameOnPause to true");
        //Debug.Log("gameOnPlaying: " + gameOnPlaying);
        //Debug.Log("gameOnpause: " + gameOnPause);
        gameOnPause = true;
        gameOnPlaying = false;

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

    void initializeBall()
    {
        ballRB.transform.position = new Vector3(0f, 5f, 0f);
        ballRB.linearVelocity = Vector3.zero;        
    }

    public int getDestroyedBricksCount()
    {
        return destroyedBrickCount;
    } 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ballRB.linearVelocity = Vector3.zero;
        isBallLaunched = false;
        isBallMissed = false;
        initialForce = 10f;
        //Debug.Log("gameOnPlaying: " + gameOnPlaying);
        //Debug.Log("gameOnpause: " + gameOnPause);

        destroyedBrickCount = 0;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isBallLaunched && gameOnPlaying)
        {
            launchBall();
            isBallLaunched = true;
        }

        if (gameOnPause)
        {
            //Debug.Log("game paused");
            pauseGame();

        }

        if (ballRB.transform.position.y < -5f) // if under paddle position
        {
            //Debug.Log("The ball is missed. Game Over");
            isBallMissed = true;
        }



    }
}
