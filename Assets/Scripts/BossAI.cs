using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public enum BossState { WaitingToEnter, BulletHell, Laser }
    public BossState currentState;

    [Header("Cài đặt")]
    public GameObject bulletPrefab;
    public GameObject laserObject;

    
    private int attackCounter = 0;

    void Start()
    {
        currentState = BossState.WaitingToEnter;
        StartCoroutine(BossBehaviorLoop());
    }

    IEnumerator BossBehaviorLoop()
    {
        
        while (transform.position.y > 4f)
        {
            yield return null;
        }

        
        yield return new WaitForSeconds(1f);

        
        while (true)
        {
            
            if (attackCounter < 3)
            {
                currentState = BossState.BulletHell;
            }
            
            else
            {
                currentState = BossState.Laser;
            }

            switch (currentState)
            {
                case BossState.BulletHell:
                    
                    for (int i = 0; i < 3; i++)
                    {
                        SpawnBulletCircle(12); 
                        yield return new WaitForSeconds(0.8f); 
                    }

                    
                    yield return new WaitForSeconds(2f);

                    attackCounter++; 
                    break;

                case BossState.Laser:
                    
                    Debug.Log("Cảnh báo Laser!");

                    
                    yield return new WaitForSeconds(1f);

                    
                    if (laserObject != null) laserObject.SetActive(true);

                    
                    yield return new WaitForSeconds(1f);

                    
                    if (laserObject != null) laserObject.SetActive(false);

                    
                    attackCounter = 0;

                    
                    yield return new WaitForSeconds(2f);
                    break;
            }
            yield return null;
        }
    }

    void SpawnBulletCircle(int numberOfBullets)
    {
        float angleStep = 360f / numberOfBullets;
        float angle = 0f;

        for (int i = 0; i < numberOfBullets; i++)
        {
            float bulletDirX = Mathf.Sin((angle * Mathf.PI) / 180f);
            float bulletDirY = Mathf.Cos((angle * Mathf.PI) / 180f);
            Vector2 bulletVector = new Vector2(bulletDirX, bulletDirY);

            GameObject bul = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            EnemyBullet bulletScript = bul.GetComponent<EnemyBullet>();
            if (bulletScript != null)
            {
                bulletScript.SetDirection(bulletVector);
            }
            angle += angleStep;
        }
    }
}