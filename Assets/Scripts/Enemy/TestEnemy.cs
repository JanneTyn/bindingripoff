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
    [SerializeField] private float maxAiTimeVariance = 0.2f;
    [SerializeField] private Weapon weapon;

    private enum TestEnemyStates
    {
        MovingRight,
        MovingLeft
    }
    private TestEnemyStates enemyFSM = TestEnemyStates.MovingRight;

    private void Start()
    {
        aiTime = aiTime + maxAiTimeVariance * Random.Range(-1f, 1f);
        aiTime = aiTime - (aiTime / 40 * TestDifficultyScaler.instance.currentRoomDifficulty);
        if (aiTime < 0.5f) aiTime = 0.5f;
        
        timer = aiTime;
        base.Start();
    }

    private void FixedUpdate()
    {
        if (!aiActive) return;

        timer -= Time.fixedDeltaTime;

        //ai fsm
        switch (enemyFSM)
        {
            case TestEnemyStates.MovingRight:

                movementVector = Vector2.right * movementSpeed;

                //exit clause
                if (timer <= 0)
                {
                    weapon.Shoot(gameObject, Vector2.zero, false, damageMultiplier);
                    animator.SetTrigger("Attack");
                    enemyFSM = TestEnemyStates.MovingLeft;
                    timer = aiTime;
                }
                
                break;
            case TestEnemyStates.MovingLeft:

                movementVector = Vector2.left * movementSpeed;

                //exit clause
                if (timer <= 0)
                {
                    weapon.Shoot(gameObject, Vector2.zero, false, damageMultiplier);
                    enemyFSM = TestEnemyStates.MovingRight;
                    timer = aiTime;
                }
                break;
        }

        base.FixedUpdate();
    }
}
