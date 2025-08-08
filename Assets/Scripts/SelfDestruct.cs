using UnityEngine;

public class SelfDestruct : MonoBehaviour
{

    void OnEnable()
    {
        //Debug.Log("in Onable in SelfDestruct");
        GameManager.Instance.BrickSelfDestruct += OnBrickSelfDestruct;        
    }

    void OnDisable()
    {
        GameManager.Instance.BrickSelfDestruct -= OnBrickSelfDestruct;
    }

    void OnBrickSelfDestruct()
    {
        Debug.Log("SelfDestruct called. Destroying a brickself one by one");
        // Destroy this game object
        Destroy(gameObject);
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
