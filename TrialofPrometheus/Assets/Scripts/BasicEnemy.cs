using UnityEngine;

public class BasicEnemy : Enemy
{
    [SerializeField] float speed = 2f;

    private float leftBound;
    private float halfWidth;

    protected override void Start()
    {
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
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < leftBound)
        {
            Destroy(gameObject);
        }
    }
}
