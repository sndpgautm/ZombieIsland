using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private Enemy enemy;
    private float patrolTimer;
    private float patrolDuration = 7;


    public void Enter(Enemy givenEnemy)
    {
        this.enemy = givenEnemy;
    }

    public void Execute()
    {
        Patrol();
        enemy.Move();
        if (enemy.Target != null && enemy.InMeleeRange)
        {
            enemy.ChangeState(new MeleeState());
        }
    }

    public void Exit()
    {
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if (other.tag == "Edge")
        {
            enemy.ChangeDirection();
        }
    }

    private void Patrol()
    {
        //Time.deltaTime is the time it has passed since last frame was rendered
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
