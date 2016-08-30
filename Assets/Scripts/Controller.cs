using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

    public float maxSpeed = 10f;
    public float jumpPower = 5f;
    public float shotDelay = 1.0f;
    public float cakeSpeed = 100f;
    public float reticleOffset = 0.5f;

    public Text debugtext;

    public Rigidbody2D cakeProjectile;

    private int jumps = 2;
    private int playerNum;
    private bool canShoot;
    private bool facingRight = true;

    private Animator animator;
    private Character character;
    private Cake shotCake;
    private GameObject reticle;


    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();

        //Getting the player number from the character class
        character = (Character)this.GetComponent(typeof(Character));
        playerNum = character.playerNumber;

        canShoot = true;

        reticle = GameObject.Find("Player_" + playerNum + "/Reticle");
    }

    // Always has the same timestamp, so do all the physics here
    void Update()
    {

        //Moving Left and Right
        float move = Input.GetAxis("Horizontal_" + playerNum);

        this.GetComponent<Rigidbody2D>().velocity = new Vector2(move * maxSpeed, this.GetComponent<Rigidbody2D>().velocity.y);
        //Jumping
        if (Input.GetButtonDown("Jump_" + playerNum) && jumps != 0) 
        {
            Jump();
        }

        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();

        setCursor();

        //Shooting Cake
        if (Input.GetAxis("RightTrigger_" + playerNum) != 0 && canShoot)
        {
            StartCoroutine(shoot());
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
    IEnumerator shoot()
    {
        createShot();
        canShoot = false;
        yield return new WaitForSeconds(shotDelay);
        canShoot = true;
    }
    void createShot()
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

            //Creates the cake and shoots it in the direction the player's right thumbstick
            Rigidbody2D cakeSliceClone = (Rigidbody2D)Instantiate(cakeProjectile, reticle.transform.position, quat);
            Vector3 aimRotation = new Vector3(aimH, aimV);
            aimRotation.Normalize();
            cakeSliceClone.velocity = aimRotation * cakeSpeed;
        }
    }

    void setCursor()
    {
        float aimH = Input.GetAxis("Horizontal_Aim_" + playerNum);
        float aimV = Input.GetAxis("Vertical_Aim_" + playerNum);
        
        float angle = 0;
        if (!(aimH == 0 && aimV == 0))
        {

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
            if(!facingRight)
                reticle.transform.Translate(new Vector3(-reticleOffset, 0, 0));
            else
                reticle.transform.Translate(new Vector3(reticleOffset, 0, 0));

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
                break;
            case "Projectile":
                Debug.Log("Ow");
                break;
            default:
                break;
        }
    }


}
