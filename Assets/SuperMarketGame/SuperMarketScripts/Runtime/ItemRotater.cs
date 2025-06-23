using UnityEngine;

public class SmoothTouchRotate : MonoBehaviour, IRotatable
{
    private ITouchInputHandler touchInputHandler;
    [SerializeField] private float RoationSpeed;

    private void Start()
    {
        touchInputHandler = new ItemTouchInputHandler(this);
    }

    private void Update()
    {
        touchInputHandler.HandleInput();
    }

    public void Rotate(Vector2 delta)
    {
        float rotationX = delta.y * RoationSpeed;
        float rotationY = -delta.x * RoationSpeed;

        transform.Rotate(Vector3.up, rotationY, Space.World);
        transform.Rotate(Vector3.right, rotationX, Space.World);
    }
}
