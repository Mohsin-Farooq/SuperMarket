using UnityEngine;
using System;

public static class CameraEventManager
{
    
    public static event Action<Transform> OnCameraLerpTriggered;  
    public static void TriggerCameraLerp(Transform cameraTr)
    {
        OnCameraLerpTriggered?.Invoke(cameraTr);
    }
}
