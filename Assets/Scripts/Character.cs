using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {

    //protected variables can only be accessed from the class itself and the inherited class
    protected bool facingRight;
    [SerializeField]
    protected int health;

    [SerializeField]
	protected float WalkingSpeed{ get; set;}
	public bool Attack{ get; set;}
    [SerializeField]
    private List<string> damageSources;
    public abstract bool IsDead { get; }
    public bool TakingDamage { get; set; }
    public Animator MyAnimator { get; private set; }

    // Use this for initialization
    public virtual void Start () {
		facingRight = true;
		MyAnimator = GetComponent<Animator> ();
		WalkingSpeed=4;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public abstract IEnumerator TakeDamage();

    //implements the way each character dies
    public abstract void Death();

    //makes the character flip direction
    public void ChangeDirection()
	{
		facingRight = !facingRight;
		transform.localScale = new Vector3 (transform.localScale.x *-1, transform.localScale.y,transform.localScale.z);
	}

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(damageSources.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
            // makes sure the bullet hits only one enemy until he is dead
            if (other.tag == "Bullet" && !IsDead) 
            {
                DestroyObject(other.gameObject);
            }
        }
    }

}
