using System.Collections;
using UnityEngine;

public class ScannerDrag : IDraggable
{
    private readonly Transform scannerTransform;
    private readonly Camera mainCamera;
    private readonly float dragSmoothSpeed;
    private  MonoBehaviour coroutineRunner;

    private float smoothSpeed = 50f;
    private Vector3 initialPosition;
    private Vector3 dragOffset;
    private bool isDragging = false;
    private int activeFingerId = -1;
    private Coroutine snapBackCoroutine;
    private Plane dragPlane;
    public ScannerDrag(Transform transform, Camera camera, float smoothSpeed, MonoBehaviour coroutineHost)
    {
        scannerTransform = transform;
        mainCamera = camera;
        dragSmoothSpeed = smoothSpeed;
        coroutineRunner = coroutineHost;

        initialPosition = scannerTransform.position;
    }

    public void StartDrag(Vector3 inputPosition, int fingerId)
    {
        coroutineRunner.enabled = true;

        if (isDragging) return;

        Ray ray = mainCamera.ScreenPointToRay(inputPosition);
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.transform == scannerTransform)
        {
           
            
            // Ray ray = mainCamera.ScreenPointToRay(inputPosition);
            // if (Physics.Raycast(ray, out RaycastHit hit))
            // {
            //     Vector3 targetPosition = hit.point + dragOffset;
            //     targetPosition.z = fixedZ;
            //     scannerTransform.position = Vector3.Lerp(scannerTransform.position, targetPosition, lerpSpeed);
            // }
            
            dragPlane = new Plane(Vector3.forward, new Vector3(0, 0, initialPosition.z));

            if (dragPlane.Raycast(ray, out float distance))
            {
                Vector3 worldPoint = ray.GetPoint(distance);
                dragOffset = scannerTransform.position - worldPoint;

                isDragging = true;
                activeFingerId = fingerId;

                if (snapBackCoroutine != null)
                    coroutineRunner.StopCoroutine(snapBackCoroutine);
            }
        }
    }

    public void Drag(Vector3 inputPosition, int fingerId)
    {
        if (!isDragging || fingerId != activeFingerId) return;

        Ray ray = mainCamera.ScreenPointToRay(inputPosition);
        if (dragPlane.Raycast(ray, out float distance))
        {
            Vector3 worldPoint = ray.GetPoint(distance);
            Vector3 targetPosition = worldPoint + dragOffset;

            targetPosition.z = initialPosition.z; // Keep Z locked
            

            float lerpSpeed = Time.deltaTime * dragSmoothSpeed;
            scannerTransform.position = Vector3.Lerp(scannerTransform.position, targetPosition, lerpSpeed);
        }
    }

    public void EndDrag(int fingerId)
    {
        if (!isDragging || fingerId != activeFingerId) return;

        isDragging = false;
        activeFingerId = -1;

        snapBackCoroutine = coroutineRunner.StartCoroutine(SmoothSnapBack());
    }

    private IEnumerator SmoothSnapBack()
    {
        Vector3 startPos = scannerTransform.position;
        float duration = 0.3f;
        float time = 0f;

        while (time < duration)
        {
            scannerTransform.position = Vector3.Lerp(startPos, initialPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        scannerTransform.position = initialPosition;
    }
    
    
    
}
