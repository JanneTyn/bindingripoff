using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootAtPlayerEnemy : Enemy
{
    private float timer;

    [SerializeField] private int aiTime;
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
        timer = aiTime;
        DecideDirection();
        enemyFSM = EnemyStates.Move;
        base.Start();
    }

    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;

        //fsm
        switch (enemyFSM)
        {
            case EnemyStates.StopAndShoot:

                if(timer <= 0)
                {
                    var shootDir = (player.transform.position - transform.position).normalized;
                    weapon.Shoot(gameObject, shootDir, false);
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
        //TODO smarter, dont walk towards walls, etc.
        List<Vector2> directions = new()
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        movementDirection = directions[Random.Range(0, 4)];
    }
}
