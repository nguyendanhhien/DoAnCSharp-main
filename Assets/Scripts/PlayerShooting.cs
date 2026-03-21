using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform[] firePoints;

    [Header("Cài đặt mặc định")]
    public float normalFireRate = 0.3f; // Tốc độ bắn cơ bản

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

        // 1. KÍCH HOẠT ANIMATION BIẾN HÌNH
        if (animator != null) animator.SetBool("IsFrenzy", true);

        // Lấy tốc độ gốc của chế độ Frenzy (thường là 0.1)
        if (stats != null && stats.shipData != null)
        {
            currentFireRate = stats.shipData.frenzyFireRate;
        }

        Debug.Log("BẠO TẨU KÍCH HOẠT!");

        // Giữ trạng thái bạo tẩu trong 10 giây
        yield return new WaitForSeconds(10f);

        isFrenzy = false;

        // 2. KÍCH HOẠT ANIMATION TRỞ VỀ
        if (animator != null) animator.SetBool("IsFrenzy", false);

        // Trả về tốc độ thường
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
                            // --- TRẠNG THÁI BẠO TẨU ---
                            // Đạn hình to + Damage nhân đôi theo cấp độ hiện tại
                            bulletSprite.sprite = stats.shipData.frenzyBullet;
                            bulletData.SetDamage(stats.currentDamage * 2);
                        }
                        else
                        {
                            // --- TRẠNG THÁI THƯỜNG ---
                            bulletSprite.sprite = stats.shipData.normalBullet;
                            bulletData.SetDamage(stats.currentDamage);
                        }
                    }
                }
            }

            // --- CẬP NHẬT LOGIC CHỜ ĐẠN (ĐÃ SỬA) ---

            // 1. Lấy tốc độ gốc (0.3 nếu thường, 0.1 nếu Frenzy)
            float delayTime = currentFireRate;

            // 2. Luôn trừ đi Bonus từ Level (Kể cả khi đang Frenzy)
            // Level càng cao -> fireRateBonus càng lớn -> delayTime càng nhỏ -> Bắn càng nhanh
            if (stats != null)
            {
                delayTime -= stats.fireRateBonus;
            }

            // 3. Giới hạn: Không bao giờ bắn nhanh quá 0.04 giây (25 viên/giây)
            // Để tránh việc Level cao quá thì delayTime bị âm (gây lỗi)
            float realFireRate = Mathf.Max(0.04f, delayTime);

            yield return new WaitForSeconds(realFireRate);
        }
    }
}