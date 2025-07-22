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
        if (gameManager.getCanMovePaddle()) // check if not in Pause state
        {
            //Debug.Log("Paddle can move: " + gameManager.getCanMovePaddle());
            movePaddle();
        }
    }
}
