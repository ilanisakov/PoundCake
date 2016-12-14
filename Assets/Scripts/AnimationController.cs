using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

    private int playerNum;
    private Character character;

    Animator animator;

    public bool facingRight = true;

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();

        //Getting the player number from the character class
        character = (Character)this.GetComponent(typeof(Character));
        playerNum = character.playerNumber;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        float move = Input.GetAxis("Horizontal_" + playerNum);
        float moveV = Input.GetAxis("Vertical_" + playerNum);

        //Changing to Idle animation if not moving
        if (move == 0 && moveV == 0)
            animator.SetBool("Walking", false);
        else
            animator.SetBool("Walking", true);
        
        //Flipping sprite if facing the other way
        if (move > 0 && !facingRight)
            Flip();
        else if (move < 0 && facingRight)
            Flip();


    }

    void Update()
    {
        //if (Input.GetButtonDown("Attack_" + playerNum))
        //{
        //    animator.SetBool("Punching", true);
        //}
        //if (Input.GetButtonUp("Attack_" + playerNum))
        //{
        //    animator.SetBool("Punching", false);
        //}

    }

    //Flips the Sprite so it can be used when moving left or right
    void Flip()
    {
        facingRight = !facingRight;
        //Vector3 scale = transform.localScale;
        //scale.x *= -1;
        //transform.localScale = scale;
        this.GetComponent<SpriteRenderer>().flipX = !facingRight;
    }

    void OnCollisionExit2D(Collision2D coll)
    {
        switch (coll.gameObject.tag)
        {
            case "Ground":
                animator.SetBool("Grounded", false);
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
                animator.SetBool("Grounded", true);
                break;
            default:
                break;
        }
    }

    bool getFacingRight()
    {
        return facingRight;
    }
}
