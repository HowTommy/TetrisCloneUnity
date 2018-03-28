using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tetromino : MonoBehaviour
{
    float fall = 0;

    public float fallSpeed = 1;

    public bool allowRotation = true;
    public bool limitRotation = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CheckUserInput();
    }

    void CheckUserInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveTetrominoIfAllowed(1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveTetrominoIfAllowed(-1, 0, 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveTetrominoIfAllowed(0, -1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            RotateTetrominoIfAllowed();
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            while (enabled == true)
            {
                MoveTetrominoIfAllowed(0, -1, 0);
            }
        }

        if (Time.time - fall >= fallSpeed)
        {
            MoveTetrominoIfAllowed(0, -1, 0);
            fall = Time.time;
        }
    }

    private void RotateTetrominoIfAllowed()
    {
        if (!allowRotation)
        {
            return;
        }

        if (!limitRotation)
        {
            transform.Rotate(0, 0, -90);

            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
            }
            else
            {
                transform.Rotate(0, 0, 90);
            }
            return;
        }

        var currentRotationZ = transform.rotation.eulerAngles.z;
        if (currentRotationZ >= 90)
        {
            transform.Rotate(0, 0, -currentRotationZ);

            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
            }
            else
            {
                transform.Rotate(0, 0, currentRotationZ);
            }
        }
        else
        {
            transform.Rotate(0, 0, 90);

            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
            }
            else
            {
                transform.Rotate(0, 0, -90);
            }
        }
    }

    private void MoveTetrominoIfAllowed(float x, float y, float z)
    {
        transform.position += new Vector3(x, y, z);

        if (CheckIsValidPosition())
        {
            FindObjectOfType<Game>().UpdateGrid(this);
        }
        else
        {
            transform.position += new Vector3(-x, -y, -z);

            // if we are trying to move that piece down but can't, it means that we must disable it and spawn another tetromino
            if (y < 0)
            {
                FindObjectOfType<Game>().DeleteRow();

                enabled = false;
                if (!FindObjectsOfType<Tetromino>().Any(e => e.enabled))
                {
                    FindObjectOfType<Game>().SpawnNextTetromino();
                }
            }
        }
    }

    bool CheckIsValidPosition()
    {
        var gameScript = FindObjectOfType<Game>();
        foreach (Transform mino in transform)
        {
            Vector2 roundedPosition = gameScript.Round(mino.position);
            if (gameScript.CheckIsInsideGrid(roundedPosition) == false)
            {
                return false;
            }

            if (FindObjectOfType<Game>().GetTransformAtGridPosition(roundedPosition) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(roundedPosition).parent != transform)
            {
                return false;
            }
        }

        return true;
    }
}
