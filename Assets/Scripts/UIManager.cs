using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Components")]
    public Slider healthSlider;
    public Text scoreText;
    public Text waveText;
    public Text levelText;
    public GameObject gameOverPanel;

    
    [Header("Pause Menu")]
    public GameObject pausePanel;
    public Slider volumeSlider;
    public AudioSource bgmAudioSource; 

    private int score = 0;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        
        if (bgmAudioSource != null && volumeSlider != null)
        {
            volumeSlider.value = bgmAudioSource.volume;
        }
    }

    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        if (scoreText != null) scoreText.text = "" + score;
    }

    public void UpdateWave(int wave)
    {
        if (waveText != null) waveText.text = "WAVE: " + wave;
    }

    public void UpdateLevel(int level)
    {
        if (levelText != null) levelText.text = "Lv. " + level;
    }

    public void ShowGameOver()
    {
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ReplayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

   

    public void PauseGame()
    {
        if (pausePanel != null) pausePanel.SetActive(true); 
        Time.timeScale = 0f; 
    }

    public void ResumeGame()
    {
        if (pausePanel != null) pausePanel.SetActive(false); 
        Time.timeScale = 1f; 
    }

    
    public void SetVolume(float volume)
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = volume;
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MainMenu"); 
    }
    [Header("Boss Warning")]
    public GameObject bossWarningPanel; 

    
    public void ShowBossWarning()
    {
        StartCoroutine(WarningRoutine());
    }

    
    System.Collections.IEnumerator WarningRoutine()
    {
        
        for (int i = 0; i < 3; i++)
        {
            if (bossWarningPanel != null) bossWarningPanel.SetActive(true);
            yield return new WaitForSeconds(0.5f); 

            if (bossWarningPanel != null) bossWarningPanel.SetActive(false);
            yield return new WaitForSeconds(0.5f); 
        }
    }
    [Header("Victory Menu")]
    public GameObject victoryPanel; 

    
    public void ShowVictoryPanel()
    {
        StartCoroutine(VictoryRoutine());
    }

    
    System.Collections.IEnumerator VictoryRoutine()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true); 

        yield return new WaitForSeconds(3f); 

        if (victoryPanel != null) victoryPanel.SetActive(false); 
    }
}