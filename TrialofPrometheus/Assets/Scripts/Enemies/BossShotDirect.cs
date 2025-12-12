using UnityEngine;

/// <summary>
/// Boss projectile that travels in a fixed direction without tracking the player.
/// Similar to EnemyShotTargeted but simpler — just moves in the direction it was fired.
/// Damages the player on contact.
/// </summary>
public class BossShotDirect : MonoBehaviour
{
    [SerializeField] protected int damageToPlayer = 1;
    [SerializeField] public float speed = 5f;

    private Vector2 direction = Vector2.left;
    private float lifetime = 10f;

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
    }

    void Start()
    {
        // Auto-destroy after lifetime
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move in the fixed direction
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Destroy if far off-screen
        if (Mathf.Abs(transform.position.x) > 30f ||
            Mathf.Abs(transform.position.y) > 30f)
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

    protected virtual void HandleCollision(GameObject other)
    {
        if (other == null) return;

        var player = other.GetComponent<PlayerMover>();
        if (player != null)
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
            }
            Destroy(gameObject);
        }
    }
}
