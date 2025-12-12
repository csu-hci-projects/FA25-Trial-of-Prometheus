using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public AudioClip musicClip; // Assign your music in inspector or via Resources
    private AudioSource audioSource;
    private bool isPlaying = false;

    void Start()
    {
        // Create an AudioSource automatically
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = musicClip;
        audioSource.loop = true; // Keep playing in a loop
        audioSource.Play();
        isPlaying = true;
    }

    // Call this method when the player dies
    public void StopMusic()
    {
        if (isPlaying)
        {
            audioSource.Stop();
            isPlaying = false;
        }
    }
}
