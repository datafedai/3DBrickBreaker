using UnityEngine;

public class tmp : MonoBehaviour
{
/*



public class MySingleton : MonoBehaviour
{
    public static MySingleton Instance { get; private set; }

    private void Awake()
    {
        // Check if an instance already exists and it's not this one.
        if (Instance != null && Instance != this)
        {
            // Destroy the duplicate instance.
            Destroy(gameObject);
        }
        else
        {
            // Assign this instance as the Singleton.
            Instance = this;
            // Optionally, prevent the Singleton from being destroyed on scene changes.
            // DontDestroyOnLoad(gameObject);
        }
    }
}

```rb.AddForce(new Vector3(10, 0, 0), ForceMode.Force);```



```
//using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;

using UnityEngine.UI;

using UnityEngine.WSA;
using JetBrains.Annotations;

public class PaddleController : MonoBehaviour
{
    public GameManager gameManager;
    InputAction moveAction;
    public int moveSpeed;
    private float posX;
    private float posY;
    private float posZ;
    private float wall;
    private bool canMove = false;

    void OnEnable()
    {
        GameManager.Instance.OnGameStateChangedToPlaying += OnPlaying;
    }

    void OnDisable()
    {
        GameManager.Instance.OnGameStateChangedToPlaying -= OnPlaying;
    }

    void OnPlaying()
    {
        canMove = true;
    }

    private void movePaddle()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        float moveDistance = moveValue.x * Time.deltaTime * moveSpeed;
        Vector3 newPos = transform.position + new Vector3(moveDistance, 0, 0);
        newPos.x = Mathf.Clamp(newPos.x, -wall, wall);
        transform.position = newPos;

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        moveSpeed = 20;
        wall = 12f;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove) // check if not in Pause state
        {
            //Debug.Log("Paddle can move: " + gameManager.getCanMovePaddle());
            movePaddle();
        }
    }
}
```


```
// Events

    public event Action OnGameStateChangedToPlaying;
    public event Action OnGameStateChangedToPaused;
    public event Action OnGameStateChangedToOver;
    public event Action OnGameStateChangedToMenu;
```




```
        switch (currentGameState)
        {
            case GameState.Menu:
                OnGameStateChangedToMenu?.Invoke();

                break;


            case GameState.Playing:
                OnGameStateChangedToPlaying?.Invoke();

                break;
```






















*/
}
