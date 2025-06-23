using UnityEngine;

public interface IDraggable
{
    void StartDrag(Vector3 inputPosition);
    void Drag(Vector3 inputPosition);
    void EndDrag();
}