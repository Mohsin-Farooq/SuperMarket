using UnityEngine;

namespace SuperMarketGame
{
    public class ScannerInputHandler : ITouchInputHandler
    {
        private readonly IDraggable dragHandler;
        private int activeFingerId = -1;

        public ScannerInputHandler(IDraggable dragHandler)
        {
            this.dragHandler = dragHandler;
        }
        public void HandleInput()
        {
            if (Input.touchCount > 0)
            {
                foreach (Touch touch in Input.touches)
                {
                    switch (touch.phase)
                    {
                        case TouchPhase.Began:
                            if (activeFingerId == -1)
                            {
                                dragHandler.StartDrag(touch.position, touch.fingerId);
                                activeFingerId = touch.fingerId;
                            }
                            break;

                        case TouchPhase.Moved:
                            if (touch.fingerId == activeFingerId)
                            {
                                dragHandler.Drag(touch.position, touch.fingerId);
                            }
                            break;

                        case TouchPhase.Ended:
                        case TouchPhase.Canceled:
                            if (touch.fingerId == activeFingerId)
                            {
                                dragHandler.EndDrag(touch.fingerId);
                                activeFingerId = -1;
                            }
                            break;
                    }
                }
            }
        }
    }
}