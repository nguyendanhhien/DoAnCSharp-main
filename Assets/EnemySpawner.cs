using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    [Header("Cài đặt Prefab")]
    public GameObject enemyPrefab;
    public GameObject bossPrefab;

    [Header("Logic Wave")]
    public int totalEnemiesPerWave = 20;
    public float difficultyMultiplier = 1.0f;
    public int currentWave = 1;

    private int spawnedCount = 0;
    private bool isBossPhase = false; // Biến này rất quan trọng để chặn spawn

    // Biến để lưu trữ Coroutine đang chạy, giúp ta tắt nó đi khi cần
    private Coroutine currentWaveCoroutine;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject); // Đảm bảo không có 2 Spawner trùng nhau
    }

    void Start()
    {
        StartNewWave();
    }

    void StartNewWave()
    {
        // 1. Dọn dẹp các lệnh cũ để tránh chồng chéo
        CancelInvoke("StartNewWave"); // Hủy các lệnh Invoke đang chờ (nếu có)
        if (currentWaveCoroutine != null) StopCoroutine(currentWaveCoroutine); // Dừng Coroutine cũ ngay lập tức

        spawnedCount = 0;
        isBossPhase = false;

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateWave(currentWave);
        }

        // 2. Lưu lại Coroutine đang chạy vào biến
        currentWaveCoroutine = StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        // Vòng lặp spawn quái nhỏ
        while (spawnedCount < totalEnemiesPerWave)
        {
            // Kiểm tra an toàn: Nếu lỡ vào Boss phase rồi thì dừng ngay quái nhỏ
            if (isBossPhase) yield break;

            int remaining = totalEnemiesPerWave - spawnedCount;
            int burstCount = Random.Range(3, 7);
            burstCount = Mathf.Min(burstCount, remaining);

            for (int i = 0; i < burstCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.3f);
            }

            yield return new WaitForSeconds(Random.Range(3f, 5f));
        }

        // Đợi một chút trước khi ra Boss
        yield return new WaitForSeconds(2f);

        // Gọi Boss
        SpawnBoss();
    }

    void SpawnEnemy()
    {
        if (isBossPhase) return; // Chặn thêm 1 lần nữa cho chắc

        float randomX = Random.Range(-2.2f, 2.2f);
        Vector2 spawnPos = new Vector2(randomX, 12f);

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        EnemyController ec = enemy.GetComponent<EnemyController>();
        if (ec != null)
        {
            ec.baseHealth *= difficultyMultiplier;
        }

        spawnedCount++;
    }

    void SpawnBoss()
    {
        // --- FIX LỖI 3 BOSS: KIỂM TRA KỸ ---
        // 1. Nếu đang là Boss Phase rồi thì không spawn nữa
        if (isBossPhase) return;

        // 2. Kiểm tra trên màn hình xem có con Boss nào đang sống không? (Chống duplicate tuyệt đối)
        if (GameObject.FindGameObjectWithTag("Boss") != null) return;
        // (Lưu ý: Bạn nhớ đặt Tag cho Prefab Boss là "Boss" nhé)

        isBossPhase = true; // Khóa lại ngay lập tức

        Vector2 bossPos = new Vector2(0, 13f);
        GameObject boss = Instantiate(bossPrefab, bossPos, Quaternion.identity);

        EnemyController bossScript = boss.GetComponent<EnemyController>();
        if (bossScript != null)
        {
            bossScript.isBoss = true;
            bossScript.baseHealth *= difficultyMultiplier;
        }

        Debug.Log("BOSS WAVE " + currentWave + " XUẤT HIỆN!");
    }

    public void OnBossDied()
    {
        // --- FIX LỖI ĐA KÍCH HOẠT ---
        // Nếu isBossPhase đã được set về false (tức là đã xử lý chết rồi), thì không làm gì nữa
        if (!isBossPhase) return;

        Debug.Log("Boss chết! Chuẩn bị qua màn mới.");

        // Tắt cờ Boss Phase để đánh dấu là Boss đã xong
        isBossPhase = false;

        difficultyMultiplier += 1.0f;
        currentWave++;

        // Hủy mọi lệnh Invoke cũ trước khi gọi lệnh mới (để tránh spam)
        CancelInvoke("StartNewWave");
        Invoke("StartNewWave", 4f);
    }
}