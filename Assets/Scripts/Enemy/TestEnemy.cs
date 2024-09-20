using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Test AI that walks left/right
/// </summary>
public class TestEnemy : EnemyBase
{
    private float timer = 2f;
    [SerializeField] private Weapon weapon;

    private enum TestEnemyStates
    {
        MovingRight,
        MovingLeft
    }
    private TestEnemyStates enemyFSM = TestEnemyStates.MovingRight;

    private void FixedUpdate()
    {
        //ai fsm
        switch (enemyFSM)
        {
            case TestEnemyStates.MovingRight:

                movementVector = Vector2.right * 3f;
                timer -= Time.fixedDeltaTime;

                //exit clause
                if (timer <= 0)
                {
                    weapon.Shoot(gameObject, Vector2.zero, false);
                    enemyFSM = TestEnemyStates.MovingLeft;
                    timer = 2f;
                }
                
                break;
            case TestEnemyStates.MovingLeft:
                movementVector = Vector2.left * 3f;
                timer -= Time.fixedDeltaTime;

                //exit clause
                if (timer <= 0)
                {
                    weapon.Shoot(gameObject, Vector2.zero, false);
                    enemyFSM = TestEnemyStates.MovingRight;
                    timer = 2f;
                }
                break;
        }

        base.FixedUpdate();
    }
}
