using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public float speed = 3f;

    void Update()
    {
        
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        
        if (transform.position.y < -12f) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.CompareTag("Player"))
        {
            
            PlayerShooting shooting = other.GetComponent<PlayerShooting>();
            if (shooting != null)
            {
                shooting.ActivateFrenzy();
            }

            
            Destroy(gameObject);
        }
    }
}