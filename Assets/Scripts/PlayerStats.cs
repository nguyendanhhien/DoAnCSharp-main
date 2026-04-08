using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public ShipData shipData;

    [Header("Chỉ số thực tế")]
    public int currentLevel = 1;
    public int currentDamage;
    public int currentHealth;
    public int maxHealth;

    
    public float fireRateBonus = 0f;

    void Start()
    {
        currentLevel = 1; 
        CalculateStats(); 
        currentHealth = maxHealth;

        
        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateHealthBar(currentHealth, maxHealth);
            UIManager.instance.UpdateLevel(currentLevel);
        }
    }

    
    void CalculateStats()
    {
        
        currentDamage = shipData.baseDamage + ((currentLevel - 1) * 5);

        
        maxHealth = shipData.baseHealth + ((currentLevel - 1) * 20);

        
        fireRateBonus = (currentLevel - 1) * 0.01f;
    }

    public void LevelUp()
    {
        currentLevel++;
        CalculateStats(); 

        
        currentHealth = maxHealth;

        Debug.Log("LÊN CẤP " + currentLevel + "! Damage mới: " + currentDamage);

        
        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateLevel(currentLevel);
            UIManager.instance.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        
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