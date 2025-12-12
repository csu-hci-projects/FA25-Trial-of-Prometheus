using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioSource audioSource;
    [SerializeField] public AudioClip missileSound; // assign in inspector
    [SerializeField] public AudioClip damageSound;
    [SerializeField] public AudioClip deathSound;
    [SerializeField] public AudioClip enemySound;
    [SerializeField] public AudioClip enemyShot;
    [SerializeField] public AudioClip victory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMissileSound()
    {
        audioSource.PlayOneShot(missileSound);
    }
    public void PlayDamageSound()
    {
        audioSource.PlayOneShot(damageSound);
    }
    public void PlayDeathSound()
    {
        audioSource.PlayOneShot(deathSound);
    }
    public void PlayEnemyDeathSound()
    {
        audioSource.PlayOneShot(enemySound);
    }
    public void PlayEnemyShotSound()
    {
        audioSource.PlayOneShot(enemyShot);
    }
    public void PlayVictorySound()
    {
        audioSource.PlayOneShot(victory);
    }
}
