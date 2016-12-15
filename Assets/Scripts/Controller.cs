 using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

    public float maxSpeed = 1.0f;
    public float jumpPower = 1.0f;
    public float dashPower = 1.0f;
    public float shotDelay = 1.0f;
    public float hideDelay = 1.0f;
    public float cakeSpeed = 1.0f;
    public float reticleOffset = 0.5f;
    public int cakeValue = 1;
    public Color myColor;
    public GameObject intakeParticles;
    public GameObject hitParticle;

    public Rigidbody2D cakeProjectile;
    public Rigidbody2D cakeSheild;

    private int jumps = 2;
    private int playerNum;
    private int currentCakeValue;
    public bool canShoot;
    private bool facingRight = true;
    private bool grounded;
    private bool hidden = false;

    private Animator animator;
    private Character character;
    private Cake shotCake;
    private GameObject reticle;
    private Rigidbody2D myRigidBody;
    private Rigidbody2D thisCakeSheild;
    private BoxCollider2D myBoxCollider;
    private Color cakeColor;

    //BoxCollider2D rightbox;
    //BoxCollider2D leftbox;


    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();

        //Getting the player number from the character class
        character = (Character)this.GetComponent(typeof(Character));
        playerNum = character.playerNumber;
        myColor = character.getColor();
        if (myColor != Color.white)
            cakeColor = myColor;
        else
            cakeColor = new Color(.945f, .412f, 1.0f);
        //this.GetComponent<SpriteRenderer>().color = myColor;

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

        myBoxCollider = this.GetComponent<BoxCollider2D>();
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

        myRigidBody.velocity = new Vector2(moveH * maxSpeed, moveV * maxSpeed);

        float aimH = Input.GetAxis("Horizontal_Aim_" + playerNum);
        float aimV = Input.GetAxis("Vertical_Aim_" + playerNum);


        if (canShoot && (aimH != 0 | aimV != 0))
        {
            if(!grounded || aimV >= 0)
                StartCoroutine(shoot(0));
            
        }
        
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
    void Dash(float H, float V)
    {
            this.GetComponent<Rigidbody2D>().MovePosition(new Vector2(this.GetComponent<Rigidbody2D>().position.x + (dashPower * H), this.GetComponent<Rigidbody2D>().position.y + (dashPower * V)));
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

        //if (playerNum == 2)
        //    aimH = 1;
        //Debug.Log("Player " + playerNum + ": Horizontal(" + aimH + "): Vertical(" + aimV + ")");
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
        double temp = System.Math.Pow(cakeValue, character.WeightTier);
        currentCakeValue = (int)temp;
        shotCake.SetCakeValue(currentCakeValue);

        //Creates the cake and shoots it in the direction the player's right thumbstick
        Rigidbody2D cakeSliceClone = (Rigidbody2D)Instantiate(cakeProjectile, this.transform.position, quat);
        cakeSliceClone.GetComponent<SpriteRenderer>().color = cakeColor;
        float scale = character.WeightTier * 0.45f;
        cakeSliceClone.gameObject.transform.localScale = new Vector3(scale, scale, 1);
        cakeSliceClone.GetComponent<Cake>().SetCakeSize(character.WeightTier);
        Physics2D.IgnoreCollision(cakeSliceClone.GetComponent<PolygonCollider2D>(), myBoxCollider); //making the cake not collide with the shooter
        Vector3 aimRotation = new Vector3(aimH, aimV);
        aimRotation.Normalize();
        cakeSliceClone.velocity = aimRotation * cakeSpeed;
        Destroy(cakeSliceClone, 3);

        character.EatCake(-character.WeightTier);
        
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
                //Debug.Log("Ow");
                character.EatCake(coll.gameObject.GetComponent<Cake>().cakeValue);
                Object g = Instantiate(hitParticle, this.transform.position, transform.rotation);
                Destroy(g, 1);
                Destroy(coll.gameObject);
                break;
            default:
                break;
        }
    }


}
