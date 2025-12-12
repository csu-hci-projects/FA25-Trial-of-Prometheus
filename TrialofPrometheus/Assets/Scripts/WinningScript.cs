using UnityEngine;

public class WinningScript : MonoBehaviour
{
    SoundPlayer soundManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        soundManager = FindFirstObjectByType<SoundPlayer>();
        soundManager.PlayVictorySound();
    }

    
}
