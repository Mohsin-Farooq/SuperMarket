using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemTouchInputHandler : ITouchInputHandler
{
    private readonly IRotatable rotatableObject;
    private readonly Transform objectTransform;
    private Vector2 previousTouchPosition;
    private bool isRotating;
    private Camera mainCamera;
   
    public ItemTouchInputHandler(IRotatable rotatable, Transform objectTransform)
    {
        rotatableObject = rotatable;
        this.objectTransform = objectTransform;
        mainCamera = Camera.main;
    }

    public void HandleInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (IsTouchingObject(touch.position))
                {
                    previousTouchPosition = touch.position;
                    isRotating = true;
                }
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

    private bool IsTouchingObject(Vector2 inputPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(inputPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.transform == objectTransform;
        }
        return false;
    }
}
