using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SuperMarketGame
{
    public class CashCounter : MonoBehaviour
    {
        public enum InputMode { Card, Cash }
        [Header("Settings")]
        public InputMode currentMode = InputMode.Card;

        [SerializeField] private Text amountDueText;
        [SerializeField] private Text enteredAmountText;
        [SerializeField] private float amountDue = 100f;
        [SerializeField] private float MaxCashStackLimit = 10;
        [SerializeField] private float MaxCoinStackLimit = 25f;

        [SerializeField] private GameObject SubCoinStack, SubCashStack;

        private string enteredText = "";
        private float enteredAmount = 0f;
        private bool isCompleted = false;


        private Stack<float> cashHistory = new Stack<float>();
        private Stack<float> coinHistory = new Stack<float>();

        private void Start()
        {
            UpdateAmountDueDisplay();
            UpdateEnteredAmountDisplay();
        }

        private void OnEnable()
        {
            CashCounterEvent.OnAmountUpdate += UpdateAmountDue;
        }

        private void OnDisable()
        {
            CashCounterEvent.OnAmountUpdate -= UpdateAmountDue;
        }

        private void UpdateAmountDue(float amount)
        {
            amountDue = amount;
            UpdateAmountDueDisplay();
        }

        private void UpdateAmountDueDisplay()
        {
            amountDueText.text = $"${amountDue}";
        }

        private void UpdateEnteredAmountDisplay()
        {
            enteredAmount = Mathf.Round(enteredAmount * 100f) / 100f;
            enteredAmountText.text = $"{enteredAmount}";


            if (currentMode == InputMode.Card) // TO SHOW "." ON THE DISPLAY
                enteredAmountText.text = enteredText;
        }

        // ================= CARD MODE ===================
        public void OnKeyPress(string key)
        {
            if (currentMode != InputMode.Card || isCompleted) return;

            if (key == "." && enteredText.Contains(".")) return;

            enteredText += key;
            AudioManager._instance.PlaySound("Button");

            if (float.TryParse(enteredText, out float result))
                enteredAmount = result;

            UpdateEnteredAmountDisplay();
        }

        public void OnBackspacePress()
        {
            if (currentMode != InputMode.Card || isCompleted) return;

            if (enteredText.Length > 0)
            {
                enteredText = enteredText.Substring(0, enteredText.Length - 1);
                AudioManager._instance.PlaySound("Button");
                if (float.TryParse(enteredText, out float result))
                    enteredAmount = result;
                else
                    enteredAmount = 0f;

                UpdateEnteredAmountDisplay();
            }
        }

        // ================= CASH MODE ===================
        public void OnCashNotePress(float noteValue)
        {
            if (currentMode != InputMode.Cash || isCompleted) return;

            if (cashHistory.Count <= MaxCashStackLimit)
            {

                AudioManager._instance.PlaySound("Cash");
                enteredAmount += noteValue;


                cashHistory.Push(noteValue);  // Track cash addition

                SubCashStack.SetActive(true);

                UpdateEnteredAmountDisplay();

                CheckAutoComplete();
            }
        }

        public void OnCoinPress(float coinValue)
        {
            if (currentMode != InputMode.Cash || isCompleted) return;

            if (coinHistory.Count <= MaxCoinStackLimit)
            {
                AudioManager._instance.PlaySound("Coin");
                enteredAmount += coinValue;
                coinHistory.Push(coinValue);  // Track coin addition

                SubCoinStack.SetActive(true);

                UpdateEnteredAmountDisplay();

                CheckAutoComplete();
            }
        }
        public void OnCashSubtract()
        {
            if (currentMode != InputMode.Cash || isCompleted) return;

            if (cashHistory.Count > 0)
            {
                AudioManager._instance.PlaySound("Cash");
                float lastCash = cashHistory.Pop();
                enteredAmount -= lastCash;
                UpdateEnteredAmountDisplay();

                if (cashHistory.Count == 0)
                {
                    SubCashStack.SetActive(false);
                }
            }

        }

        public void OnCoinSubtract()
        {
            if (currentMode != InputMode.Cash || isCompleted) return;

            if (coinHistory.Count > 0)
            {
                AudioManager._instance.PlaySound("Coin");
                float lastCoin = coinHistory.Pop();
                enteredAmount -= lastCoin;
                UpdateEnteredAmountDisplay();

                if (coinHistory.Count == 0)
                {
                    SubCoinStack.SetActive(false);
                }
            }

        }

        private void CheckAutoComplete()
        {
            if (Mathf.Approximately(enteredAmount, amountDue))
            {
                OnConfirmPress();
            }
        }

        // ================= CONFIRM CHECK ===================
        public void OnConfirmPress()
        {
            if (isCompleted) return;

            if (Mathf.Approximately(enteredAmount, amountDue))
            {
                PaymentSuccessful();
                isCompleted = true;
            }
            else
            {
                Debug.Log("Incorrect Amount! Try again.");
            }
        }

        private void PaymentSuccessful()
        {

            CameraTrigger.instacne.TriggerCameraWhenComplete();
            LevelManager.Instance.CompleteLevel();
        }
    }
}