using UnityEngine;
using System.Collections.Generic;

public class PlayerShooter : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform firePoint;      // where the missile spawns (e.g. a child empty object)
    public int maxMissiles = 5;      // how many shots allowed on screen
    public float fireRate = 0.25f;   // seconds between shots
    

    private float nextFireTime = 0f;
    private List<GameObject> activeMissiles = new List<GameObject>();
    SoundPlayer soundManager;
    void Awake()
    {
        if (soundManager == null)
            soundManager = FindFirstObjectByType<SoundPlayer>();
    }
    void Update()
    {
        // Clean up null (destroyed) missiles
        activeMissiles.RemoveAll(m => m == null);

        // Fire input
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            if (activeMissiles.Count < maxMissiles)
            {
                FireMissile();
                nextFireTime = Time.time + fireRate;
            }
        }
    }

    void FireMissile()
    {
        GameObject missile = Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
        activeMissiles.Add(missile);
        soundManager.PlayMissileSound();
    }

    
}
