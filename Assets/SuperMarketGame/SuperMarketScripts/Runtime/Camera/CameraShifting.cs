using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShifting : MonoBehaviour
{
    public float lerpDuration = 5f;

    private void OnEnable()
    {
        CameraEventManager.OnCameraLerpTriggered += LerpToPosition;
    }

    private void OnDisable()
    {   
        CameraEventManager.OnCameraLerpTriggered -= LerpToPosition;
    }

  
    private void LerpToPosition(Transform cameraTargetTr)
    { 
        StartCoroutine(LerpCamera(cameraTargetTr));
    }

    private IEnumerator LerpCamera(Transform cameraTargetTr)
    {
        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;

        float elapsedTime = 0f;

        while (elapsedTime < lerpDuration)
        {
            elapsedTime += Time.deltaTime;

          
            transform.position = Vector3.Lerp(startPosition, cameraTargetTr.position, elapsedTime / lerpDuration);
            transform.rotation = Quaternion.Lerp(startRotation, cameraTargetTr.rotation, elapsedTime / lerpDuration);

            yield return null; 
        }

      
        transform.position = cameraTargetTr.position;
        transform.rotation = cameraTargetTr.rotation;
    }
}
