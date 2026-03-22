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

    // --- CÁC BIẾN MỚI CHO TẠM DỪNG ---
    [Header("Pause Menu")]
    public GameObject pausePanel;
    public Slider volumeSlider;
    public AudioSource bgmAudioSource; // Nguồn phát nhạc nền

    private int score = 0;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    void Start()
    {
        // Đồng bộ thanh trượt với âm lượng thực tế lúc mới vào game
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

    // ==========================================
    // --- CÁC HÀM MỚI CHO TẠM DỪNG & ÂM THANH ---
    // ==========================================

    public void PauseGame()
    {
        if (pausePanel != null) pausePanel.SetActive(true); // Hiện Menu
        Time.timeScale = 0f; // Đóng băng thời gian trong game
    }

    public void ResumeGame()
    {
        if (pausePanel != null) pausePanel.SetActive(false); // Ẩn Menu
        Time.timeScale = 1f; // Chạy lại thời gian
    }

    // Hàm này sẽ nhận giá trị tự động từ thanh Slider
    public void SetVolume(float volume)
    {
        if (bgmAudioSource != null)
        {
            bgmAudioSource.volume = volume;
        }
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f; // Rất quan trọng: Phải rã đông thời gian trước khi đổi Scene
        SceneManager.LoadScene("MainMenu"); // Điền tên Scene Menu của bạn vào đây
    }
}