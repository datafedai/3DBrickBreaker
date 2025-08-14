using UnityEngine;
using UnityEngine.UIElements;

public class brick : MonoBehaviour
{

    void OnEnable()
    {
        // Subscribe to the ball's BrickDestroyed event
        ball.Instance.ballHitBrick += OnBallHitBrick;        
    }

    void OnDisable()
    {
        // Unsubscribe from the ball's BrickDestroyed event
        ball.Instance.ballHitBrick -= OnBallHitBrick;
    }

    void OnBallHitBrick(GameObject brickObject)
    {
        // Logic to handle when the ball hits a brick
        // For example, you can destroy the brick or change its state
        Debug.Log("Brick hit by ball: " + gameObject.name);
        Rigidbody brickRB = brickObject.GetComponent<Rigidbody>();
        brickRB.isKinematic = false; // Make sure the brick can be affected by physics
        //brickRB.AddForce(Vector3.up * 5f, ForceMode.Impulse); //
        brickRB.useGravity = true;
        brickRB.rotation = Quaternion.Euler(90, 45, 0);
    }




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {

    }
}
