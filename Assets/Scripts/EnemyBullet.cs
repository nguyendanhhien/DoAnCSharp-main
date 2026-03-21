using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10; // Sát thương mặc định (sẽ bị ghi đè bởi độ khó)
    private Vector2 moveDirection;

    public void SetDirection(Vector2 dir)
    {
        moveDirection = dir;
    }

    void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime);
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats player = other.GetComponent<PlayerStats>();
            if (player != null)
            {
                // Trừ máu theo chỉ số damage hiện tại
                player.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}