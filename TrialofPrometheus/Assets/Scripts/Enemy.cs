using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int maxHealth = 2;
    [SerializeField] protected int damageToPlayer = 1;

    protected int currentHealth;
    void Awake()
{
   // if (transform.rotation == Quaternion.identity)
       // transform.Rotate(0, 0, 90);
}

    protected virtual void Start()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        HandleCollision(other.gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.gameObject);
    }

    protected virtual void HandleCollision(GameObject other)
    {
        if (other == null) return;

        var shot = other.GetComponent<ShotMover>();
        if (shot != null)
        {
            Destroy(other);
            TakeDamage(1);
            return;
        }

        var player = other.GetComponent<PlayerMover>();
        if (player != null)
        {
            var playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageToPlayer);
            }
            Die();
            return;
        }
    }
}
