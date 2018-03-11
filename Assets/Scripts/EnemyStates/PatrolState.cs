using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private Enemy enemy;
    private float patrolTimer;
    private float patrolDuration;


    public void Enter(Enemy givenEnemy)
    {
        patrolDuration = UnityEngine.Random.Range(1, 7);
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
        if (other.tag == "Knife" || other.tag == "Bullet")
        {
            enemy.Target = Player.Instance.gameObject; // enemy runs towards player when it is hit at idle state
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
