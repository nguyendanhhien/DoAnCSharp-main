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
    private bool isBossPhase = false; 

    
    private Coroutine currentWaveCoroutine;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject); 
    }

    void Start()
    {
        StartNewWave();
    }

    void StartNewWave()
    {
        CancelInvoke("StartNewWave"); 
        if (currentWaveCoroutine != null) StopCoroutine(currentWaveCoroutine); 

        spawnedCount = 0;
        isBossPhase = false;

        if (UIManager.instance != null)
        {
            UIManager.instance.UpdateWave(currentWave);
        }

        
        currentWaveCoroutine = StartCoroutine(SpawnWaveRoutine());
    }

    IEnumerator SpawnWaveRoutine()
    {
        
        while (spawnedCount < totalEnemiesPerWave)
        {
            
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

       

        
        yield return new WaitForSeconds(1f);

        
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowBossWarning();
        }

        
        yield return new WaitForSeconds(3f);

        
        SpawnBoss();
    }

    void SpawnEnemy()
    {
        if (isBossPhase) return; 

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
        
        if (isBossPhase) return;

        
        if (GameObject.FindGameObjectWithTag("Boss") != null) return;
        

        isBossPhase = true; 

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
        
        if (!isBossPhase) return;

        Debug.Log("Boss chết! Chuẩn bị qua màn mới.");

        
        isBossPhase = false;
        
        if (UIManager.instance != null)
        {
            UIManager.instance.ShowVictoryPanel();
        }
        

        difficultyMultiplier += 0.5f;
        currentWave++;

        
        CancelInvoke("StartNewWave");
        Invoke("StartNewWave", 4f);
    }
}