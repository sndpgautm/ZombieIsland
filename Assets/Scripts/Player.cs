using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private Rigidbody2D myRigidbody;


	[SerializeField]
	private float movementSpeed;


	// Use this for initialization
	void Start () {
	
		myRigidbody = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		float horizontal = Input.GetAxis ("Horizontal");

		HandleMovement (horizontal);
	}

	//handles the movement of player
	private void HandleMovement(float horizontal)
	{
		myRigidbody.velocity = new Vector2 (horizontal*movementSpeed, myRigidbody.velocity.y);
	}
}
