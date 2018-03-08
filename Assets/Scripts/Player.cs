using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {


	private static Player instance;
	public static Player Instance
	{ 
		get
		{
			if (instance == null) 
			{
				instance = GameObject.FindObjectOfType<Player> ();
			}
			return instance;
		}
	}




	//Serialized field can be edited from the inspector window
	[SerializeField]
	private Transform[] groundPoints;
	[SerializeField]
	private float groundRadius;
	[SerializeField]
	private LayerMask whatIsGround;
	[SerializeField]
	private bool airControl;
	[SerializeField]
	private float jumpForce;
	[SerializeField]
	private Transform knifePos;
	[SerializeField]
	private GameObject knifePrefab;


	//properties are written different to variables starting with capital letter
	public Rigidbody2D MyRigidbody { get; set;}
	public bool Jump { get; set;}
	public bool OnGround{ get; set;}
	public bool IsRunning{ get; set;}

    public override bool IsDead
    {
        get
        {
            return health <= 0; //returns true
        }
    }

    private Vector2 startPos;


	// Use this for initialization
	public override void Start () {
		base.Start();
		IsRunning = false;
		MyRigidbody = GetComponent<Rigidbody2D> ();

	}
	
	// Update is called once per frame
	void Update(){
		HandleInput ();
	}




	void FixedUpdate () 
	{
		float horizontal = Input.GetAxis ("Horizontal");
		OnGround = IsGrounded ();
		HandleMovement (horizontal);
		Flip (horizontal);
		HandleLayers();
		ResetValues ();
	}

	//handles the movement of player
	private void HandleMovement(float horizontal)
	{

		if (MyRigidbody.velocity.y < 0) 
		{
			MyAnimator.SetBool ("land", true);
		}
		if (!Attack && (OnGround || airControl)) {
			if (IsRunning && Mathf.Abs(horizontal)>0.01)
				/* checks if the running button is pressed or not and turns
				the run animation only when both shift and direction keys are pressed*/
			{ 
				MyRigidbody.velocity = new Vector2 (horizontal*WalkingSpeed*2, MyRigidbody.velocity.y);
				MyAnimator.SetBool ("run", true);
				MyAnimator.SetFloat("movementSpeed", WalkingSpeed*2);
				/*speed is case sensitive and horizontal is made positive to compare
				player goes right if speed is greater than 0.01 and left if less than 0.01
				checks horizontal to see if the player pressed left or right button
				speed is greater than 0.01 if the speed if player presses right and vice versa*/
			} else {
				MyRigidbody.velocity = new Vector2 (horizontal*WalkingSpeed, MyRigidbody.velocity.y);
				MyAnimator.SetBool ("run", false);
				MyAnimator.SetFloat("movementSpeed", WalkingSpeed);

			}
		}

		if (Jump && MyRigidbody.velocity.y == 0) {
			MyRigidbody.AddForce (new Vector2 (0, jumpForce));
		}

		MyAnimator.SetFloat ("speed", Mathf.Abs (horizontal));
	}

	//Handles the input keyss
	private void HandleInput()
	{
		if (Input.GetKeyDown(KeyCode.Space)) //GetKeyDown When key is pressed once
		{
			MyAnimator.SetTrigger ("jump");
		}
		if(Input.GetKey(KeyCode.LeftShift)){ //GetKey Checks when key is pressed continuously
			IsRunning = true;
		}

		if (Input.GetKeyDown (KeyCode.V)) {
			MyAnimator.SetTrigger ("throw");
		}
	}



	//Flips the player in opposite direction
	private void Flip(float horizontal)
	{
		if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight) {
			ChangeDirection ();
		}
	}



	//Checks if the player is standing on the ground
	private bool IsGrounded()
	{
		if (MyRigidbody.velocity.y <= 0)
		{
			foreach (Transform point in groundPoints)
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll (point.position, groundRadius, whatIsGround);

				for (int i = 0; i < colliders.Length; i++)
				{
					if (colliders [i].gameObject != gameObject) 
					{	
						return true;
					}
				}
			}	
		}
		return false;
	}

	//resets the values
	private void ResetValues(){
		IsRunning = false;
	}
		
	//Handles the animator layers
	private void HandleLayers()
	{
		if (!OnGround) {
			MyAnimator.SetLayerWeight (1, 1); //Layer weight is set to 1 and Layer number 1 refers to AirLayer
		}else{
			MyAnimator.SetLayerWeight (1, 0); //0 refers to GroundLayer
		}
	}

	//Throws knife
	public void ThrowKnife(int value)
	{
		//makes sure we only throw one knife at a time
		if (!OnGround && value == 1 || OnGround && value == 0) 
		{
			if (facingRight)
			{

				GameObject tmp = (GameObject)Instantiate (knifePrefab, knifePos.position , Quaternion.Euler(new Vector3(180,0,-90))); // rotates the knife
				tmp.GetComponent<Knife>().Initialize(Vector2.right);
			} else 
			{
				GameObject tmp = (GameObject)Instantiate (knifePrefab, knifePos.position, Quaternion.Euler(new Vector3(0,0,90)));
				tmp.GetComponent<Knife>().Initialize(Vector2.left);
			}
		}

	}

    public override IEnumerator TakeDamage()
    {
        return null;
    }
}
