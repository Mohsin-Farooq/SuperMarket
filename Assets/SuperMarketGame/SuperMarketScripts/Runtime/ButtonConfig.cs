using UnityEngine;

public class ButtonConfig : MonoBehaviour
{
    [SerializeField] private string key;
    [SerializeField] private CashCounter counter;
    public void TriggerAction()
    {
        if (counter != null)
        {
            if (key == "Backspace")
            {
                counter.OnBackspacePress();
            }
            else if (key == "Confirm")
            {
                counter.OnConfirmPress();
            }
            else
            {
                counter.OnKeyPress(key);
            }
        }
        else
        {
            Debug.LogError("CashCounter script not found in the scene.");
        }
    }
}
