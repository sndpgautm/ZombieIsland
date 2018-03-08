using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySight : MonoBehaviour {

    [SerializeField]
    private Enemy enemy;

    void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player") //if enemy sees the player it will make him it's target
        {
            enemy.Target = other.gameObject;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") { //if player exits the box then the target is null
            enemy.Target = null;
        }
    }
}
