using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    int currentHealth;
    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;
    SoundPlayer soundManager;
    SceneController sceneControl;
    void Awake()
    {
        if (soundManager == null)
            soundManager = FindFirstObjectByType<SoundPlayer>();
            sceneControl = FindFirstObjectByType<SceneController>();
    }
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        Debug.Log($"Took damage, health remaining: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            soundManager.PlayDamageSound();
        }
    }

    
    void Die()
    {
        soundManager.PlayDeathSound();
        sceneControl.PlayerDied(3f);
        Debug.Log("Player died.");
        Destroy(gameObject);
    }
}
