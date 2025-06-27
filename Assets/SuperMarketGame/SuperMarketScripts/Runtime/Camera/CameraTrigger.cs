using System;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public static CameraTrigger instacne;

    public Transform scanningTarget;
    private void Awake()
    {
        instacne = this;
    }

    public void TriggerCameraWhenScan()
    {          
        CameraEventManager.TriggerCameraLerp(scanningTarget);
    }
}
