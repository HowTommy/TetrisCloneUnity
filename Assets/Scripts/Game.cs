using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static int gridWidth = 10;
    public static int gridHeight = 20;

    // Use this for initialization
    void Start()
    {
        SpawnNextTetromino();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckIsInsideGrid(Vector2 position)
    {
        return ((int)position.x >= 1 && (int)position.x - 1 < gridWidth && (int)position.y > 0);
    }

    public Vector2 Round(Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }

    public void SpawnNextTetromino()
    {
        GameObject nextTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/" + GetRandomTetromino(), typeof(GameObject)), new Vector2(5, 20), Quaternion.identity);
    }

    string GetRandomTetromino()
    {
        var randomTetrominoId = Random.Range(1, 8);
        switch(randomTetrominoId)
        {
            case 1:
                return "Tetromino_T";
            case 2:
                return "Tetromino_Z";
            case 3:
                return "Tetromino_Square";
            case 4:
                return "Tetromino_S";
            case 5:
                return "Tetromino_Long";
            case 6:
                return "Tetromino_L";
            case 7:
                return "Tetromino_J";
        }

        throw new System.Exception();
    }
}
