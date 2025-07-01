using System;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public static CameraTrigger instacne;

    public Transform scanningTarget;
    [SerializeField] private Transform BillingTransform;
    private void Awake()
    {
        instacne = this;
    }

    public void TriggerCameraWhenScan()
    {          
        CameraEventManager.TriggerCameraLerp(scanningTarget);
    }
    public void TriggerCameraWhenBill()
    {
        CameraEventManager.TriggerCameraLerp(BillingTransform);
    }

}
