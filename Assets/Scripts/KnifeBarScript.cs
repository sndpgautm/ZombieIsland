using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeBarScript : MonoBehaviour {


    [SerializeField]
    private Text valueText;

    private int knifes;



	
	// Update is called once per frame
	void Update () {
        HandleBar();
		
	}

    private void HandleBar()
    {
        knifes = Player.Instance.knifeAmount;
        valueText.text = this.knifes.ToString();
    }

}
