using UnityEngine;

public interface IDraggable
{
    void StartDrag(Vector3 inputPosition,int fingerID);
    void Drag(Vector3 inputPosition,int fingerID);
    void EndDrag(int fingerID);
}