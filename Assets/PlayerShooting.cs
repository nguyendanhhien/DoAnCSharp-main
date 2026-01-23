using System.Collections;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public Transform[] firePoints;

    [Header("Cài đặt mặc định")]
    public float normalFireRate = 0.3f; // Tốc độ bắn cơ bản

    [Header("Animation")]
    public Animator animator; // Kéo Animator vào đây

    private float currentFireRate; // Biến này sẽ thay đổi giữa Normal và Frenzy
    private bool isFrenzy = false;
    private PlayerStats stats;

    void Start()
    {
        stats = GetComponent<PlayerStats>();
        // Tự động tìm Animator nếu quên kéo
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

        // Tăng tốc độ bắn (Lấy tốc độ siêu nhanh của Frenzy)
        if (stats != null && stats.shipData != null)
        {
            currentFireRate = stats.shipData.frenzyFireRate;
        }

        Debug.Log("BẠO TẨU KÍCH HOẠT! BIẾN HÌNH!");

        // Giữ trạng thái bạo tẩu trong 10 giây
        yield return new WaitForSeconds(10f);

        isFrenzy = false;

        // 2. KÍCH HOẠT ANIMATION TRỞ VỀ (DETRANSFORM)
        if (animator != null) animator.SetBool("IsFrenzy", false);

        // Trả về tốc độ thường
        currentFireRate = normalFireRate;
        Debug.Log("Hết bạo tẩu. Trở về dạng thường.");
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
                            // Đạn hình to + Damage nhân đôi
                            bulletSprite.sprite = stats.shipData.frenzyBullet;
                            bulletData.SetDamage(stats.currentDamage * 2);
                        }
                        else
                        {
                            // --- TRẠNG THÁI THƯỜNG ---
                            // Đạn hình nhỏ + Damage chuẩn theo cấp độ
                            bulletSprite.sprite = stats.shipData.normalBullet;
                            bulletData.SetDamage(stats.currentDamage);
                        }
                    }
                }
            }

            // --- CẬP NHẬT LOGIC CHỜ ĐẠN (MỚI) ---

            // 1. Lấy tốc độ hiện tại (Nếu đang Frenzy thì nó là tốc độ siêu nhanh, nếu thường thì là 0.3)
            float delayTime = currentFireRate;

            // 2. Nếu đang ở chế độ THƯỜNG (không phải Frenzy), ta mới trừ Bonus từ Level
            // (Vì chế độ Frenzy đã bắn max tốc độ rồi, không cần trừ thêm nữa)
            if (!isFrenzy && stats != null)
            {
                delayTime -= stats.fireRateBonus;
            }

            // 3. Giới hạn: Không bao giờ bắn nhanh quá 0.05 giây (để tránh lỗi game/lag máy)
            float realFireRate = Mathf.Max(0.05f, delayTime);

            yield return new WaitForSeconds(realFireRate);
        }
    }
}