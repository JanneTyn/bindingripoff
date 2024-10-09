using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtPlayerEnemy : Enemy
{
    private float timer;

    [SerializeField] private float aiTime;
    [SerializeField] private Weapon weapon;

    private Vector2 movementDirection;
    private Transform player;

    private enum EnemyStates
    {
        StopAndShoot,
        Move
    }

    private EnemyStates enemyFSM;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        aiTime = aiTime - (aiTime / 40 * TestDifficultyScaler.instance.currentRoomDifficulty);
        if (aiTime < 0.5f) aiTime = 0.5f;
        timer = aiTime;
        DecideDirection();
        enemyFSM = EnemyStates.Move;
        base.Start();
    }

    private void FixedUpdate()
    {
        if (!aiActive) return;

        timer -= Time.fixedDeltaTime;

        //fsm
        switch (enemyFSM)
        {
            case EnemyStates.StopAndShoot:

                if(timer <= 0)
                {
                    var shootDir = (player.transform.position - transform.position).normalized;
                    // 1 - damageMultiplier
                    weapon.Shoot(gameObject, shootDir, false, damageMultiplier);
                    DecideDirection();
                    enemyFSM = EnemyStates.Move;
                    timer = aiTime;
                }
                break;

            case EnemyStates.Move:

                movementVector = movementDirection * movementSpeed;

                if(timer <= 0)
                {
                    enemyFSM = EnemyStates.StopAndShoot;
                    movementVector = Vector2.zero;
                    timer = aiTime;
                }
                break;
        }

        base.FixedUpdate();
    }

    /// <summary>
    /// Decide which direction to move next
    /// </summary>
    private void DecideDirection()
    {
        List<Vector2> directions = new()
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };
        List<Vector2> freeDirections = new();

        foreach (var dir in directions)
        {
            if(!Physics2D.Raycast(transform.position, dir, 5f, LayerMask.GetMask("Wall")))
            {
                freeDirections.Add(dir);
            }
        }

        if(freeDirections.Count == 0) enemyFSM = EnemyStates.StopAndShoot;
        else
        {
            movementDirection = freeDirections[Random.Range(0, freeDirections.Count)];
        }
        
    }
}