using UnityEngine;
using System.Collections; 

public class EnemyController : MonoBehaviour
{
    [Header("Phân loại")]
    public bool isBoss = false; 

    [Header("Chỉ số cơ bản")]
    public float baseHealth = 3f; 
    private float currentHealth;
    public int scoreValue = 100;

    [Header("Di chuyển")]
    public float moveSpeed = 4f;
    public float topBoundary = 5f;
    private float stopYPosition;
    private bool reachedTarget = false;

    [Header("Tấn công")]
    public GameObject enemyBulletPrefab;
    public float timeBetweenShots = 2f;
    private float shotTimer;

    [Header("Loot")]
    public GameObject itemSPrefab;
    public GameObject itemLevelUpPrefab; 
    [Range(0f, 1f)] public float dropChance = 0.2f;

    private Transform playerTransform;
    private SpriteRenderer _renderer;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
        _renderer = GetComponent<SpriteRenderer>();

        
        if (isBoss)
            stopYPosition = 3.5f; 
        else
            stopYPosition = Random.Range(1f, 4.5f); 

        
        float multiplier = 1f;
        if (EnemySpawner.instance != null)
        {
            multiplier = EnemySpawner.instance.difficultyMultiplier;
        }

        
        currentHealth = baseHealth * multiplier;

        
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    void HandleMovement()
    {
        
        if (!reachedTarget)
        {
            transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);
            if (transform.position.y <= stopYPosition) reachedTarget = true;
        }
        else
        {
            float newX = transform.position.x + Mathf.Sin(Time.time * 3f) * moveSpeed * Time.deltaTime;
            newX = Mathf.Clamp(newX, -2.5f, 2.5f);
            transform.position = new Vector2(newX, transform.position.y);
        }
    }

    void HandleShooting()
    {
        if (playerTransform == null || transform.position.y > topBoundary) return;

        shotTimer += Time.deltaTime;
        if (shotTimer >= timeBetweenShots)
        {
            ShootAtPlayer();
            shotTimer = 0;
        }
    }

    void ShootAtPlayer()
    {
        if (enemyBulletPrefab != null)
        {
            GameObject bullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
            Vector2 direction = (playerTransform.position - transform.position).normalized;

            EnemyBullet bulletScript = bullet.GetComponent<EnemyBullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(direction);

                
                if (EnemySpawner.instance != null)
                {
                    bulletScript.damage = Mathf.RoundToInt(bulletScript.damage * EnemySpawner.instance.difficultyMultiplier);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (transform.position.y > topBoundary) return; // Bất tử khi chưa vào sân

        if (other.CompareTag("PlayerBullet"))
        {
            BulletData bulletData = other.GetComponent<BulletData>();
            int damage = bulletData != null ? bulletData.damage : 1;
            other.gameObject.SetActive(false);
            TakeDamage(damage);
        }
        else if (other.CompareTag("Player"))
        {
            PlayerStats player = other.GetComponent<PlayerStats>();

            
            float multiplier = (EnemySpawner.instance != null) ? EnemySpawner.instance.difficultyMultiplier : 1f;
            int collisionDamage = Mathf.RoundToInt(20 * multiplier);

            if (player != null) player.TakeDamage(collisionDamage);

            
            if (!isBoss) Die();
        }
    }

    void TakeDamage(float damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashRed());
        if (currentHealth <= 0) Die();
    }

    System.Collections.IEnumerator FlashRed()
    {
        if (_renderer) _renderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        if (_renderer) _renderer.color = Color.white;
    }

    void Die()
    {
        
        if (UIManager.instance != null) UIManager.instance.AddScore(scoreValue);

        
        if (isBoss && EnemySpawner.instance != null)
        {
            EnemySpawner.instance.OnBossDied();
        }

        
        if (Random.value < dropChance) 
        {
            
            if (Random.value < 0.5f)
            {
                if (itemSPrefab != null)
                    Instantiate(itemSPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                if (itemLevelUpPrefab != null)
                    Instantiate(itemLevelUpPrefab, transform.position, Quaternion.identity);
            }
        }

        Destroy(gameObject);
    }
}