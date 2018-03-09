using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour {

    [SerializeField]
    private string targetTag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == targetTag)
        {
            GetComponent<Collider2D>().enabled = false; // makes sure enemy can't hit us twice when switching directions
        }
    }
}
