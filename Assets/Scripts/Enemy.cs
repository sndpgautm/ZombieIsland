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

    // Use this for initialization
    public override void Start () {
        base.Start();
        WalkingSpeed = 2;
        //enemy is set to idle state at start
        ChangeState(new IdleState());
	}

    
	
	// Update is called once per frame
	void Update () {

        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }
            
            LookAtTarget();//works if you attack and enemy and jump around it, enemey also turns around
        }
        
		
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
            MyAnimator.SetFloat("speed", 1);
            transform.Translate(GetDirection() * (WalkingSpeed * Time.deltaTime)); //deltatime makes sure the enemy moves at same speed in different devices and different framerates

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
        base.OnTriggerEnter2D(other);
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
}
