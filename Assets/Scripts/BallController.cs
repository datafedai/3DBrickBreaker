using UnityEngine;

public class BallController : MonoBehaviour
{


    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("gameObject: " + collision.gameObject.name);
        Debug.Log("collider: " + collision.collider.name);
        
        if (collision.gameObject.name == "Brick(Clone)" || collision.gameObject.name == "Brick")
        {
            Destroy(collision.gameObject);
        }

        //Debug.Log(collision.otherCollider.name);
    }


    // Start is
    //  called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}


