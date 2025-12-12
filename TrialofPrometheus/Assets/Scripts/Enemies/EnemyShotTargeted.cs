using UnityEngine;

public class EnemyShotTargeted : MonoBehaviour
{
    [SerializeField] protected int damageToPlayer = 1;
    public float speed = 5f;

    private Vector2 targetDirection;  // normalized direction from projectile to player
    private float lifetime = 10f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    SoundPlayer soundManager;
    void Awake()
    {
        if (soundManager == null)
            soundManager = FindFirstObjectByType<SoundPlayer>();
    }
     void Start()
    {
        // Find player
        soundManager.PlayEnemyShotSound();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Calculate direction from projectile to player's current position
            Vector2 playerPos = player.transform.position;
            Vector2 startPos = transform.position;

            targetDirection = (playerPos - startPos).normalized;
        }
        else
        {
            // No player found → just shoot left or delete
            targetDirection = Vector2.left;
        }

        // Optional: Destroy automatically if nothing happens
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        // Move in the locked direction
        transform.Translate(targetDirection * speed * Time.deltaTime, Space.World);

        // Destroy if far off-screen (simple bounds check)
        if (Mathf.Abs(transform.position.x) > 30f ||
            Mathf.Abs(transform.position.y) > 30f)
        {
            Destroy(gameObject);
        }
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
