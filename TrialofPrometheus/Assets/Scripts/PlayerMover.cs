using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float speed = 6.0f;

    private float minX, maxX, minY, maxY;
    private float halfWidth, halfHeight;
    
    void Start()
    {
        Camera cam = Camera.main;

        // Get orthographic size and aspect
        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        // Get player sprite size in world units
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            halfWidth = sr.bounds.extents.x;
            halfHeight = sr.bounds.extents.y;
        }

        // Calculate camera edges in world coordinates
        minX = cam.transform.position.x - camWidth / 2f + halfWidth;
        maxX = cam.transform.position.x + 0.3f + camWidth / 2f - halfWidth;
        minY = cam.transform.position.y - 0.1f - camHeight / 2f + halfHeight;
        maxY = cam.transform.position.y + 0.1f + camHeight / 2f - halfHeight;
    }

    void Update()
    {
        // Get input from keyboard (horizontal and vertical)
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(moveX, moveY, 0f).normalized * speed * Time.deltaTime;
        transform.position += move;

        // Clamp to screen
        float clampedX = Mathf.Clamp(transform.position.x, minX, maxX);
        float clampedY = Mathf.Clamp(transform.position.y, minY, maxY);
        transform.position = new Vector3(clampedX, clampedY, transform.position.z);
    }

    void FixedUpdate()
    {
        // Move the player by setting the Rigidbody2D's position
        
    }
}
