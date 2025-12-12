using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] protected int maxHealth = 2;
    [SerializeField] protected int damageToPlayer = 1;

    protected int currentHealth;

    public int CurrentHealth => currentHealth;
    SoundPlayer soundManager;
    public int MaxHealth => maxHealth;

    protected virtual void Start()
    {
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
    public System.Action<int, int> OnHealthChanged; // (current, max)
    void Awake()
    {
        if (soundManager == null)
            soundManager = FindFirstObjectByType<SoundPlayer>();
    }

    public virtual void TakeDamage(int amount)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        soundManager.PlayEnemyDeathSound();
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

