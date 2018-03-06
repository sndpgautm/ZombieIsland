using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour {

    [SerializeField]
    private Collider2D other;



	// Use this for initialization
	private void Awake () {
        //ignores collison between it's own collider and other collider
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other, true);
	}
	
}
