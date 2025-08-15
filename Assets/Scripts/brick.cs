using UnityEngine;
using UnityEngine.UIElements;

public class brick : MonoBehaviour
{

    private Animator anim;
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
        //Debug.Log("Brick hit by ball: " + gameObject.name);
        Rigidbody brickRB = brickObject.GetComponent<Rigidbody>();
        //brickRB.isKinematic = false; // Make sure the brick can be affected by physics
        //brickRB.useGravity = true;
        //brickRB.rotation = Quaternion.Euler(90, 45, 0);

        anim = brickObject.GetComponent<Animator>();
        anim.Play("brickAnimation");
    }


    private void FindAndDestoryBrickClone()
    {
        // if fallen bricks are under paddle position, destroy
        GameObject[] foundObject = GameObject.FindGameObjectsWithTag("BrickClone");
        if (foundObject != null)
        {
            foreach (GameObject each in foundObject)
            {
                //Debug.Log("Found object: " + foundObject[i].name + " : " + i);
                //Debug.Log("Found the object: " + foundObject[0].name);
                if (each.transform.position.y < -3f)
                {
                    Destroy(each);
                }
            }

        }
        else
        {
            Debug.Log("Object not found!");
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        FindAndDestoryBrickClone();

    }
}
