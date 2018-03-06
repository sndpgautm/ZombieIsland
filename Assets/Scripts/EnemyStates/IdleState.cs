using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    private Enemy enemy;

    //contains the time the enemy has been idle and is used to change the states if the threshold is crossed
    private float idleTimer;

    private float idleDuration = 5;

    public void Enter(Enemy givenEnemy)
    {
        this.enemy = givenEnemy;
    }

    public void Execute()
    {
        Debug.Log("Idle");
        Idle();
    }

    public void Exit()
    {
    }

    public void OnTriggerEnter(Collider2D other)
    {
    }

    private void Idle()
    {
        enemy.MyAnimator.SetFloat("speed", 0);
        //Time.deltaTime is the time it has passed since last frame was rendered
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
}
