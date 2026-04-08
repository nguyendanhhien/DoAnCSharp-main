using UnityEngine;

public class BulletData : MonoBehaviour
{
    public float speed = 15f;
    public int damage = 10; 

    
    public void SetDamage(int dmg)
    {
        this.damage = dmg;
    }

    void Update()
    {
        
        transform.Translate(Vector2.up * speed * Time.deltaTime);

        
        if (transform.position.y > 12f) gameObject.SetActive(false);
    }
}