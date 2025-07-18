using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class BrickGenerator : MonoBehaviour
{
    public GameObject brickObject;
    private int numRows;
    private int numCols;
    private float brickHeight;
    private float brickWidth;

    // Method1: 
    // create bricks starting from the middle column 
    // and alternate left and right toward 1st and 13th column
    public void createBricks(Transform refPos)
    {
        Vector3 currentPos = refPos.position; // position of the (middle = original) object

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
                Instantiate(brickObject, currentPos, Quaternion.identity);
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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        numRows = 8;
        numCols = 13;
        brickHeight = 0.6f;
        brickWidth = 2f;
 
        // creeate bricks in 8 rows x 13 columns using Method1
        createBricks(brickObject.transform);
    }



    // Update is called once per frame
    void Update()
    {

    }
}
