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
    [Header("Boss Warning")]
    public GameObject bossWarningPanel; // <--- Kéo BossWarningPanel vào đây

    // Hàm này Spawner sẽ gọi khi đủ quái
    public void ShowBossWarning()
    {
        StartCoroutine(WarningRoutine());
    }

    // Coroutine tạo hiệu ứng chớp nháy (Không làm dừng game)
    System.Collections.IEnumerator WarningRoutine()
    {
        // Vòng lặp chớp nháy 3 lần (Bật 0.5s -> Tắt 0.5s)
        for (int i = 0; i < 3; i++)
        {
            if (bossWarningPanel != null) bossWarningPanel.SetActive(true);
            yield return new WaitForSeconds(0.5f); // Đợi nửa giây

            if (bossWarningPanel != null) bossWarningPanel.SetActive(false);
            yield return new WaitForSeconds(0.5f); // Đợi nửa giây
        }
    }
    [Header("Victory Menu")]
    public GameObject victoryPanel; // <--- Kéo VictoryPanel vào đây

    // Hàm này sẽ được gọi khi Boss chết
    public void ShowVictoryPanel()
    {
        StartCoroutine(VictoryRoutine());
    }

    // Coroutine tự động hiện Panel chúc mừng 3 giây rồi tắt
    System.Collections.IEnumerator VictoryRoutine()
    {
        if (victoryPanel != null) victoryPanel.SetActive(true); // Bật chữ chúc mừng

        yield return new WaitForSeconds(3f); // Đợi 3 giây (bằng thời gian Spawner nghỉ)

        if (victoryPanel != null) victoryPanel.SetActive(false); // Tắt chữ đi để bắn tiếp
    }
}