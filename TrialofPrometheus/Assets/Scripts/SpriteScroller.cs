using UnityEngine;

public class SpriteLooper : MonoBehaviour
{
    [Header("Scroll Settings")]
    public float speed = 2f; // world units per second
    public Camera cam;       // optional; auto-fills with main camera

    [Header("Tiles")]
    public Transform[] tiles; // assign manually or auto-detect children

    void Start()
    {
        if (cam == null)
            cam = Camera.main;

        // Auto-fill if user didn’t assign
        if (tiles == null || tiles.Length == 0)
        {
            int count = transform.childCount;
            tiles = new Transform[count];
            for (int i = 0; i < count; i++)
                tiles[i] = transform.GetChild(i);
        }

        // Sort by x position so we know left→right order
        System.Array.Sort(tiles, (a, b) => a.position.x.CompareTo(b.position.x));

        // Align tiles edge-to-edge horizontally
        float currentX = tiles[0].position.x;
        for (int i = 1; i < tiles.Length; i++)
        {
            float prevWidth = GetSpriteWorldWidth(tiles[i - 1]);
            Vector3 pos = tiles[i].position;
            pos.x = tiles[i - 1].position.x + prevWidth;
            tiles[i].position = pos;
        }

        // Optional: shift everything left so the leftmost tile just covers the camera's left edge
        float camLeft = cam.ViewportToWorldPoint(Vector3.zero).x;
        float leftmostRightEdge = tiles[0].position.x + GetSpriteWorldWidth(tiles[0]) / 2f;
        float offset = leftmostRightEdge - camLeft;
        foreach (Transform t in tiles)
            t.position -= Vector3.right * offset;
    }

    void Update()
    {
        float move = speed * Time.deltaTime;

        // Move all tiles left
        foreach (Transform t in tiles)
            t.position += Vector3.left * move;

        // Check wrap for each tile
        float camLeft = cam.ViewportToWorldPoint(Vector3.zero).x;
        for (int i = 0; i < tiles.Length; i++)
        {
            Transform t = tiles[i];
            float width = GetSpriteWorldWidth(t);
            float rightEdge = t.position.x + width / 2f;

            if (rightEdge < camLeft)
            {
                Transform rightmost = GetRightmostTile();
                float newX = rightmost.position.x + GetSpriteWorldWidth(rightmost);
                t.position = new Vector3(newX, t.position.y, t.position.z);
            }
        }
    }

    Transform GetRightmostTile()
    {
        Transform rightmost = tiles[0];
        foreach (Transform t in tiles)
            if (t.position.x > rightmost.position.x)
                rightmost = t;
        return rightmost;
    }

    float GetSpriteWorldWidth(Transform t)
    {
        SpriteRenderer sr = t.GetComponent<SpriteRenderer>();
        return sr != null ? sr.bounds.size.x : 1f;
    }
}