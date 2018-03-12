using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour {
    [SerializeField]
    private Stat health;
    [SerializeField]
    public int knife;
    private KnifeBarScript knifeBar;

	

    private void Awake()
    {
        health.Initialize();
    }


    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            health.CurrentVal -= 10;
            knifeBar.knifeAmount -=1 ;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            health.CurrentVal += 10;
            knifeBar.knifeAmount += 1;
        }
    }

        

}
