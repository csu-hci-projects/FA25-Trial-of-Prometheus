using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static List<EnemyCollision> Enemies = new();
    public static List<ShotMover> Shots = new();
    public static PlayerMover Player;

    void Update()
    {
        CheckShotEnemyCollisions();
        CheckPlayerEnemyCollisions();
    }

    // Simple bounds-based collision
    private bool IsOverlapping(Bounds a, Bounds b)
    {
        return a.Intersects(b);
    }

    void CheckPlayerEnemyCollisions()
    {
        if (Player == null)
            return;

        for (int j = Enemies.Count - 1; j >= 0; j--)
        {
            EnemyCollision enemy = Enemies[j];

            if (IsOverlapping(enemy.Bounds, Player.Bounds))
            {
                Player.TakeDamage(1);

            }
        }
    }
    
    void CheckShotEnemyCollisions()
    {
        for (int i = Shots.Count - 1; i >= 0; i--)
        {
            ShotMover shot = Shots[i];
            for (int j = Enemies.Count - 1; j >= 0; j--)
            {
                EnemyCollision enemy = Enemies[j];

                if (IsOverlapping(enemy.Bounds, shot.Bounds))
                {
                    enemy.TakeDamage(1);
                    shot.DestroyShot();
                    break;
                }
            }
        }
    }
}
