using UnityEngine;
using System.Collections.Generic;

public class HealthDisplay : MonoBehaviour
{
    [Header("Settings")]
    public PlayerHealth player;       // Your player object
    [SerializeField] public GameObject healthPrefab;   // The prefab for one health icon
    public Vector2 startPosition = new Vector2(50, -50); // upper-left corner offset
    public float spacing = 50f;       // space between icons

    private List<GameObject> icons = new List<GameObject>();

    void Start()
    {
        // Create one icon per max health
        for (int i = 0; i < player.MaxHealth; i++)
        {
            GameObject icon = Instantiate(healthPrefab, transform);
            icon.transform.localPosition = new Vector3(startPosition.x + i * spacing, startPosition.y, 0);
            icons.Add(icon);
        }
    }

    void Update()
    {
        // Turn icons on/off according to current health
        for (int i = 0; i < icons.Count; i++)
        {
            icons[i].SetActive(i < player.CurrentHealth);
        }
    }
}

