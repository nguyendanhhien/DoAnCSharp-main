using System.Collections;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public enum BossState { WaitingToEnter, BulletHell, Laser }
    public BossState currentState;

    [Header("Cài đặt")]
    public GameObject bulletPrefab;
    public GameObject laserObject;

    // Biến đếm số lần bắn đạn thường
    private int attackCounter = 0;

    void Start()
    {
        currentState = BossState.WaitingToEnter;
        StartCoroutine(BossBehaviorLoop());
    }

    IEnumerator BossBehaviorLoop()
    {
        // 1. CHỜ BOSS BAY VÀO VỊ TRÍ
        // Đợi cho đến khi Boss bay xuống thấp hơn Y=4
        while (transform.position.y > 4f)
        {
            yield return null;
        }

        // Đợi thêm 1 giây cho ổn định đội hình rồi mới bắt đầu đánh
        yield return new WaitForSeconds(1f);

        // Bắt đầu vòng lặp chiến đấu
        while (true)
        {
            // --- LOGIC QUYẾT ĐỊNH CHIÊU THỨC ---
            // Nếu chưa bắn đủ 3 đợt đạn thường -> Bắn đạn thường
            if (attackCounter < 3)
            {
                currentState = BossState.BulletHell;
            }
            // Nếu đã đủ 3 đợt -> Bắn Laser
            else
            {
                currentState = BossState.Laser;
            }

            switch (currentState)
            {
                case BossState.BulletHell:
                    // Bắn 3 vòng đạn liên tiếp
                    for (int i = 0; i < 3; i++)
                    {
                        SpawnBulletCircle(12); // 12 viên/vòng
                        yield return new WaitForSeconds(0.8f); // Nghỉ giữa các vòng bắn
                    }

                    // Bắn xong combo thì nghỉ 2 giây
                    yield return new WaitForSeconds(2f);

                    attackCounter++; // Tăng biến đếm lên 1
                    break;

                case BossState.Laser:
                    // --- TUYỆT CHIÊU LASER ---
                    Debug.Log("Cảnh báo Laser!");

                    // Nhấp nháy cảnh báo 1 giây trước khi bắn (để người chơi né)
                    // (Bạn có thể thêm hiệu ứng nháy đỏ ở đây nếu muốn)
                    yield return new WaitForSeconds(1f);

                    // BẬT LASER
                    if (laserObject != null) laserObject.SetActive(true);

                    // --- SỬA Ở ĐÂY: CHỈ BẮN 1 GIÂY RỒI TẮT ---
                    yield return new WaitForSeconds(1f);

                    // TẮT LASER
                    if (laserObject != null) laserObject.SetActive(false);

                    // Reset biến đếm về 0 để quay lại quy trình bắn đạn thường
                    attackCounter = 0;

                    // Nghỉ 2 giây sau khi bắn Laser rồi mới đánh tiếp
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