using UnityEngine;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float speed = 6.0f;

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get input from keyboard (horizontal and vertical)
        movement.x = Input.GetAxisRaw("Horizontal"); // use GetAxisRaw for snappier input
        movement.y = Input.GetAxisRaw("Vertical");
        movement = movement.normalized; // normalize so diagonal speed isn't faster
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    void FixedUpdate()
    {
        // Move the player by setting the Rigidbody2D's position
        
    }
}
