using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    public int playerNumber = 1;
    public float maxHealth = 100f;

    public Text cakeAmountText;

    private float cakeAmount = 0;
    private float maxCake = 500f;
    private float health;


	// Use this for initialization
	void Start () {
        cakeAmountText.text = "0, Health: 0";
        health = maxHealth;
	}
	
	// Update is called once per frame
	void Update () {
        updateHud();
	}

    public bool EatCake(int value)
    {
        if (cakeAmount < maxCake)
        {
            cakeAmount = Mathf.Min(cakeAmount + value, maxCake);
        }
        else
            return false;

        return true;
    }

    void updateHud()
    {
        cakeAmountText.text = cakeAmount.ToString() + ", Health: " + health;
    }

    public void takeDamage(float damage)
    {
        health = health - damage;
        health = Mathf.Max(health, 0);
    }
}
