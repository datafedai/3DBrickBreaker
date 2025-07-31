using UnityEngine;

public class ball : MonoBehaviour
{


    private Rigidbody ballRB;
    private bool gameOnPlaying;
    private bool gameOnPause;
    private bool isBallLaunched;
    public bool isBallMissed;



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
        ballRB.AddForce(new Vector3(0f, -1f, 0f) * 10f, ForceMode.Impulse);
    }

    void pauseGame()
    {
        Time.timeScale = 0;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //ballRB.linearVelocity = Vector3.zero;
        isBallLaunched = false;
        isBallMissed = false;
        //Debug.Log("gameOnPlaying: " + gameOnPlaying);
        //Debug.Log("gameOnpause: " + gameOnPause);

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

        if (ballRB.transform.position.y < -5f)
        {
            Debug.Log("The ball is missed. Game Over");
            isBallMissed = true;
        }
    }
}
