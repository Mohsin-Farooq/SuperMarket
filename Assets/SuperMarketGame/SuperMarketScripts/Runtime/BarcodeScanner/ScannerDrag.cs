using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerDrag : IDraggable
{
    private readonly Transform scannerTransform;
    private readonly Camera mainCamera;
    private readonly float dragSmoothSpeed;
    private readonly float fixedZ;
    private float smoothSpeed = 15f; 
    private Vector3 previousPosition;
    private bool isDragging = false;
    private Vector3 dragOffset;

    public ScannerDrag(Transform transform, Camera camera, float smoothSpeed, float zOffset)
    {
        scannerTransform = transform;
        mainCamera = camera;
        dragSmoothSpeed = smoothSpeed;
        fixedZ = zOffset;
    }

    public void StartDrag(Vector3 inputPosition)
    {
        Ray ray = mainCamera.ScreenPointToRay(inputPosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == scannerTransform)
        {
            dragOffset = scannerTransform.position - hit.point;
            isDragging = true;
        }
    }

    public void Drag(Vector3 inputPosition)
    {
        if (!isDragging) return;

        Ray ray = mainCamera.ScreenPointToRay(inputPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            
            Vector3 targetPosition = hit.point ;
            targetPosition.z = scannerTransform.position.z;

            scannerTransform.position = Vector3.Lerp(scannerTransform.position, targetPosition, Time.deltaTime * smoothSpeed);

            previousPosition = scannerTransform.position;
        }
    }

    public void EndDrag()
    {
        isDragging = false;
    }
}
