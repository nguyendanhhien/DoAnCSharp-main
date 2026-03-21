using UnityEngine;

public class BulletData : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 10; // Sát thương hiện tại

    // Hàm này để PlayerShooting gọi và truyền sát thương vào
    public void SetDamage(int dmg)
    {
        this.damage = dmg;
    }

    void Update()
    {
        // Bay thẳng lên trên
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        // Tắt khi bay quá màn hình (số 12f tùy màn hình của bạn)
        if (transform.position.y > 12f) gameObject.SetActive(false);
    }
}