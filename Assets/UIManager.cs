using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("UI Components")]
    public Slider healthSlider;     // Thanh máu
    public Text scoreText;          // Điểm số
    public Text waveText;           // <--- MỚI (Hiển thị Wave)
    public Text levelText;          // <--- MỚI (Hiển thị Level)
    public GameObject gameOverPanel; // Bảng thua game

    private int score = 0;

    void Awake()
    {
        if (instance == null) instance = this;
    }

    // --- CẬP NHẬT THANH MÁU ---
    public void UpdateHealthBar(int current, int max)
    {
        if (healthSlider == null) return;
        healthSlider.maxValue = max;
        healthSlider.value = current;
    }

    // --- CẬP NHẬT ĐIỂM SỐ ---
    public void AddScore(int amount)
    {
        if (scoreText == null) return;
        score += amount;
        scoreText.text = "Score: " + score;
    }

    // --- CẬP NHẬT WAVE (MỚI) ---
    public void UpdateWave(int wave)
    {
        // Kiểm tra an toàn trước khi gán
        if (waveText == null) return;

        waveText.text = "WAVE: " + wave;
    }

    // --- CẬP NHẬT LEVEL (MỚI) ---
    public void UpdateLevel(int level)
    {
        // Kiểm tra an toàn trước khi gán
        if (levelText == null) return;

        levelText.text = "Lv. " + level;
    }

    // --- GAME OVER ---
    public void ShowGameOver()
    {
        if (gameOverPanel == null) return;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // --- CHƠI LẠI ---
    public void ReplayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}