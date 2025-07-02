using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{

    [SerializeField] private CartHandler GamelogicStart;
    public static LevelManager Instance;
    private int currentLevel = 1;
    [SerializeField] private GameObject Cart;
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
        Cart.SetActive(false);
        currentLevel++;
        PlayerPrefs.SetInt("CurrentLevel", currentLevel);
        UIManager.Instance.ShowLevelCompleteUI(OnLevelContinueClicked);
    }

    private void OnLevelContinueClicked()
    {
        CameraTrigger.instacne.TriggerCameraInitialPos();
        StartCoroutine(ReloadLevelSmoothly());
    }

    private IEnumerator ReloadLevelSmoothly()
    {
        yield return new WaitForSeconds(1.2f); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
