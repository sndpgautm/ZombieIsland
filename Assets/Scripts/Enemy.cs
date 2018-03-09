using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    //refers to enemey's current state and swtiches between idle, meele and patrol state
    private IEnemyState currentState;

    public GameObject Target { get; set; }
    [SerializeField]
    private float meleeRange;
    [SerializeField]
    private EdgeCollider2D enemyHandCollider;
    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <=meleeRange; //returns true if distance between the enemy and target is less than melee range
            }

            return false;
        }
    }

    public override bool IsDead
    {
        get
        {
            return health <= 0; //return true
        }
    }

    public EdgeCollider2D EnemyHandCollider
    {
        get
        {
            return enemyHandCollider;
        }
    }

    private Vector2 startPos;
    [SerializeField]
    private Transform leftEdge;
    [SerializeField]
    private Transform rightEdge;

    // Use this for initialization
    public override void Start () {
        base.Start();
        startPos = transform.position;
        Player.Instance.Dead += new DeadEventHandler(RemoveTarget); // Remove target function is called when players dead event is triggered

        WalkingSpeed = 2;
        //enemy is set to idle state at start
        ChangeState(new IdleState());
	}

    
	
	// Update is called once per frame
	void Update () {

        if (!IsDead) // if enemy is alive
        {
            if (!TakingDamage) // if enemy is not taking damage
            {
                //executes the current state, this makes the enemy move or attack etc.
                currentState.Execute();
            }
            
            LookAtTarget();//works if you attack an enemy and jump around it, enemey also turns around
        }
        
		
	}


    //when enemy kills the player he will go into patrol state and stops attacking the player
    public void RemoveTarget()
    {
        Target = null;
        ChangeState(new PatrolState());
    }

    //Follows the direction of target(player)
    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - this.transform.position.x; //returns targets direction minus own direction, if<0 then the target is on left and if>0 then its on right

            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }

    }

    //Changes the state of enemy
    public void ChangeState(IEnemyState newState) {
        //if we had have a state we need to exit it and chaneg it to new state
        if (currentState != null) {
            currentState.Exit();
        }

        currentState = newState;

        //drives the current enemy
        currentState.Enter(this);
    }

    //moves the enemy in the direction it is floating
    public void Move() {

        if (!Attack) // enemy cannot move if she is attacking
        {
            if ((GetDirection().x > 0 && transform.position.x < rightEdge.position.x) || (GetDirection().x < 0 && transform.position.x > leftEdge.position.x))
            {
                MyAnimator.SetFloat("speed", 1);
                //moves the enemy in right direction
                transform.Translate(GetDirection() * (WalkingSpeed * Time.deltaTime)); //deltatime makes sure the enemy moves at same speed in different devices and different framerates

            }
            else if (currentState is PatrolState)
            {
                ChangeDirection();
            }
        }

        


        

    }

    //returns direction of enemy
    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    //enemy attacks with hand
    public void MeleeAttack()
    {
        EnemyHandCollider.enabled = true;
    }


    public override void  OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("hit");
        //calls the base on trigger enter
        base.OnTriggerEnter2D(other);

        //calls ontriggerenter on the current state
        currentState.OnTriggerEnter(other);
    }

    public override IEnumerator TakeDamage()
    {
        health -= 10;

        if (!IsDead)
        {
            MyAnimator.SetTrigger("damage");

        }
        else
        {
            MyAnimator.SetTrigger("die");
            yield return null;
        }
    }


    //respwaning the enemy
    public override void Death()
    {
        MyAnimator.ResetTrigger("die");
        MyAnimator.SetTrigger("idle");
        transform.position = startPos;

    }
}
