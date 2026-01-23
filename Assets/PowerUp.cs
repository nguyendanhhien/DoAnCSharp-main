using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        // Bay chầm chậm xuống dưới
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        // Hủy khi bay khỏi màn hình
        if (transform.position.y < -12f) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra xem có phải Player chạm vào không
        // Đảm bảo Tag của máy bay là "Player"
        if (other.CompareTag("Player"))
        {
            // Gọi hàm kích hoạt bạo tẩu trên máy bay
            PlayerShooting shooting = other.GetComponent<PlayerShooting>();
            if (shooting != null)
            {
                shooting.ActivateFrenzy();
            }

            // Ăn xong thì xóa vật phẩm
            Destroy(gameObject);
        }
    }
}