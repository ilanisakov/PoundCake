using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    public int playerNumber = 1;

    public Text cakeAmountText;

    private float cakeAmount = 0;
    private float maxCake = 500f;


	// Use this for initialization
	void Start () {
        cakeAmountText.text = "0";
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public bool EatCake(int value)
    {
        if (cakeAmount < maxCake)
        {
            cakeAmount = Mathf.Min(cakeAmount + value, maxCake);
            cakeAmountText.text = cakeAmount.ToString();
        }
        else
            return false;

        return true;
    }
}
