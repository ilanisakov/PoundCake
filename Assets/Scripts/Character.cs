using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    public int playerNumber = 1;
    public float maxHealth = 100f;
    public GameObject deathParticle;

    public Text cakeAmountText;

    private float cakeAmount = 0;
    private float maxCake = 250f;
    private float health;

    private int weightTier = 1;
    private Transform character;


    Controller controller;


	// Use this for initialization
	void Start () {
        cakeAmountText.text = "0, Health: 0";
        health = maxHealth;
        controller = this.gameObject.GetComponent<Controller>();
        character = this.gameObject.GetComponent<Rigidbody2D>().transform;

    }
	
	// Update is called once per frame
	void Update () {
        updateHud();
        if (health <= 0)
        {
            Instantiate(deathParticle, character.position, transform.rotation);
            health = 1;
        }
            
	}

    public bool EatCake(int value)
    {
        float scale = value / 50.0f;
        //Debug.Log(scale);
        if (cakeAmount < maxCake)
        {
            cakeAmount = Mathf.Min(cakeAmount + value, maxCake);
            this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x + scale, this.gameObject.transform.localScale.y + scale, this.gameObject.transform.localScale.z);
            WeightTier = (int)this.gameObject.transform.localScale.x; //setting weight tier
            //this.GetComponent<Rigidbody2D>().gravityScale = this.gameObject.transform.localScale.x * .5f; //Setting gravity
            //Debug.Log("Player " + playerNumber + ":" + this.gameObject.transform.localScale + " : " + weightTier);
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

    public int WeightTier
    {
        get{ return weightTier; }
        set{
            
            if(value != weightTier)
            {
                if (value > weightTier)
                    controller.cakeSpeed *= 1.15f;
                else
                    controller.cakeSpeed *= .15f;

                weightTier = value;
            }
        }
    }
}
