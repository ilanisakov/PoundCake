using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    enum PowerUp{
        Haste,
        Health,
        RapidFire,
        None
    }

    public int playerNumber = 1;
    public int healthRestore;
    public float maxHealth = 100f;
    public float moveSpeedScale;
    public float cakeDelayScale;
    public float cakeSpeedScale;
    public float rapidFireSpeed;
    public float rapidFireDuration;
    public float hasteSpeed;
    public float hasteSpeedDuration;
    public GameObject deathParticle;
    public GameObject powerUpParticle;
    public GameObject HUD;
    public bool isDead;

    private float cakeAmount = 0;
    private float maxCake = 150f;
    private float health;

    private PowerUp currentMove;
    //private bool[] readyMoves;
    private bool moveReady;
    private int numMoves = 3;

    private int weightTier = 1;
    private Transform character;

    private bool dead = false;

    private Controller controller;
    private HUDScript HUDController;

    private GameObject deathparticles;
    private float cakeDecreaseScale;

	// Use this for initialization
	void Start () {

        currentMove = PowerUp.None;
        moveReady = false;
        //readyMoves = new bool[] { false, false, false };

        isDead = false;

        cakeDecreaseScale = -.15f / 50.0f;

    }
	
	// Update is called once per frame
	void Update () {
        updateHud();
        if (health <= 0 && !isDead)
        {
            controller.canShoot = false;
            if (dead)
            {
                
                deathparticles.transform.position = character.position;
                cakeAmount -= .15f;
                this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x + cakeDecreaseScale, this.gameObject.transform.localScale.y + cakeDecreaseScale, this.gameObject.transform.localScale.z);

                if (cakeAmount <= 0)
                    isDead = true;
            }
            else
            {
                deathparticles = (GameObject)Instantiate(deathParticle, character.position, transform.rotation);
                dead = true;
                controller.maxSpeed = 8.0f;
            }
        }
            
	}

    public bool EatCake(int value)
    {
        if (currentMove == PowerUp.RapidFire)
            return false;

        float scale = value / 50.0f;

        //Debug.Log(scale);
        if (value > 0)
        {
            if (cakeAmount != maxCake)
            {
                cakeAmount = Mathf.Min(cakeAmount + value, maxCake);
                this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x + scale, this.gameObject.transform.localScale.y + scale, this.gameObject.transform.localScale.z);
                WeightTier = (int)this.gameObject.transform.localScale.x; //setting weight tier
                //this.GetComponent<Rigidbody2D>().gravityScale = this.gameObject.transform.localScale.x * .5f; //Setting gravity
                //Debug.Log("Player " + playerNumber + ":" + this.gameObject.transform.localScale + " : " + weightTier);
                return true;
            }
        }
        else
        {
            if (cakeAmount != 0)
            {
                cakeAmount = Mathf.Max(cakeAmount + value, 0);
                this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x + scale, this.gameObject.transform.localScale.y + scale, this.gameObject.transform.localScale.z);
                WeightTier = (int)this.gameObject.transform.localScale.x; //setting weight tier
                //this.GetComponent<Rigidbody2D>().gravityScale = this.gameObject.transform.localScale.x * .5f; //Setting gravity
                //Debug.Log("Player " + playerNumber + ":" + this.gameObject.transform.localScale + " : " + weightTier);
                return true;
            }
        }
        return false;

        
    }

    void updateHud()
    {
        //cakeAmountText.text = cakeAmount.ToString() + ", Health: " + health;
        HUDController.UpdateHealth(health, maxHealth);
        HUDController.UpdateCake(cakeAmount / maxCake, maxCake);
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

            if (value != weightTier && value > 0)
            {
                if (value > weightTier)
                {
                    controller.cakeSpeed *= cakeSpeedScale;
                    controller.maxSpeed *= moveSpeedScale;

                    controller.shotDelay += cakeDelayScale;
                }
                else
                {
                    controller.cakeSpeed /= cakeSpeedScale;
                    controller.maxSpeed /= moveSpeedScale;

                    controller.shotDelay -= cakeDelayScale;
                }

                weightTier = value;
            }
        }
    }

    public void SetUp()
    {
        health = maxHealth;
        controller = this.gameObject.GetComponent<Controller>();
        character = this.gameObject.GetComponent<Rigidbody2D>().transform;
        HUDController = HUD.GetComponent<HUDScript>();
        HUDController.ChangeColor(this.GetComponent<SpriteRenderer>().color);
    }

    public Color getColor()
    {
        return this.GetComponent<SpriteRenderer>().color;
    }

    public void newPowerUp()
    {
        if (!moveReady && currentMove == PowerUp.None)
        {
            int moveNum = Random.Range(0, numMoves);
            switch (moveNum)
            {
                //Haste
                case 0:
                    currentMove = PowerUp.Haste;
                    powerUpParticle.GetComponent<ParticleSystem>().startColor = new Color(0, 0, 1, 1);
                    Object g = Instantiate(powerUpParticle, transform.position, transform.rotation);
                    Destroy(g, 1);
                    HUDController.PowerUpHUD(0, hasteSpeedDuration);
                    break;
                //Health
                case 1:
                    currentMove = PowerUp.Health;
                    powerUpParticle.GetComponent<ParticleSystem>().startColor = new Color(1, 0, 0, 1);
                    Object v = Instantiate(powerUpParticle, transform.position, transform.rotation);
                    Destroy(v, 1);
                    HUDController.PowerUpHUD(1, 1.0f);
                    break;
                //Rapid Fire
                case 2:
                    currentMove = PowerUp.RapidFire;
                    powerUpParticle.GetComponent<ParticleSystem>().startColor = new Color(0, 1, 0, 1);
                    Object b = Instantiate(powerUpParticle, transform.position, transform.rotation);
                    Destroy(b, 1);
                    HUDController.PowerUpHUD(2, rapidFireDuration);
                    break;
                default:
                    break;
            };
            moveReady = true;

            Debug.Log("CurrentMove = " + currentMove);


            
            UseMove();
        }
    }

    public void UseMove()
    {
        if (moveReady)
        {
            moveReady = false;
            switch (currentMove)
            {
                //Haste
                case PowerUp.Haste:
                    StartCoroutine(Haste());
                    break;
                //Health
                case PowerUp.Health:
                    HealthBoost();
                    break;
                //Rapid Fire
                case PowerUp.RapidFire:
                    StartCoroutine(RapidFire());
                    break;
                default:
                    break;
            };
            
        }
    }

    IEnumerator Haste()
    {
        float oldSpeed = controller.maxSpeed;
        controller.maxSpeed = hasteSpeed;
        yield return new WaitForSeconds(hasteSpeedDuration);
        controller.maxSpeed = oldSpeed;
        currentMove = PowerUp.None;
    }

    IEnumerator RapidFire()
    {
        float oldSpeed = controller.shotDelay;
        controller.shotDelay = oldSpeed / 2.0f;
        yield return new WaitForSeconds(rapidFireDuration);
        controller.shotDelay = oldSpeed;
        currentMove = PowerUp.None;
    }

    private void HealthBoost()
    {
        health = Mathf.Min(health + healthRestore, maxHealth);
        currentMove = PowerUp.None;
    }
}
