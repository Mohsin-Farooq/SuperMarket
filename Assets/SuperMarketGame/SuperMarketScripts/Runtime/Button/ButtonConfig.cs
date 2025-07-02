using UnityEngine;

public class ButtonConfig : MonoBehaviour
{
    public enum ButtonType { Key, Backspace, Confirm, CashNote, Coin, CashSubtract, CoinSubtract }

    [Header("Button Type")]
    [SerializeField] private ButtonType buttonType;

    [Header("For Key / Cash / Coin Buttons")]
    [SerializeField] private string key;            // For number keys like "1", "2", etc (For Card Mode)
    [SerializeField] private float cashValue;       // For Cash note buttons (e.g., 5, 10, 20)
    [SerializeField] private float coinValue;       // For Coin buttons (e.g., 1, 2, etc)

    [Header("Reference")]
    [SerializeField] private CashCounter counter;

    public void TriggerAction()
    {
        if (counter == null)
        {
            Debug.LogError("CashCounter reference missing on button: " + gameObject.name);
            return;
        }

        switch (buttonType)
        {
            case ButtonType.Key:
                counter.OnKeyPress(key);
                break;

            case ButtonType.Backspace:
                counter.OnBackspacePress();
                break;

            case ButtonType.Confirm:
                counter.OnConfirmPress();
                break;

            case ButtonType.CashNote:
                Debug.Log("pressed");
                counter.OnCashNotePress(cashValue);
                break;

            case ButtonType.Coin:
                counter.OnCoinPress(coinValue);
                break;

            case ButtonType.CashSubtract:
                counter.OnCashSubtract();
                break;

            case ButtonType.CoinSubtract:
                counter.OnCoinSubtract();
                break;
        }
    }
}