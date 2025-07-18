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


    InputAction moveAction;
    public int moveSpeed;
    private float posX;
    private float posY;

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
        posX = transform.position.x;
        posY = transform.position.y;
        Debug.Log("posX: " + posX);
        if (posX < -12f)
        {
            transform.position = new Vector3(-11.9f, posY, 0);
        }
        else if (posX > 12f)
        {
            transform.position = new Vector3(11.9f, posY, 0);
        }
        transform.Translate(moveValue.x * Time.deltaTime * moveSpeed, 0, 0);



    }
}
