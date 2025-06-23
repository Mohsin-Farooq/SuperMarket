using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTouchInputHandler : ITouchInputHandler
{
    private readonly IRotatable rotatableObject; 
    private Vector2 previousTouchPosition;
    private bool isRotating;

    public ItemTouchInputHandler(IRotatable rotatable)
    {
        rotatableObject = rotatable;
    }

    public void HandleInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                previousTouchPosition = touch.position;
                isRotating = true;
            }
            else if (touch.phase == TouchPhase.Moved && isRotating)
            {
                Vector2 delta = touch.position - previousTouchPosition;
                rotatableObject.Rotate(delta); 
                previousTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isRotating = false;
            }
        }
    }
}
