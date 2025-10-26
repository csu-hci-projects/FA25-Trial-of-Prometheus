using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static List<EnemyCollision> Enemies = new();
    public static List<ShotMover> Shots = new();

    void Update()
    {
        // Check all shots against all enemies
        for (int i = Shots.Count - 1; i >= 0; i--)
        {
            ShotMover shot = Shots[i];
            for (int j = Enemies.Count - 1; j >= 0; j--)
            {
                EnemyCollision enemy = Enemies[j];

                if (IsOverlapping(enemy, shot))
                {
                    enemy.TakeDamage(1);
                    shot.DestroyShot();
                    break; // Shot destroyed, move to next shot
                }
            }
        }
    }

    // Simple bounds-based collision
    private bool IsOverlapping(EnemyCollision enemy, ShotMover shot)
    {
        return enemy.Bounds.Intersects(shot.Bounds);
    }
}
