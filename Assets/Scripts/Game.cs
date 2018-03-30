using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static int gridWidth = 11;
    public static int gridHeight = 20;

    public static Transform[,] grid = new Transform[gridWidth, gridHeight];

    // Use this for initialization
    void Start()
    {
        SpawnNextTetromino();
    }

    public bool CheckIsAboveGrid(Tetromino tetromino)
    {
        for (int x = 1; x < gridWidth; x++)
        {
            foreach(Transform mino in tetromino.transform)
            {
                Vector2 pos = Round(mino.position);
                if(pos.y > gridHeight)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool IsFullRowAt(int y)
    {
        for (int x = 1; x < gridWidth; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    public void DeleteMinoAtRow(int y)
    {
        for (int x = 1; x < gridWidth; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }

    public void MoveRowDown(int y)
    {
        for (int x = 1; x < gridWidth; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y].position += new Vector3(0, -1, 0);
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
            }
        }
    }

    public void MoveAllRowsDown(int y)
    {
        for (int i = y; i < gridHeight; i++)
        {
            MoveRowDown(i);
        }
    }

    public void DeleteRow()
    {
        for(int y = 0; y < gridHeight; y++)
        {
            if(IsFullRowAt(y))
            {
                DeleteMinoAtRow(y);
                MoveAllRowsDown(y+1);
                y--;
            }
        }
    }

    public void UpdateGrid(Tetromino tetromino)
    {
        for (int y = 0; y < gridHeight; y++)
        {
            for (int x = 0; x < gridWidth; x++)
            {
                if (grid[x, y] != null && grid[x, y].parent == tetromino.transform)
                {
                    grid[x, y] = null;
                }
            }
        }

        foreach (Transform mino in tetromino.transform)
        {
            Vector2 position = Round(mino.position);

            if (position.y < gridHeight)
            {
                grid[(int)position.x, (int)position.y] = mino;
            }
        }
    }

    public bool CheckIsInsideGrid(Vector2 position)
    {
        return ((int)position.x >= 1 && (int)position.x < gridWidth && (int)position.y > 0);
    }

    public Vector2 Round(Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }

    public void SpawnNextTetromino()
    {
        GameObject nextTetromino = (GameObject)Instantiate(Resources.Load("Prefabs/" + GetRandomTetromino(), typeof(GameObject)), new Vector2(5, 20), Quaternion.identity);
    }

    public Transform GetTransformAtGridPosition(Vector2 position)
    {
        if (position.y > gridHeight - 1)
        {
            return null;
        }

        return grid[(int)position.x, (int)position.y];
    }

    string GetRandomTetromino()
    {
        var randomTetrominoId = Random.Range(1, 8);
        switch (randomTetrominoId)
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

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
