using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Test AI that walks left/right
/// </summary>
public class TestEnemy : Enemy
{
    private float timer;
    [SerializeField] private float aiTime = 2f;
    [SerializeField] private Weapon weapon;

    private enum TestEnemyStates
    {
        MovingRight,
        MovingLeft
    }
    private TestEnemyStates enemyFSM = TestEnemyStates.MovingRight;

    private void Start()
    {
        timer = aiTime;
        base.Start();
    }

    private void FixedUpdate()
    {
        timer -= Time.fixedDeltaTime;

        //ai fsm
        switch (enemyFSM)
        {
            case TestEnemyStates.MovingRight:

                movementVector = Vector2.right * movementSpeed;

                //exit clause
                if (timer <= 0)
                {
                    weapon.Shoot(gameObject, Vector2.zero, false, 1);
                    enemyFSM = TestEnemyStates.MovingLeft;
                    timer = aiTime;
                }
                
                break;
            case TestEnemyStates.MovingLeft:

                movementVector = Vector2.left * movementSpeed;

                //exit clause
                if (timer <= 0)
                {
                    weapon.Shoot(gameObject, Vector2.zero, false, 1);
                    enemyFSM = TestEnemyStates.MovingRight;
                    timer = aiTime;
                }
                break;
        }

        base.FixedUpdate();
    }
}
