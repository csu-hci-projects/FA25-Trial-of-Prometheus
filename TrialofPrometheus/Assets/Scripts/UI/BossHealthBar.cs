using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays the boss's health on a UI Slider (or Image with fill if swapped).
/// Attach this to a UI GameObject that holds a Slider. Assign the BossEnemy in the inspector.
/// </summary>
public class BossHealthBar : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Enemy boss; // reference to BossEnemy (inherits Enemy)
    [SerializeField] private bool hideWhenNoBoss = true;

    void Start()
    {
        if (healthSlider == null)
        {
            healthSlider = GetComponentInChildren<Slider>();
        }

        // Hide initially until boss appears
        SetVisible(false);

        // Always search for boss in scene, ignore prefab assignment
        StartCoroutine(FindBossRoutine());
    }

    void Update()
    {
        // Check if boss still exists, hide if it's destroyed
        if (boss != null && boss.gameObject == null)
        {
            boss = null;
            SetVisible(false);
        }
    }

    private System.Collections.IEnumerator FindBossRoutine()
    {
        while (true)
        {
            // Only search if we don't have a boss
            if (boss == null)
            {
                var foundBoss = FindFirstObjectByType<BossEnemy>();
                if (foundBoss != null)
                {
                    Debug.Log("BossHealthBar: Found boss via polling!");
                    boss = foundBoss;
                    WireUp(boss);
                }
            }
            yield return new WaitForSeconds(0.1f); // Check more frequently
        }
    }

    void OnEnable()
    {
        BossEnemy.OnBossSpawned += HandleBossSpawned;
    }

    void OnDisable()
    {
        BossEnemy.OnBossSpawned -= HandleBossSpawned;
    }

    private void HandleBossSpawned(BossEnemy spawnedBoss)
    {
        Debug.Log("BossHealthBar: Boss spawn event received!");
        if (boss == null)
        {
            boss = spawnedBoss;
            WireUp(boss);
            Debug.Log("BossHealthBar: Wired up to boss!");
        }
    }

    public void SetBoss(Enemy newBoss)
    {
        if (boss != null)
        {
            boss.OnHealthChanged -= OnBossHealthChanged;
        }
        boss = newBoss;
        WireUp(boss);
    }

    private void WireUp(Enemy target)
    {
        if (target == null || healthSlider == null)
        {
            Debug.LogWarning("BossHealthBar: Cannot wire up - target or slider is null!");
            return;
        }

        healthSlider.maxValue = target.MaxHealth;
        healthSlider.value = target.CurrentHealth;
        target.OnHealthChanged += OnBossHealthChanged;
        SetVisible(true);
        Debug.Log($"BossHealthBar: Displaying bar! Health: {target.CurrentHealth}/{target.MaxHealth}");
    }

    private void OnDestroy()
    {
        if (boss != null)
        {
            boss.OnHealthChanged -= OnBossHealthChanged;
        }
    }

    private void OnBossHealthChanged(int current, int max)
    {
        if (healthSlider == null) return;
        healthSlider.maxValue = max;
        healthSlider.value = Mathf.Max(0, current);

        if (hideWhenNoBoss && current <= 0)
        {
            SetVisible(false);
        }
    }

    private void SetVisible(bool visible)
    {
        var cg = GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = visible ? 1f : 0f;
            cg.interactable = visible;
            cg.blocksRaycasts = visible;
        }
        else
        {
            gameObject.SetActive(visible);
        }
    }
}
