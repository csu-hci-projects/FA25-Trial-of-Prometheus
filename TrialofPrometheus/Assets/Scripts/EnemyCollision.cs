using Unity.VisualScripting;
using UnityEngine;

public class EnemyCollision : MonoBehaviour, IEnemy
{
    [SerializeField] private int health = 1;
    private SpriteRenderer sr;

    public Bounds Bounds => sr.bounds; // Expose bounds for collision check

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()  => CollisionManager.Enemies.Add(this);
    private void OnDisable() => CollisionManager.Enemies.Remove(this);

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

 
}
