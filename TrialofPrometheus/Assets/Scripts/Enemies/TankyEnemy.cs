using UnityEngine;

public class TankyEnemy : Enemy
{
    [SerializeField] float speed = 0.35f; // Slower horizontal speed
    [SerializeField] int tankHealth = 5;  // Higher hit points than default
    private float leftBound;
    private float halfWidth;

    protected override void Start()
    {
        maxHealth = tankHealth; // Set higher health before base initializes
        base.Start();
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
        Vector2 oldPos = transform.position;
        Vector2 newPos = oldPos;
        newPos.x -= speed * Time.deltaTime; // Straight left movement
        transform.position = newPos;

        // Face the direction we moved this frame
        Vector2 direction = newPos - oldPos;
        if (direction.sqrMagnitude > 0.000001f)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 90f; // assumes sprite faces up by default
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        if (transform.position.x < leftBound)
        {
            Destroy(gameObject);
        }
    }
}
