using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;

public class BrickGenerator : MonoBehaviour
{

    public GameObject brickObject;
    private int row;
    private int col;
    private int midCol;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        row = 8;
        col = 13;
        midCol = 7;

        int[] columns = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 }; // columns for the grid
        Debug.Log("print statement here");
        printRowCol(row, col);


        createBricks(brickObject.transform);
    }

    public void printRowCol(int row, int col)
    {
        for (int i = 1; i <= row; i++) // row
        {
            for (int j = midCol; j <= col; j++) // col
            {


            }
        }

    }





    public void createBricks(Transform refPos)
    {
        Vector3 currentPos = refPos.position; // position of the original object
        int n = (col - 1) / 2;
        int count = 0;
        float brick_thickness = 0.6f;


        for (int j = 0; j < row; j++)
        {
            for (int i = -n; i <= n; i++)
            {
                currentPos = refPos.position + new Vector3(i * 2, -j * brick_thickness, 0);
                Instantiate(brickObject, currentPos, Quaternion.identity);
                count++;
            }

        }
        Debug.Log("total bricks created: " + count);

    }
    // Update is called once per frame
    void Update()
    {

    }
}
