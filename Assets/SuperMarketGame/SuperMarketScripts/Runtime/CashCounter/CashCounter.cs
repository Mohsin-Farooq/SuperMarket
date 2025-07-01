using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CashCounter : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Text amountDueText;    
    [SerializeField] private Text enteredAmountText; 

    [Header("Settings")]
    [SerializeField] private float amountDue = 100.00f; 

    private string enteredAmount = ""; 

    private void Start()
    {
        UpdateDisplay();
    }

    private void OnEnable()
    {
        CashCounterEvent.OnAmountUpdate += updateAmountdue;
    }

    private void OnDisable()
    {
        CashCounterEvent.OnAmountUpdate -= updateAmountdue;
    }

    private void updateAmountdue(float amount)
    {
        amountDue = amount;
        amountDueText.text = $"Amount Due: ${amountDue:F2}";
    }

    private void UpdateDisplay()
    {
       
        enteredAmountText.text = $"Entered: ${enteredAmount}";
    }

    public void OnKeyPress(string key)
    {
        Debug.Log("keypress");
        if (key == "." && enteredAmount.Contains(".")) return;
 
        enteredAmount += key;
        UpdateDisplay();
    }

    public void OnBackspacePress()
    {
        if (enteredAmount.Length > 0)
        {
            enteredAmount = enteredAmount.Substring(0, enteredAmount.Length - 1);
            UpdateDisplay();
        }
    }

    public void OnConfirmPress()
    {
        if (float.TryParse(enteredAmount, out float enteredValue) && Mathf.Approximately(enteredValue, amountDue))
        {
            Debug.Log("Payment Successful!");
            PaymentSuccessful();
        }
        else
        {
            Debug.Log("Incorrect Amount! Please try again.");
        }
    }

    private void PaymentSuccessful()
    {
        enteredAmount = "";
        UpdateDisplay();
    }
}

