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

	private Vector2 touchPosition;
    private float swipeResistanceX = 50.0f;
    private float swipeResistanceY = 100.0f;

    private void Start()
    {
        instance = this;
        Direction = SwipeDirection.None;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
			touchPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        if (Input.GetMouseButtonUp(0))
        {
			Vector2 touchPosition2 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			Vector2 deltaSwipe = touchPosition - touchPosition2;

            if (Mathf.Abs(deltaSwipe.x) > swipeResistanceX)
            {
                // x axis
                Direction |= deltaSwipe.x < 0 ? SwipeDirection.Left : SwipeDirection.Right;
            }

            if (Mathf.Abs(deltaSwipe.y) > swipeResistanceY)
            {
                // y axis
                Direction |= deltaSwipe.y < 0 ? SwipeDirection.Up : SwipeDirection.Down;
            }
        }
    }

    public bool IsSwiping(SwipeDirection dir)
    {
        var result = (Direction & dir) == dir;
        if (result == true)
        {
            Direction = SwipeDirection.None;
        }
        return result;
    }
}
