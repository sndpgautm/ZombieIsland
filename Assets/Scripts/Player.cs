using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private Rigidbody2D myRigidbody;
	private Animator myAnimator;
	//can be edited from the inspector window
	[SerializeField]
	private float walkingSpeed=4;
	private bool facingRight;
	private bool isRunning;
	[SerializeField]
	private Transform[] groundPoints;
	[SerializeField]
	private float groundRadius;
	[SerializeField]
	private LayerMask whatIsGround;
	private bool isGrounded;
	private bool jump;
	[SerializeField]
	private bool airControl;
	[SerializeField]
	private float jumpForce;

	// Use this for initialization
	void Start () {
		
		facingRight = true;
		isRunning = false;
		myRigidbody = GetComponent<Rigidbody2D> ();
		myAnimator = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update(){
		HandleInput ();
	}




	void FixedUpdate () 
	{
		float horizontal = Input.GetAxis ("Horizontal");
		isGrounded = IsGrounded ();
		HandleMovement (horizontal);
		Flip (horizontal);
		HandleLayers ();
		ResetValues ();
	}

	//handles the movement of player
	private void HandleMovement(float horizontal)
	{
		//checks if the player is falling toward s ground
		if (myRigidbody.velocity.y < 0)
		{
			myAnimator.SetBool ("land", true);
		}
		//can only move if the player is on ground and has air control
		if(isGrounded || airControl)
		{
			//moves the player

			if (isRunning && Mathf.Abs(horizontal)>0.01) /* checks if the running button is pressed or not and turns
			the run animation only when both shift and direction keys are pressed*/
			{ 
				myRigidbody.velocity = new Vector2 (horizontal*walkingSpeed*2, myRigidbody.velocity.y);
				/*speed is case sensitive and horizontal is made positive to compare
				player goes right if speed is greater than 0.01 and left if less than 0.01
				checks horizontal to see if the player pressed left or right button
				speed is greater than 0.01 if the speed if player presses right and vice versa*/
				myAnimator.SetFloat ("speed", Mathf.Abs(horizontal));
				myAnimator.SetBool ("run", true);
				myAnimator.SetFloat("movementSpeed", walkingSpeed*2);
			} else {
				myRigidbody.velocity = new Vector2 (horizontal*walkingSpeed, myRigidbody.velocity.y);
				myAnimator.SetFloat ("speed", Mathf.Abs(horizontal));
				myAnimator.SetBool ("run", false);
				myAnimator.SetFloat("movementSpeed", walkingSpeed);
			}


		}


		//used to jump the player
		if (isGrounded && jump)
		{
			isGrounded = false;
			myRigidbody.AddForce (new Vector2 (0, jumpForce));
			myAnimator.SetTrigger ("jump");
		}
	}

	//Handles the input keyss
	private void HandleInput()
	{
		if (Input.GetKeyDown(KeyCode.Space)) //GetKeyDown When key is pressed once
		{
			jump = true;
		}
		if(Input.GetKey(KeyCode.LeftShift)){ //GetKey Checks when key is pressed continuously
			isRunning = true;
		}
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


	//Resets the values of jump, attack
	private void ResetValues()
	{
		jump = false;
		isRunning = false;
	}

	//Checks if the player is standing on the ground
	private bool IsGrounded()
	{
		if (myRigidbody.velocity.y <= 0)
		{
			foreach (Transform point in groundPoints)
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);

				for (int i = 0; i < colliders.Length; i++)
				{
					if (colliders [i].gameObject != gameObject) 
					{	
						myAnimator.ResetTrigger ("jump");
						myAnimator.SetBool ("land", false);
						return true;
					}
				}
			}	
		}
		return false;
	}
		
	//Handles the animator layers
	private void HandleLayers()
	{
		if (!isGrounded) {
			myAnimator.SetLayerWeight (1, 1); //Layer weight is set to 1 and Layer number 1 refers to AirLayer
		}else{
			myAnimator.SetLayerWeight (1, 0); //0 refers to GroundLayer
		}
	}

}
