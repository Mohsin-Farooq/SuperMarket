using MoreMountains.NiceVibrations;
using UnityEngine;

namespace SuperMarketGame
{
    public class ButtonManager : MonoBehaviour
    {
        [SerializeField] private LayerMask buttonLayer;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, buttonLayer))
                {
                    ButtonConfig buttonConfig = hit.collider.GetComponent<ButtonConfig>();
                    if (buttonConfig != null)
                    {
                        MMVibrationManager.Haptic(HapticTypes.LightImpact);
                        buttonConfig.TriggerAction();
                    }
                    else
                    {
                        Debug.LogWarning($"No ButtonConfig found on {hit.collider.gameObject.name}");
                    }
                }
            }
        }
    }
}