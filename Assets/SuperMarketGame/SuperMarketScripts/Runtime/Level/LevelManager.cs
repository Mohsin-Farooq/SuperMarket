using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private CartHandler GamelogicStart;
    public static LevelManager Instance;
    private int currentLevel = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("CurrentLevel", 1);
        UIManager.Instance.ShowLevelStartUI(currentLevel, OnLevelIntroComplete);
    }

    private void OnLevelIntroComplete()
    {
        GamelogicStart.enabled = true;
    }

    public void CompleteLevel()
    {
        currentLevel++;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        UIManager.Instance.ShowLevelCompleteUI(OnLevelContinueClicked);
    }

    private void OnLevelContinueClicked()
    {
        StartCoroutine(ReloadLevelSmoothly());
    }

    private IEnumerator ReloadLevelSmoothly()
    {
        yield return new WaitForSeconds(0.5f); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
