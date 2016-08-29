using UnityEngine;


public class Controller : MonoBehaviour {

    public float maxSpeed = 10f;
    public float jumpPower = 5f;

    public float cakeSpeed = 100f;

    public Rigidbody2D cakeProjectile;

    private int jumps = 2;
    private int playerNum;

    private Animator animator;
    private Character character;

    


    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();

        //Getting the player number from the character class
        character = (Character)this.GetComponent(typeof(Character));
        playerNum = character.playerNumber;
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
        //Shooting Cake
        if(Input.GetAxis("RightTrigger_" + playerNum) != 0)
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
                else if(aimH == -1)
                {
                    quat = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    quat = (rotation > 0) ? Quaternion.Euler(0,-180, 360.0f - rotation) : Quaternion.Euler(0, 0, 360.0f + rotation);
                }
                
                //Creates the cake and shoots it in the direction the player's right thumbstick
                Rigidbody2D cakeSliceClone = (Rigidbody2D)Instantiate(cakeProjectile, transform.position, quat);
                Vector3 aimRotation = new Vector3(aimH, aimV);
                aimRotation.Normalize();
                cakeSliceClone.velocity = aimRotation * cakeSpeed;
            }
        }

    }

    void Jump()
    {
            this.GetComponent<Rigidbody2D>().velocity = new Vector2(this.GetComponent<Rigidbody2D>().velocity.x, jumpPower);
            jumps--;
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
            default:
                break;
        }
    }
}
