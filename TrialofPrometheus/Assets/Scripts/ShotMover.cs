using UnityEngine;
using UnityEngine.UIElements;

public class ShotMover : MonoBehaviour
{
    [SerializeField] float speed = 6.0f;
    public float lifetime = 5f; // fallback lifetime in seconds

    private float rightBound, halfHeight, halfWidth;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private SpriteRenderer sr;
    public Bounds Bounds => sr.bounds; // Expose bounds for collision check

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()  => CollisionManager.Shots.Add(this);
    private void OnDisable() => CollisionManager.Shots.Remove(this);
    void Start()
    {
        Camera cam = Camera.main;

        // Get orthographic size and aspect
        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        // Get player sprite size in world units
        if (sr != null)
        {
            halfWidth = sr.bounds.extents.x;
        }

        // Calculate camera edges in world coordinates
        rightBound = cam.transform.position.x + 0.3f + camWidth / 2f - halfWidth;
        // Auto-destroy after a few seconds (safety)
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = new Vector3(1, 0f, 0f).normalized * speed * Time.deltaTime;
        transform.position += move;

        if (transform.position.x > rightBound)
        {
            Destroy(gameObject);

        }
    }
    public void DestroyShot()
    {
        CollisionManager.Shots.Remove(this);
        Destroy(gameObject);
    }
}
