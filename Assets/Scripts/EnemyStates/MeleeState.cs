using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyState {

    private float attackTimer;
    private float attackCooldown = 1;
    private bool canAttack = true;
    private Enemy enemy;
    public void Enter(Enemy givenEnemy)
    {
        this.enemy = givenEnemy;
    }

    public void Execute()
    {
        Attack();
        if (!enemy.InMeleeRange)
        {
            enemy.ChangeState(new PatrolState());
        }else if(enemy.Target == null)
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {
    }

    private void Attack()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackCooldown)
        {
            canAttack = true;
            attackTimer = 0;

        }

        if (canAttack)
        {
            canAttack = false;
            enemy.MyAnimator.SetTrigger("attack");
        }

    }
}
