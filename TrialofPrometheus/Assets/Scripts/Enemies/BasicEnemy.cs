using UnityEngine;

public class BasicEnemy : Enemy
{
    [SerializeField] float speed = 2;           // Horizontal speed (to the left)
    public float amplitude = 1f;       // Height of sine wave
    public float frequency = 2f;       // Frequency of sine wave

    private float startY;              // Starting Y position
    private float timeOffset; 

    private float leftBound;
    private float halfWidth;

    protected override void Start()
    {
        base.Start();
        startY = transform.position.y;
        timeOffset = Random.value * 10f;

        Camera cam = Camera.main;
        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            halfWidth = sr.bounds.extents.x;
        }

        leftBound = cam.transform.position.x - camWidth / 2f - halfWidth - 0.5f;
    }

    void Update()
    {
         // --- Movement ---
        
        // Move left on X
        float x = transform.position.x - speed * Time.deltaTime;

        // Oscillate Y using sine wave
        float y = startY + Mathf.Sin((Time.time + timeOffset) * frequency) * amplitude;

        Vector2 newPos = new Vector2(x, y);

        // Calculate the direction the enemy is moving *this frame*
        Vector2 direction = newPos - (Vector2)transform.position;

        // Apply movement
        transform.position = newPos;

        // --- Rotation ---
        
        if (direction.sqrMagnitude > 0.0001f)
        {
            // Rotate to face movement direction
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 90f;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        if (transform.position.x < leftBound)
        {
            Destroy(gameObject);
        }
    }
}
