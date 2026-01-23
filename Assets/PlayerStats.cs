using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public ShipData shipData;

    [Header("Chỉ số thực tế")]
    public int currentLevel = 1;
    public int currentDamage;
    public int currentHealth;
    public int maxHealth;

    // Biến này để PlayerShooting đọc tốc độ bắn (Càng cao bắn càng nhanh)
    public float fireRateBonus = 0f;

    void Start()
    {
        currentLevel = 1; // Reset về 1 khi chơi mới
        CalculateStats(); // Tính chỉ số lần đầu
        currentHealth = maxHealth;

        // Cập nhật UI ngay khi vào game
        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateHealthBar(currentHealth, maxHealth);
            UIManager.instance.UpdateLevel(currentLevel);
        }
    }

    // Hàm tính toán chỉ số dựa trên Level
    void CalculateStats()
    {
        // Công thức RPG: 
        // Damage = Gốc + (Cấp * 5)
        currentDamage = shipData.baseDamage + ((currentLevel - 1) * 5);

        // Máu = Gốc + (Cấp * 20)
        maxHealth = shipData.baseHealth + ((currentLevel - 1) * 20);

        // Tốc độ bắn: Cứ mỗi cấp giảm 0.01s thời gian chờ (tức là bắn nhanh hơn)
        fireRateBonus = (currentLevel - 1) * 0.01f;
    }

    public void LevelUp()
    {
        currentLevel++;
        CalculateStats(); // Tính lại chỉ số mới

        // Phần thưởng khi lên cấp: Hồi đầy máu
        currentHealth = maxHealth;

        Debug.Log("LÊN CẤP " + currentLevel + "! Damage mới: " + currentDamage);

        // Cập nhật lại toàn bộ UI
        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateLevel(currentLevel);
            UIManager.instance.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Cập nhật thanh máu khi bị bắn
        if (UIManager.instance != null)
            UIManager.instance.UpdateHealthBar(currentHealth, maxHealth);

        if (currentHealth <= 0) Die();
    }

    void Die()
    {
        if (UIManager.instance != null) UIManager.instance.ShowGameOver();
        gameObject.SetActive(false);
    }
}