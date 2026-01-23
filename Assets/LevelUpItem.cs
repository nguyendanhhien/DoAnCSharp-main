using UnityEngine;

public class LevelUpItem : MonoBehaviour
{
    public float speed = 3f; // Tốc độ rơi của vật phẩm

    void Update()
    {
        // Bay từ từ xuống dưới
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        // Nếu bay quá màn hình (Y < -12) thì tự hủy cho đỡ nặng máy
        if (transform.position.y < -12f) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu chạm vào Player
        if (other.CompareTag("Player"))
        {
            // Tìm script PlayerStats trên người chơi
            PlayerStats stats = other.GetComponent<PlayerStats>();

            if (stats != null)
            {
                stats.LevelUp(); // GỌI HÀM LÊN CẤP
            }

            // Ăn xong thì vật phẩm biến mất
            Destroy(gameObject);
        }
    }
}