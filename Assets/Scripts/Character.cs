using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

	//protected variables can only be accessed from the class itself and the inherited class
	protected Animator myAnimator;
	protected bool facingRight;
	[SerializeField]
	protected float WalkingSpeed{ get; set;}
	public bool Attack{ get; set;}


	// Use this for initialization
	public virtual void Start () {
		facingRight = true;
		myAnimator = GetComponent<Animator> ();
		WalkingSpeed=4;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	//makes the character flip direction
	public void ChangeDirection()
	{
		facingRight = !facingRight;
		transform.localScale = new Vector3 (transform.localScale.x *-1, transform.localScale.y,transform.localScale.z);
	}
}
