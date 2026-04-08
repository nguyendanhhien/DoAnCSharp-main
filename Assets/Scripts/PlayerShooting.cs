using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform[] firePoints;

    [Header("Cài đặt mặc định")]
    public float normalFireRate = 0.3f; 

    [Header("Animation")]
    public Animator animator;

    private float currentFireRate;
    private bool isFrenzy = false;
    private PlayerStats stats;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        if (animator == null) animator = GetComponent<Animator>();

        currentFireRate = normalFireRate;
        StartCoroutine(AutoShootRoutine());
    }

    public void ActivateFrenzy()
    {
        if (!isFrenzy)
        {
            StartCoroutine(FrenzyRoutine());
        }
    }

    IEnumerator FrenzyRoutine()
    {
        isFrenzy = true;

        
        if (animator != null) animator.SetBool("IsFrenzy", true);

        
        if (stats != null && stats.shipData != null)
        {
            currentFireRate = stats.shipData.frenzyFireRate;
        }

        Debug.Log("BẠO TẨU KÍCH HOẠT!");

        
        yield return new WaitForSeconds(10f);

        isFrenzy = false;

        
        if (animator != null) animator.SetBool("IsFrenzy", false);

        
        currentFireRate = normalFireRate;
        Debug.Log("Hết bạo tẩu.");
    }

    IEnumerator AutoShootRoutine()
    {
        while (true)
        {
            foreach (Transform point in firePoints)
            {
                GameObject bullet = ObjectPool.instance.GetPooledObject();
                if (bullet != null)
                {
                    bullet.transform.position = point.position;
                    bullet.transform.rotation = point.rotation;
                    bullet.SetActive(true);

                    BulletData bulletData = bullet.GetComponent<BulletData>();
                    SpriteRenderer bulletSprite = bullet.GetComponent<SpriteRenderer>();

                    if (stats != null && bulletData != null)
                    {
                        if (isFrenzy)
                        {
                            
                            bulletSprite.sprite = stats.shipData.frenzyBullet;
                            bulletData.SetDamage(stats.currentDamage * 2);
                        }
                        else
                        {
                            
                            bulletSprite.sprite = stats.shipData.normalBullet;
                            bulletData.SetDamage(stats.currentDamage);
                        }
                    }
                }
            }

            

            
            float delayTime = currentFireRate;

            
            if (stats != null)
            {
                delayTime -= stats.fireRateBonus;
            }

            
            float realFireRate = Mathf.Max(0.04f, delayTime);

            yield return new WaitForSeconds(realFireRate);
        }
    }
}