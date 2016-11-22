 using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

    public float maxSpeed = 10f;
    public float jumpPower = 5f;
    public float dashPower = 2f;
    public float shotDelay = 1.0f;
    public float hideDelay = 3.0f;
    public float cakeSpeed = 100f;
    public float reticleOffset = 0.5f;
    public Color myColor;
    public GameObject intakeParticles;
    

    public Text debugtext;

    public Rigidbody2D cakeProjectile;
    public Rigidbody2D cakeSheild;

    private int jumps = 2;
    private int playerNum;
    private int currentCakeValue;
    private bool canShoot;
    private bool facingRight = true;
    private bool grounded;
    private bool hidden = false;

    private Animator animator;
    private Character character;
    private Cake shotCake;
    private GameObject reticle;
    private Rigidbody2D myRigidBody;
    private Rigidbody2D thisCakeSheild;

    //BoxCollider2D rightbox;
    //BoxCollider2D leftbox;


    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();

        //Getting the player number from the character class
        character = (Character)this.GetComponent(typeof(Character));
        playerNum = character.playerNumber;
        myColor.a = 1.0f;
        this.GetComponent<SpriteRenderer>().color = myColor;

        canShoot = true;

        reticle = GameObject.Find("Player_" + playerNum + "/Reticle");

        myRigidBody = this.GetComponent<Rigidbody2D>();

        //BoxCollider2D[] hitboxes = this.GetComponentsInChildren<BoxCollider2D>();
        //foreach(BoxCollider2D collider in hitboxes)
        //{
        //    switch (collider.name)
        //    {
        //        case "RightHitbox":
        //            rightbox = collider;
        //            break;
        //        case "LeftHitbox":
        //            leftbox = collider;
        //            break;
        //    }

        //    //collider.enabled = false;
        //}

        shotCake = cakeProjectile.GetComponent<Cake>();

        //Debug.Log(myColor);
    }

    // Always has the same timestamp, so do all the physics here
    void Update()
    {

        //Moving Left and Right
        float moveH = Input.GetAxis("Horizontal_" + playerNum);
        float moveV = Input.GetAxis("Vertical_" + playerNum);

        if (moveH > 0 && !facingRight)
            Flip();
        else if (moveH < 0 && facingRight)
            Flip();

        this.GetComponent<Rigidbody2D>().velocity = new Vector2(moveH * maxSpeed, moveV * maxSpeed);
        //Debug.Log(this.GetComponent<Rigidbody2D>().velocity);
        //if (moveV > 0)
        //    Jump();
        //else if (moveV < 0)
        //    Dive();
        //Jumping

        //if (Input.GetAxis("RightTrigger_" + playerNum) != 0)
        //    Jump();
        //else if (Input.GetAxis("LeftTrigger_" + playerNum) != 0)
        //    Jump();

        if (Input.GetButtonDown("DashRight_" + playerNum))
            Dash(true);
        else if (Input.GetButtonDown("DashLeft_" + playerNum))
            Dash(false);

        // }
        if (Input.GetButtonDown("Attack_" + playerNum))
        {
            Debug.Log("Attack");
            StartCoroutine(shoot(1));
            Attack(moveH,moveV);
        }

        if (Input.GetButtonDown("Fire2_" + playerNum))
        {
            if (hidden)
            {
                unhide();
            }
            else
                StartCoroutine(hide());
        }


        //setCursor();
        float aimH = Input.GetAxis("Horizontal_Aim_" + playerNum);
        float aimV = Input.GetAxis("Vertical_Aim_" + playerNum);

        if(canShoot && (aimH != 0 | aimV != 0))
        {
            if(!grounded || aimV >= 0)
                StartCoroutine(shoot(0));
            
        }
        //Shooting Cake
        //if (Input.GetAxis("RightTrigger_" + playerNum) != 0 && canShoot)
        //{
        //    StartCoroutine(shoot());
        //}

        //if (Input.GetButtonUp("Attack_" + playerNum))
        //{
        //    rightbox.enabled = false;
        //}

        
    }

    void Flip()
    {
        facingRight = !facingRight;
    }

    void Jump()
    { 
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, jumpPower);
            jumps--;
    }
    void Dive()
    {
        this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, -jumpPower);
       
    }
    void Dash(bool right)
    {
        if (right)
            this.GetComponent<Rigidbody2D>().MovePosition(new Vector2(this.GetComponent<Rigidbody2D>().position.x + dashPower, this.GetComponent<Rigidbody2D>().position.y));
        else
            this.GetComponent<Rigidbody2D>().MovePosition(new Vector2(this.GetComponent<Rigidbody2D>().position.x - dashPower, this.GetComponent<Rigidbody2D>().position.y));
    }
    void Attack(float moveH, float moveV)
    {
        ////Attack is Left or Right
        //if(moveH > moveV)
        //{
        //    if(moveH > 0)
        //    {
        //        rightbox.enabled = true;
        //    }
        //    else
        //    {
        //        leftbox.enabled = false;
        //    }
        //}
        //else
        //{
        //    if(moveV < 0)//attack is down
        //    {

        //    }
        //    else if(moveV == 0)//attack is stationary
        //    {
        //        if (GetComponent<AnimationController>().facingRight)
        //        {
        //            rightbox.enabled = true;
        //        }
        //        else
        //        {
        //            leftbox.enabled = false;
        //        }
        //    }
        //    else//attack is up
        //    {

        //    }
        //}
    }
    IEnumerator shoot(int direction)
    {
        createShot(direction);
        canShoot = false;
        yield return new WaitForSeconds(shotDelay);
        canShoot = true;
    }
    IEnumerator hide()
    {
        Instantiate(intakeParticles, this.transform.position, this.transform.rotation);
        yield return new WaitForSeconds(0.15f);
        goHide();
        hidden = true;
        yield return new WaitForSeconds(hideDelay);
        unhide();
        hidden = false;
    }
    void createShot(int direction)
    {
        float aimH = Input.GetAxis("Horizontal_Aim_" + playerNum);
        float aimV = Input.GetAxis("Vertical_Aim_" + playerNum);
        //Debug.Log("Player " + playerNum + ": Horizontal(" + aimH + "): Vertical(" + aimV + ")");
        if (!(aimH == 0 && aimV == 0))
        {
            Quaternion quat = Quaternion.identity;
            float rotation = Mathf.Rad2Deg * Mathf.Atan(aimV / aimH);
            //Debug.Log(rotation);
            //Getting the proper angle to rotate the cake image 
            if (aimV > 0)
            {
                quat = (rotation > 0) ? Quaternion.Euler(0, 0, rotation) : Quaternion.Euler(0, -180, -rotation);
            }
            else if (aimH == -1)
            {
                quat = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                quat = (rotation > 0) ? Quaternion.Euler(0, -180, 360.0f - rotation) : Quaternion.Euler(0, 0, 360.0f + rotation);
            }

            //Setting Cake Value
            currentCakeValue = character.WeightTier * 2;
            shotCake.SetCakeValue(currentCakeValue);

            //Creates the cake and shoots it in the direction the player's right thumbstick
            Rigidbody2D cakeSliceClone = (Rigidbody2D)Instantiate(cakeProjectile, this.transform.position, quat);
            cakeSliceClone.GetComponent<SpriteRenderer>().color = myColor;
            float scale = character.WeightTier * 0.9f;
            cakeSliceClone.gameObject.transform.localScale = new Vector3(scale, scale, 1);
            Physics2D.IgnoreCollision(cakeSliceClone.GetComponent<PolygonCollider2D>(), this.GetComponent<BoxCollider2D>()); //making the cake not collide with the shooter
            Vector3 aimRotation = new Vector3(aimH, aimV);
            aimRotation.Normalize();
            cakeSliceClone.velocity = aimRotation * cakeSpeed;

            //character.EatCake(-currentCakeValue);
        }
    }

    void goHide()
    {
        //Instantiate(intakeParticles, this.transform.position, this.transform.rotation);
        thisCakeSheild = (Rigidbody2D)Instantiate(cakeSheild, this.transform.position, this.transform.rotation);
        this.GetComponent<Renderer>().enabled = false;
        this.GetComponent<BoxCollider2D>().enabled = false;
        myRigidBody.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    void unhide()
    {
        thisCakeSheild.gameObject.SetActive(false);
        this.GetComponent<Renderer>().enabled = true;
        this.GetComponent<BoxCollider2D>().enabled = true;
        myRigidBody.constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
    }

    void setCursor()
    {
        float aimH = Input.GetAxis("Horizontal_Aim_" + playerNum);
        float aimV = Input.GetAxis("Vertical_Aim_" + playerNum);
        
        
        float angle = 0;
        if (!(aimH == 0 && aimV == 0))
        {
            if (!facingRight)
            {
                aimH = -aimH;
                aimV = -aimV;
            }
            float rotation = Mathf.Rad2Deg * Mathf.Atan(aimV / aimH);
           // debugtext.text = "Player " + playerNum + ": Horizontal(" + aimH + "): Vertical(" + aimV + "): Rotation(" + rotation + ")";

            if (aimV > 0)
            {
                angle = (rotation > 0) ? rotation : 180.0f + rotation;
            }
            else if (aimH < 0 && aimV == 0)
            {
                angle = 180.0f;
            }
            else
            {
                angle = (rotation > 0) ? 180.0f + rotation : 360.0f + rotation;
            }

            reticle.transform.position = this.transform.position;
            reticle.transform.rotation = Quaternion.identity;
            reticle.transform.Rotate(transform.forward, angle);
            float offset = reticleOffset;
            offset *= this.gameObject.transform.localScale.x * 1.5f;
            if(!facingRight)
                reticle.transform.Translate(new Vector3(-offset, 0, 0));
            else
                reticle.transform.Translate(new Vector3(offset, 0, 0));

        }
        else
        {
            reticle.transform.position = this.transform.position;
            reticle.transform.rotation = Quaternion.identity;
        }
        
        
    }


    void OnCollisionExit2D(Collision2D coll)
    {
        switch (coll.gameObject.tag)
        {
            case "Ground":
                grounded = false;
                break;
            default:
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        switch (coll.gameObject.tag)
        {
            case "Ground":
                jumps = 2;
                grounded = true;
                break;
            case "Projectile":
                Debug.Log("Ow");
                character.EatCake(coll.gameObject.GetComponent<Cake>().cakeValue);
                character.takeDamage(coll.gameObject.GetComponent<Cake>().cakeValue / 2);
                Destroy(coll.gameObject);
                break;
            default:
                break;
        }
    }


}
