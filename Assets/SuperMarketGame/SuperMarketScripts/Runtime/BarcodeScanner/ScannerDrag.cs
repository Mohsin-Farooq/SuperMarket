using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerDrag : IDraggable
{
    private readonly Transform scannerTransform;
    private readonly Camera mainCamera;
    private readonly float dragSmoothSpeed;
    private readonly float minX, maxX, minY, maxY;

    private float smoothSpeed = 50f; 
    private Vector3 previousPosition;
    private bool isDragging = false;
    private Vector3 dragOffset;
    
  

    public ScannerDrag(Transform transform, Camera camera, float smoothSpeed,float MinX,float MaxX,float MinY, float MaxY)
    {
        scannerTransform = transform;
        mainCamera = camera;
        dragSmoothSpeed = smoothSpeed;
        minX = MinX;
        maxX = MaxX;
        minY = MinY;
        maxY = MaxY;



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
            Vector3 targetPosition = hit.point + dragOffset;

           
            targetPosition.z = scannerTransform.position.z;

          
            //targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            //targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);

           
            float lerpSpeed = Time.deltaTime * smoothSpeed;
            scannerTransform.position = Vector3.Lerp(scannerTransform.position, targetPosition, lerpSpeed);

           
            previousPosition = scannerTransform.position;
        }
    }



    public void EndDrag()
    {
        isDragging = false;
    }
}
