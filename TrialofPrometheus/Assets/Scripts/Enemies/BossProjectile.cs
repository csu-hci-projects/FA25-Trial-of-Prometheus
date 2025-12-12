using UnityEngine;

/// <summary>
/// Simple projectile mover for boss enemy shots.
/// Moves in a fixed direction and handles player/screen collision cleanup.
/// </summary>
public class BossProjectile : MonoBehaviour
{
    private Vector2 direction = Vector2.left;
    private float speed = 5f;
    private float lifetime = 10f;

    public void SetDirection(Vector2 newDirection, float newSpeed)
    {
        direction = newDirection.normalized;
        speed = newSpeed;
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move in direction
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Destroy if far off-screen
        if (Mathf.Abs(transform.position.x) > 30f || Mathf.Abs(transform.position.y) > 30f)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    void HandleCollision(GameObject other)
    {
        if (other == null) return;

        // Check if it hit the player
        var player = other.GetComponent<PlayerMover>();
        if (player != null)
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
            Destroy(gameObject);
            return;
        }
    }
}
