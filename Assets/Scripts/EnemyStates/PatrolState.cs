using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private Enemy enemy;
    private float patrolTimer;
    private float patrolDuration = 10;


    public void Enter(Enemy givenEnemy)
    {
        this.enemy = givenEnemy;
    }

    public void Execute()
    {
        Debug.Log("patroling");
        Patrol();
        enemy.Move();
    }

    public void Exit()
    {
    }

    public void OnTriggerEnter(Collider2D other)
    {
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
