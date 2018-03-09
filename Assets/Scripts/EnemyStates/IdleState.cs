using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    private Enemy enemy;

    //contains the time the enemy has been idle and is used to change the states if the threshold is crossed
    private float idleTimer;

    private float idleDuration;

    public void Enter(Enemy givenEnemy)
    {
        idleDuration = UnityEngine.Random.Range(1, 7);
        this.enemy = givenEnemy;
    }

    public void Execute()
    {
        Idle();
        if (enemy.Target != null)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit()
    {
    }

    public void OnTriggerEnter(Collider2D other)
    {
        if (other.tag == "Knife")
        {
            enemy.Target = Player.Instance.gameObject; // enemy runs towards player when it is hit at idle state
        }
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
