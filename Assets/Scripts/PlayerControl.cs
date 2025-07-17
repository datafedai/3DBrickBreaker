using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{


    InputAction moveAction;
    public int moveSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        moveSpeed = 20;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();
        if (moveValue.x == -1f)
        {
            Debug.Log("left arrow pressed");
        }
        else if (moveValue.x == 1f)
        {
            Debug.Log("right arrow pressed");
        }

        // move paddle left or right
        transform.Translate(moveValue.x * Time.deltaTime * moveSpeed, 0, 0);

    }
}
