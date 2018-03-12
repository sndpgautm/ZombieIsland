using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeBarScript : MonoBehaviour {


    [SerializeField]
    private Text valueText;
    public int knifeAmount;



	
	// Update is called once per frame
	void Update () {
        HandleBar();
		
	}

    private void HandleBar()
    {
        knifeAmount = 3;
        valueText.text = this.knifeAmount.ToString();
    }

}
