using UnityEngine;
using System.Collections; // Cần thiết để dùng Coroutine (IEnumerator)

public class EnemyController : MonoBehaviour
{
    [Header("Phân loại")]
    public bool isBoss = false; // Spawner sẽ tự tick cái này nếu là Boss

    [Header("Chỉ số cơ bản")]
    public float baseHealth = 3f; // Đổi sang float để nhân % cho dễ
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
    public GameObject itemLevelUpPrefab; // <--- MỚI: Biến chứa Prefab Item Nâng Cấp
    [Range(0f, 1f)] public float dropChance = 0.2f;

    private Transform playerTransform;
    private SpriteRenderer _renderer;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) playerTransform = player.transform;
        _renderer = GetComponent<SpriteRenderer>();

        // --- FIX BIẾN MẤT: Luôn dừng trong màn hình ---
        if (isBoss)
            stopYPosition = 3.5f; // Boss dừng cao hơn chút
        else
            stopYPosition = Random.Range(1f, 4.5f); // Quái nhỏ dừng rải rác

        // --- LOGIC TĂNG ĐỘ KHÓ ---
        // Lấy hệ số từ Spawner (Ví dụ 1.01)
        float multiplier = 1f;
        if (EnemySpawner.instance != null)
        {
            multiplier = EnemySpawner.instance.difficultyMultiplier;
        }

        // Nhân máu lên
        currentHealth = baseHealth * multiplier;

        // (Tùy chọn) Tăng tốc độ bay 1 chút xíu nếu thích khó hơn
        // moveSpeed *= multiplier; 
    }

    void Update()
    {
        HandleMovement();
        HandleShooting();
    }

    void HandleMovement()
    {
        // Logic: Bay xuống -> Đến điểm dừng -> Dừng lại lượn lờ -> KHÔNG BAO GIỜ BIẾN MẤT
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

                // --- TĂNG SÁT THƯƠNG ĐẠN THEO ĐỘ KHÓ ---
                // Sát thương đạn = Mặc định * Hệ số khó
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

            // Sát thương đâm va cũng tăng theo độ khó
            float multiplier = (EnemySpawner.instance != null) ? EnemySpawner.instance.difficultyMultiplier : 1f;
            int collisionDamage = Mathf.RoundToInt(20 * multiplier);

            if (player != null) player.TakeDamage(collisionDamage);

            // Nếu là Boss đâm trúng thì không chết, quái nhỏ đâm thì chết
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
        // Cộng điểm
        if (UIManager.instance != null) UIManager.instance.AddScore(scoreValue);

        // Nếu là Boss chết thì báo cho Spawner
        if (isBoss && EnemySpawner.instance != null)
        {
            EnemySpawner.instance.OnBossDied();
        }

        // --- LOGIC RỚT ĐỒ NGẪU NHIÊN (MỚI) ---
        if (Random.value < dropChance) // Nếu may mắn được rớt đồ
        {
            // Tung đồng xu: 50% ra S, 50% ra LevelUp
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