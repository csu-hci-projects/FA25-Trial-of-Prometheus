using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Boss enemy that:
/// 1. Moves left across the screen
/// 2. Stops and oscillates up/down slowly
/// 3. Fires projectiles in a cone pattern towards the left in bursts
/// Inherits from Enemy for collision handling and health.
/// </summary>
public class BossEnemy : Enemy
{
    public static System.Action<BossEnemy> OnBossSpawned;

    [Header("Movement")]
    [SerializeField] float moveLeftSpeed = 2f;
    [SerializeField] float moveLeftDistance = 5f; // How far left to move before stopping
    [SerializeField] float verticalSpeed = 1.5f;
    [SerializeField] float verticalRange = 2f; // How far up/down to oscillate

    [Header("Shooting")]
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform shootPoint1; // Left cone spread
    [SerializeField] Transform shootPoint2; // Additional shoot point
    [SerializeField] Transform shootPoint3; // Full circle spread
    [SerializeField] int projectilesPerBurst = 5;
    [SerializeField] float coneAngle = 45f; // Spread angle in degrees
    [SerializeField] float projectileSpeed = 5f;
    [SerializeField] float shootInterval = 0.1f; // Delay between projectiles in a burst
    [SerializeField] float burstCooldown = 3f; // Delay between bursts

    [Header("Attack Patterns")]
    [SerializeField] bool useShootPoint1 = true; // Fire cone left from shootPoint1
    [SerializeField] bool useShootPoint2 = false; // Fire from shootPoint2 (custom pattern)
    [SerializeField] bool useShootPoint3CircleSpread = false; // Fire in full circle from shootPoint3

    private float startY;
    private float startX;
    private float distanceMoved = 0f;
    private bool hasArrived = false;
    private bool isShooting = false;
    SceneController sceneControl;

    protected override void Start()
    {
        sceneControl = FindFirstObjectByType<SceneController>();
        base.Start();
        startY = transform.position.y;
        startX = transform.position.x;

        // Notify health bar that boss has spawned
        OnBossSpawned?.Invoke(this);

        // Start shooting immediately and movement
        StartCoroutine(BossMovementRoutine());
        StartCoroutine(BossShoottingRoutine());
    }

    void Update()
    {
        if (!hasArrived)
        {
            // Move left until reaching distance
            Vector3 pos = transform.position;
            float moveAmount = moveLeftSpeed * Time.deltaTime;
            pos.x -= moveAmount;
            distanceMoved += moveAmount;

            if (distanceMoved >= moveLeftDistance)
            {
                hasArrived = true;
            }

            transform.position = pos;
            transform.rotation = Quaternion.Euler(0f, 0f, 90f); // Face left
        }
        else
        {
            // Oscillate up and down
            float verticalOffset = Mathf.Sin(Time.time * verticalSpeed) * verticalRange;
            Vector3 pos = transform.position;
            pos.y = startY + verticalOffset;
            transform.position = pos;
            transform.rotation = Quaternion.Euler(0f, 0f, 90f); // Face left
        }
    }

    IEnumerator BossMovementRoutine()
    {
        // Boss moves left for moveLeftDistance, then oscillates in place
        while (!hasArrived)
        {
            yield return null;
        }
    }

    IEnumerator BossShoottingRoutine()
    {
        // Start shooting immediately and keep shooting forever
        while (true)
        {
            if (useShootPoint1)
                yield return StartCoroutine(ShootBurstCone(shootPoint1, 180f));
            if (useShootPoint2)
                yield return StartCoroutine(ShootBurstCone(shootPoint2, 180f));
            if (useShootPoint3CircleSpread)
                yield return StartCoroutine(ShootBurstCircle(shootPoint3));

            yield return new WaitForSeconds(burstCooldown);
        }
    }

    IEnumerator ShootBurstCone(Transform shootPoint, float centerAngle)
    {
        isShooting = true;

        for (int i = 0; i < projectilesPerBurst; i++)
        {
            FireProjectileCone(shootPoint, centerAngle, i);
            yield return new WaitForSeconds(shootInterval);
        }

        isShooting = false;
    }

    IEnumerator ShootBurstCircle(Transform shootPoint)
    {
        isShooting = true;

        for (int i = 0; i < projectilesPerBurst; i++)
        {
            FireProjectileCircle(shootPoint, i);
            yield return new WaitForSeconds(shootInterval);
        }

        isShooting = false;
    }

    void FireProjectileCone(Transform shootPoint, float centerAngle, int index)
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("BossEnemy: No projectile prefab assigned.");
            return;
        }

        // Determine spawn position
        Vector3 spawnPos = shootPoint != null ? shootPoint.position : transform.position;

        // Calculate angle for cone spread
        float minAngle = centerAngle - (coneAngle / 2f);
        float maxAngle = centerAngle + (coneAngle / 2f);
        float angle = Mathf.Lerp(minAngle, maxAngle, projectilesPerBurst > 1 ? (float)index / (projectilesPerBurst - 1) : 0.5f);

        // Convert angle to direction vector
        float radians = angle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;

        SpawnProjectile(spawnPos, direction);
    }

    void FireProjectileCircle(Transform shootPoint, int index)
    {
        if (projectilePrefab == null)
        {
            Debug.LogWarning("BossEnemy: No projectile prefab assigned.");
            return;
        }

        // Determine spawn position
        Vector3 spawnPos = shootPoint != null ? shootPoint.position : transform.position;

        // Calculate angle for full circle spread
        float angle = (360f / projectilesPerBurst) * index;

        // Convert angle to direction vector
        float radians = angle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;

        SpawnProjectile(spawnPos, direction);
    }

    void SpawnProjectile(Vector3 spawnPos, Vector2 direction)
    {
        // Instantiate projectile
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        // Apply movement script if it exists
        var mover = projectile.GetComponent<Rigidbody2D>();
        if (mover != null)
        {
            mover.linearVelocity = direction * projectileSpeed;
        }
        else
        {
            // If no Rigidbody2D, try to find or add a movement component
            var bossProjectile = projectile.GetComponent<BossProjectile>();
            if (bossProjectile == null)
            {
                bossProjectile = projectile.AddComponent<BossProjectile>();
            }
            bossProjectile.SetDirection(direction, projectileSpeed);
        }
    }

    public override void TakeDamage(int amount)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            sceneControl.BossDied(0f);
        }
    }
}
