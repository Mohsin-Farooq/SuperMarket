using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Level Start UI")]
    [SerializeField] private CanvasGroup levelStartUIPanel;
    [SerializeField] private Text levelText;

    [Header("Level Complete UI")]
    [SerializeField] private CanvasGroup levelCompleteUIPanel;
    [SerializeField] private Button continueButton;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float delayBeforeFade = 1f;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    public void ShowLevelStartUI(int levelNumber, Action onComplete)
    {
        levelText.text = $"Level {levelNumber}";
        SetCanvasGroupVisible(levelStartUIPanel, true, 0f);
        StartCoroutine(FadeCanvasGroup(levelStartUIPanel, 0f, 1f, fadeDuration, 0f, () =>
        {
            StartCoroutine(FadeCanvasGroup(levelStartUIPanel, 1f, 0f, fadeDuration, delayBeforeFade, () =>
            {
                levelStartUIPanel.gameObject.SetActive(false);
                onComplete?.Invoke();
            }));
        }));
    }
    public void ShowLevelCompleteUI(Action onContinue)
    {
       
        SetCanvasGroupVisible(levelCompleteUIPanel, true, 0f); 

        StartCoroutine(FadeCanvasGroup(levelCompleteUIPanel, 0f, 1f, fadeDuration, delayBeforeFade, () =>
        {
            continueButton.interactable = true;
            continueButton.onClick.RemoveAllListeners();
           
            continueButton.onClick.AddListener(() =>
            {
                StartCoroutine(FadeCanvasGroup(levelCompleteUIPanel, 1f, 0f, fadeDuration, 0f, () =>
                {
                    levelCompleteUIPanel.gameObject.SetActive(false);
                    onContinue?.Invoke();
                }));
            });
        }));
    }
    private void SetCanvasGroupVisible(CanvasGroup canvas, bool visible, float alpha)
    {
        canvas.gameObject.SetActive(visible);
        canvas.alpha = alpha;
        canvas.blocksRaycasts = visible;
        canvas.interactable = visible;
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvas, float from, float to, float duration, float delay, Action onComplete)
    {
        yield return new WaitForSeconds(delay);

        float t = 0f;
        canvas.alpha = from;

        while (t < duration)
        {
            t += Time.deltaTime;
            canvas.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }

        canvas.alpha = to;
        canvas.blocksRaycasts = to > 0;
        canvas.interactable = to > 0;

        onComplete?.Invoke();
    }
}
