using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class BrickGenerator : MonoBehaviour
{
    public GameObject brickObject;
    //private GameObject[,] brickClones = new GameObject[8, 13]; // 13 columns, 8 rows 
    private int numRows;
    private int numCols;
    private float brickHeight;
    private float brickWidth;
    public Material[] materials;
    public bool randomizedBrickColors;
    private int count = 0;


    private int chooseMaterial(int j)
    {
        if (randomizedBrickColors == true)
        {
            return Random.Range(0, 8);
        }
        else
        {
            return j;
        }
    }

    // Method1: 
    // create bricks starting from the middle column 
    // and alternate left and right toward 1st and 13th column
    public void createBricks(Transform refPos)
    {
        Vector3 currentPos = refPos.position; // position of the (middle = original) object
        //int count = 0;

        for (int j = 0; j < numRows; j++) // rows: j=0~7 
        {
            for (int jj = 0; jj < numCols; jj++) // columns: jj=0~12
            {
                // column layout: 1,2,3,4,5,6,[7],8,9,10,11,12,13
                // order of creation: (jj, col) = (0, 7th), (1, 6th), (2, 8th), (3, 5th), (4, 9th), (5, 4th), ....
                // shift index i from the middle as jj changes 0~12:
                // (jj, i) = (0, 0), (1, -1), (2, 1), (3, -2), (4, 2), (5, -3), ....
                // In short, the shift index changes 0, -1, 1, -2, 2, -3, 3, -4, ....
                int i = (int)(Mathf.Pow(-1, jj) * Mathf.Floor((jj + 1) / 2));
                //Debug.Log("jj:i = " + jj + ":" + i);

                // j=0 top row, j=1 2nd row from top, j=2 3rd row from top, etc.
                // As i changes in the order of 0, -1, 1, -2, 2, -3, 3,.... 
                // object is created first in the middle(7th=0*brickWidth), 
                // then 1st left(6th=-1*brickWidth) of middle, then 1st right(8th=1*brickWidth) of middle
                // then 2nd left(5th=-2*brickWidth) of middle, then 2nd right(9th=2*brickWidth) of middle, .....
                currentPos = refPos.position + new Vector3(i * brickWidth, -j * brickHeight, 0);



                GameObject instantiatedObject = Instantiate(brickObject, currentPos, Quaternion.identity);
                Renderer brickRenderer = instantiatedObject.GetComponent<Renderer>();
                brickRenderer.material = materials[chooseMaterial(j)];

                count++;
                //Debug.Log(count + " : " + Random.Range(0, 8));
            }
        }
    }

    void createWINBricks(Transform refPos)
    {
        bool[,] WINBoolMatrix = new bool[,] {
        {true, false, false, false, true, false, true, false, true, false, false, false, true}, // row1
        {true, false, false, false, true, false, true, false, true, true, false, false, true},  // row2
        {true, false, true, false, true, false, true, false, true, true, false, false, true},  // row3
        {true, false, true, false, true, false, true, false, true, false, true, false, true},  // row4
        {true, false, true, false, true, false, true, false, true, false, true, false, true},  // row5        
        {true, false, true, false, true, false, true, false, true, false, false, true, true},  // row6 
        {false, true, false, true, false, false, true, false, true, false, false, true, true},  // row7 
        {false, true, false, true, false, false, true, false, true, false, false, false, true},  // row8         
        };

        Vector3 currentPos = refPos.position; // position of the (middle = original) object
        for (int R = 0; R < numRows; R++) // rows 
        {
            for (int i = -6; i <= 6; i++) // i=0 for the middle column, i<0 for left 6 cols, i>0 for right 6 cols
            {
                //GameObject instantiatedObject = Instantiate(brickObject, currentPos, Quaternion.identity);
                //Renderer brickRenderer = instantiatedObject.GetComponent<Renderer>();
                //brickRenderer.material = materials[chooseMaterial(j)];
                int C = i + 6; // Colums: 0~12
                if (WINBoolMatrix[R, C])
                {
                    currentPos = refPos.position + new Vector3(i * brickWidth, -R * brickHeight, 0);
                    GameObject instantiatedObject = Instantiate(brickObject, currentPos, Quaternion.identity);
                    Renderer brickRenderer = instantiatedObject.GetComponent<Renderer>();
                    brickRenderer.material = materials[chooseMaterial(R)];
                }
            }
        }
    }


    // Method2:
    // create bricks starting from left and right,
    // but brick positions are symetrical to the middle column.
    public void createBricks2(Transform refPos)
    {
        Vector3 currentPos = refPos.position; // position of the (middle = original) object
        int n = (numCols - 1) / 2; // n = number of rows left or right of the middle column

        for (int j = 0; j < numRows; j++) // rows: j=0~7 
        {
            for (int i = -n; i <= n; i++) // columns: i=-6~6, total 13 columns
            {
                // column layout: 1,2,3,4,5,6,[7],8,9,10,11,12,13
                // order of creation: (jj, col) = (-6, 1st column), (-5, 2nd), (-4, 3rd), (-3, 4th), 
                // (-2, 5th), (-1, 6th), (0, 7th), (1, 8th), ...., (6, 13th column)

                // j=0 top row, j=1 2nd row from top, j=2 3rd row from top, etc.
                // As i changes in the order of -6, -5, -4, ..., 0, 1, ...., 5, 6 
                // object is created first in the far left, then 2nd left, ..., and far right.          
                currentPos = refPos.position + new Vector3(i * brickWidth, -j * brickHeight, 0);
                Instantiate(brickObject, currentPos, Quaternion.identity);
            }

        }

    }


    void findBrickClone()
    {
        GameObject[] foundObject = GameObject.FindGameObjectsWithTag("BrickClone");
        if (foundObject != null)
        {

            for (int i = 0; i < foundObject.Length; i++)
            {
                Debug.Log("Found object: " + foundObject[i].name + " : " + i);

                //Debug.Log("Found the object: " + foundObject[0].name);
            }
        }
        else
        {
            Debug.Log("Object not found!");
        }

    }

    public void cheatToDestroyAllBricks()
    {
        Debug.Log("Cheat: destroy all bricks");
        GameObject[] foundObject = GameObject.FindGameObjectsWithTag("BrickClone");
        if (foundObject != null)
        {

            for (int i = 0; i < foundObject.Length; i++)
            {
                Debug.Log("Found object: " + foundObject[i].name + " : " + i);

                //Debug.Log("Found the object: " + foundObject[0].name);
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
        numRows = 8;
        numCols = 13;
        brickHeight = 0.6f;
        brickWidth = 2f;

        // creeate bricks in 8 rows x 13 columns using Method1
        //Debug.Log("creating bricks in current scene: " + SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "Win_Scene")
        {
            createWINBricks(brickObject.transform);
        }
        else
        {
            createBricks(brickObject.transform);
        }


    }



    // Update is called once per frame
    void Update()
    {

    }
}
