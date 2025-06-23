using UnityEngine;

public class ScannerInputHandler : ITouchInputHandler
{
    private readonly IDraggable dragHandler;

    public ScannerInputHandler(IDraggable dragHandler)
    {
        this.dragHandler = dragHandler;
    }

    public void HandleInput()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    dragHandler.StartDrag(touch.position);
                    break;

                case TouchPhase.Moved:
                    dragHandler.Drag(touch.position);
                    break;

                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    dragHandler.EndDrag();
                    break;
            }
        }
    }
}
