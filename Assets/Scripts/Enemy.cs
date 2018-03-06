using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    //refers to enemey's current state and swtiches between idle, meele and patrol state
    private IEnemyState currentState;

	// Use this for initialization
	public override void Start () {
        base.Start();
        WalkingSpeed = 2;
        //enemy is set to idle state at start
        ChangeState(new IdleState());
	}
	
	// Update is called once per frame
	void Update () {
        currentState.Execute();
		
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
        MyAnimator.SetFloat("speed", 1);
        transform.Translate(GetDirection() * (WalkingSpeed * Time.deltaTime)); //deltatime makes sure the enemy moves at same speed in different devices and different framerates
            
    }

    //returns direction of enemy
    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }
}
