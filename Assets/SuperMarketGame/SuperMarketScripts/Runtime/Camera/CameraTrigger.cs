using UnityEngine;

namespace SuperMarketGame
{
    public class CameraTrigger : MonoBehaviour
    {
        public static CameraTrigger instacne;

        public Transform scanningTarget;
        [SerializeField] private Transform BillingTransform, LevelCompleteTransform, CameraStartTransorm;

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
        public void TriggerCameraWhenComplete()
        {

            CameraEventManager.TriggerCameraLerp(LevelCompleteTransform);
        }

        public void TriggerCameraInitialPos()
        {

            CameraEventManager.TriggerCameraLerp(CameraStartTransorm);
        }

    }
}