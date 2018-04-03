using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SwipeDirection
{
    None = 0,
    Left = 1 << 0,
    Right = 1 << 1,
    Up = 1 << 2,
    Down = 1 << 3,
    Touch = 1 << 4,

    LeftUp = 5,
    LeftDown = 9,
    RightUp = 6,
    RightDown = 10
}

public class SwipeManager : MonoBehaviour
{
    private static SwipeManager instance;
    public static SwipeManager Instance { get { return instance; } }

    public SwipeDirection Direction { get; set; }

    private Vector3 touchPosition;
    private Vector3 touchPosition2;
    private float swipeResistanceX = 50.0f;
    private float swipeResistanceY = 100.0f;

    private void Start()
    {
        instance = this;
        Direction = SwipeDirection.None;
    }

    private void Update()
    {
        Direction = SwipeDirection.None;
        bool moved = false;

        if (Input.GetMouseButtonDown(0))
        {
            touchPosition = Input.mousePosition;
        }
        else if (Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Began)
        {
            touchPosition = Input.touches[0].position;
        }

        if (Input.GetMouseButtonUp(0))
        {
            moved = true;
            touchPosition2 = Input.mousePosition;
        }
        else if (Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)
        {
            moved = true;
            touchPosition2 = Input.touches[0].position;
        }

        if (moved)
        {
            Vector2 deltaSwipe = touchPosition - touchPosition2;

            if (Mathf.Abs(deltaSwipe.x) > swipeResistanceX)
            {
                // x axis
                Direction |= deltaSwipe.x < 0 ? SwipeDirection.Right : SwipeDirection.Left;
            }

            if (Mathf.Abs(deltaSwipe.y) > swipeResistanceY)
            {
                // y axis
                Direction |= deltaSwipe.y < 0 ? SwipeDirection.Down : SwipeDirection.Up;
            }

            if (Direction == SwipeDirection.None)
            {
                Direction = SwipeDirection.Touch;
            }
        }
    }

    public bool IsSwiping(SwipeDirection dir)
    {
        return (Direction & dir) == dir;
    }
}
