using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EnemySpawner : MonoBehaviour
{
    public enum SpawnMode { Periodic, Waves }

    [Header("General")]
    public SpawnMode mode = SpawnMode.Periodic;
    public bool startOnAwake = true;
    [SerializeField] bool loop = true;

    [Header("Spawn Area")]
    public float spawnXOffset = 1f; // how far off the right edge to spawn
    public Vector2 spawnYRange = new Vector2(-3f, 3f);

    [Header("Periodic Settings")]
    public GameObject[] periodicPrefabs;
    public float spawnInterval = 3f;
    public int maxSimultaneous = 10; // how many can spawn at once

    [Header("Waves Settings")]
    public Wave[] waves;
    [System.Serializable]
    public struct Wave
    {
        public GameObject prefab;
        public int count;
        public float spacing; // seconds between spawns in this wave
        public float delayBefore; // seconds delay before this wave starts
    }

    Coroutine running;
    
    void Start()
    {
        if (startOnAwake)
        {
            StartSpawning();
        }
    }

    public void StartSpawning()
    {
        if (running != null) StopCoroutine(running);
        running = StartCoroutine(SpawnRoutine());
    }

    public void StopSpawning()
    {
        if (running != null) StopCoroutine(running);
        running = null;
    }

    IEnumerator SpawnRoutine()
    {
        do
        {
            if (mode == SpawnMode.Periodic)
            {
                yield return StartCoroutine(PeriodicLoop());
            }
            else
            {
                yield return StartCoroutine(WavesLoop());
            }
        } while (loop);
    }

    IEnumerator PeriodicLoop()
    {
        while (true)
        {
            if (periodicPrefabs != null && periodicPrefabs.Length > 0)
            {
                // Don't spawn too many at once
                if (maxSimultaneous <= 0 || CountActiveEnemies() < maxSimultaneous)
                {
                    var prefab = periodicPrefabs[Random.Range(0, periodicPrefabs.Length)];
                    Spawn(prefab);
                }
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    IEnumerator WavesLoop()
    {
        if (waves == null || waves.Length == 0) yield break;

        foreach (var w in waves)
        {
            if (w.delayBefore > 0f) yield return new WaitForSeconds(w.delayBefore);

            for (int i = 0; i < w.count; i++)
            {
                Spawn(w.prefab);
                if (w.spacing > 0f)
                    yield return new WaitForSeconds(w.spacing);
            }
        }
        
        if (!loop && SceneManager.GetActiveScene().name == "Level1")
        {
         yield return new WaitForSeconds(5);
         SceneManager.LoadScene("Level2");
        }
        yield break;
    }

    void Spawn(GameObject prefab)
    {
        if (prefab == null) return;

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("EnemySpawner: No main camera found.");
            return;
        }

        float camHeight = cam.orthographicSize * 2f;
        float camWidth = camHeight * cam.aspect;

        float spawnX = cam.transform.position.x + camWidth / 2f + spawnXOffset;

        float y = Random.Range(spawnYRange.x, spawnYRange.y);

        Vector3 pos = new Vector3(spawnX, y, 0f);
        Instantiate(prefab, pos, Quaternion.identity);
    }

    int CountActiveEnemies()
    {
        // Simple heuristic: count active objects with Enemy component in scene
        var enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        return enemies != null ? enemies.Length : 0;
    }
}
