using UnityEngine;

public class SwoopEnemyPixelCorrected : Enemy
{
    [Header("Swoop Settings")]
    public float speed = 2f;               // horizontal movement speed
    public float verticalOffset = 0.3f;    // small vertical sine arc (~10 pixels)
    public float stopDistanceX = 0.5f; // distance from target X to fire   
    public GameObject bulletPrefab;

    private Transform firePoint;
    private Transform spriteTransform;

    private Vector2 startPos;
    private Vector2 targetPos;

    private bool hasFired = false;
    private bool isLeaving = false;
    SoundPlayer soundManager;
    
    protected override void Start()
    {
        base.Start();

        // Find player
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        // Start position already set in scene
        startPos = transform.position;

        // Target: horizontal near player, vertical slightly above/below player
        float yOffset = Random.Range(-0.3f, 0.3f);
        targetPos = new Vector2(player.transform.position.x, player.transform.position.y + yOffset);

        // Sprite transform: first child or self
        spriteTransform = transform.childCount > 0 ? transform.GetChild(0) : transform;

        // Auto-create firePoint at nose of enemy
        firePoint = new GameObject("FirePoint").transform;
        firePoint.SetParent(transform);
        firePoint.localPosition = new Vector3(0f, 0.25f, 0f);
        firePoint.localRotation = Quaternion.identity;
    }

    void Update()
    {
        if (isLeaving)
        {
            MoveLeaving();
        }
        else
        {
            MoveTowardTarget();
        }
    }

    private void MoveTowardTarget()
    {
        Vector2 currentPos = transform.position;

        // Move X toward target
        float newX = Mathf.MoveTowards(currentPos.x, targetPos.x, speed * Time.deltaTime);

        // Compute progress 0→1
        float progress = Mathf.InverseLerp(startPos.x, targetPos.x, newX);

        // Smooth vertical path: linear lerp + small sine arc
        float newY = Mathf.Lerp(startPos.y, targetPos.y, progress)
                     + Mathf.Sin(progress * Mathf.PI) * verticalOffset;

        Vector2 newPos = new Vector2(newX, newY);
        Vector2 direction = newPos - (Vector2)transform.position;
        transform.position = newPos;

       
        if (direction.sqrMagnitude > 0.0001f)
        {
            // Rotate to face movement direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // Fire when close enough
        if (!hasFired && Mathf.Abs(newX - targetPos.x) <= stopDistanceX)
        {
            FireBullet();
            hasFired = true;
            isLeaving = true;
            startPos = transform.position; // reference for leaving
            targetPos = new Vector2(startPos.x + ((startPos.x < 0) ? -3f : 3f), startPos.y); // leave direction
        }
    }

    private void MoveLeaving()
    {
        Vector2 currentPos = transform.position;

        // Move horizontally
        float newX = Mathf.MoveTowards(currentPos.x, targetPos.x, speed * Time.deltaTime);

        // Small vertical offset while leaving
        float progress = Mathf.InverseLerp(startPos.x, targetPos.x, newX);
        float newY = startPos.y + Mathf.Sin(progress * Mathf.PI) * (verticalOffset * 0.5f);

        Vector2 newPos = new Vector2(newX, newY);
        Vector2 direction = newPos - (Vector2)transform.position;
        transform.position = newPos;

        
        if (direction.sqrMagnitude > 0.0001f)
        {
            // Rotate to face movement direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect; 
        // Destroy if offscreen (roughly)
        if (newX < Camera.main.transform.position.x - camWidth/2 - 1f || 
            newX > Camera.main.transform.position.x + camWidth/2 + 1f)
        {
            Destroy(gameObject);
        }
    }

    private void FireBullet()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    }
/*
    private void RotateSpriteTowards(Vector2 dir)
    {
        if (dir.sqrMagnitude < 0.0001f) return;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        angle -= 90f; // sprite points up
        spriteTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
    */
}

