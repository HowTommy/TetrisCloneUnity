using System;
using System.Collections;
using System.Collections.Generic;
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

        if (Time.time - fall >= fallSpeed)
        {
            MoveTetrominoIfAllowed(0, -1, 0);
            fall = Time.time;
        }
    }

    private void RotateTetrominoIfAllowed()
    {
        if(!allowRotation)
        {
            return;
        }

        if (!limitRotation)
        {
            transform.Rotate(0, 0, -90);

            if (!CheckIsValidPosition())
            {
                transform.Rotate(0, 0, 90);
            }
            return;
        }

        var currentRotationZ = transform.rotation.eulerAngles.z;
        if (currentRotationZ >= 90)
        {
            transform.Rotate(0, 0, -currentRotationZ);

            if(!CheckIsValidPosition())
            {
                transform.Rotate(0, 0, currentRotationZ);
            }
        }
        else
        {
            transform.Rotate(0, 0, 90);

            if (!CheckIsValidPosition())
            {
                transform.Rotate(0, 0, -90);
            }
        }
    }

    private void MoveTetrominoIfAllowed(float x, float y, float z)
    {
        transform.position += new Vector3(x, y, z);

        if (!CheckIsValidPosition())
        {
            transform.position += new Vector3(-x, -y, -z);

            // if we try to move down that piece, we disable it + spawn new tetromino
            if (x == 0 && z == 0 && y < 0)
            {
                enabled = false;
                FindObjectOfType<Game>().SpawnNextTetromino();
            }
        }


    }

    bool CheckIsValidPosition()
    {
        var gameScript = FindObjectOfType<Game>();
        foreach (Transform tetromino in transform)
        {
            Vector2 roundedPosition = gameScript.Round(tetromino.position);
            if (gameScript.CheckIsInsideGrid(roundedPosition) == false)
            {
                return false;
            }
        }

        return true;
    }
}
