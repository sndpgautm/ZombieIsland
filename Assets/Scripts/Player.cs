using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void DeadEventHandler();

public class Player : Character {


	private static Player instance;

    public event DeadEventHandler Dead; // enemy can listen to this dead event and when it it triggered enemy knows player is dead

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

    private bool immortal = false;  //player gets some time where he is immortal when he takes damage
    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float immortalTime;

	//properties are written different to variables starting with capital letter
	public Rigidbody2D MyRigidbody { get; set;}
	public bool Jump { get; set;}
	public bool OnGround{ get; set;}
	public bool IsRunning{ get; set;}

    public override bool IsDead
    {
        get
        {
            if (health <= 0)
            {
                OnDead();
            }
            
            return health <= 0; //returns true
        }
    }

    private Vector2 startPos;


	// Use this for initialization
	public override void Start () {
		base.Start();

        startPos = transform.position;
        IsRunning = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        MyRigidbody = GetComponent<Rigidbody2D> ();

	}
	
	// Update is called once per frame
	void Update(){

        if (!TakingDamage && !IsDead) // makes sure we cannot move when we take hit from enemy
        {
            if (transform.position.y <= -14f)
            {
                Death();
            }

            HandleInput();
        }


		
	}




	void FixedUpdate () 
	{
        if (!TakingDamage && !IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");
            OnGround = IsGrounded();
            HandleMovement(horizontal);
            Flip(horizontal);
            HandleLayers();
            ResetValues();
        }
		
	}

    public void OnDead()
    {
        if (Dead != null)
        {
            Dead();
        }
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


    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);

        }
    }

    public override IEnumerator TakeDamage()
    {

        if (!immortal)
        {
            health -= 10;

            if (!IsDead)
            {
                MyAnimator.SetTrigger("damage");
                immortal = true;
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);

                immortal = false;
            }
            else
            {
                MyAnimator.SetLayerWeight(1, 0); //makes sure player is in ground no matter which layer he dies
                MyAnimator.SetTrigger("die");
            }


        }

    }

    //used to reswpan player
    public override void Death()
    {
        MyRigidbody.velocity = Vector2.zero; // makes sure player does not move when respwaning
        MyAnimator.SetTrigger("idle");
        health = 50;
        transform.position = startPos;
    }
}
