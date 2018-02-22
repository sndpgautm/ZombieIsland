using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private Rigidbody2D myRigidbody;
	private Animator myAnimator;


	[SerializeField]
	private float walkingSpeed;

	private bool facingRight;


	// Use this for initialization
	void Start () {
		
		facingRight = true;
		myRigidbody = GetComponent<Rigidbody2D> ();
		myAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		float horizontal = Input.GetAxis ("Horizontal");
		HandleMovement (horizontal);
		Flip (horizontal);
	}

	//handles the movement of player
	private void HandleMovement(float horizontal)
	{
		myRigidbody.velocity = new Vector2 (horizontal*walkingSpeed, myRigidbody.velocity.y);
		myAnimator.SetFloat ("speed", Mathf.Abs(horizontal)); //speed is case sensitive and horizontal is made positive to compare
	}



	//Flips the player in opposite direction
	private void Flip(float horizontal)
	{
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
			facingRight = !facingRight; //toggles the values true if its false and vice versa.
			Vector3 theScale = transform.localScale; //referencing the local scale of player
			theScale.x*=-1; //multiplying the value of x of scale by negative
			transform.localScale = theScale; //changed the value of scale in Transform
		}
	}
}
