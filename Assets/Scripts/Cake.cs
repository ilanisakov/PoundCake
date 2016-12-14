using UnityEngine;
using System.Collections;

public class Cake : MonoBehaviour
{

    public int cakeValue = 20;

    public bool isProjectile;
    public GameObject splatterParticle;
    public GameObject splatterSprite;

    private Transform myTransform;
    private Color myColor;
    private PolygonCollider2D myCollider;
    private Quaternion quat;
    private Rigidbody2D rigidbody;
    private Vector2 velocity;
    private int size = 1;

    // Use this for initialization
    void Start()
    {
        myTransform = this.GetComponent<Rigidbody2D>().transform;
        myColor = this.GetComponent<SpriteRenderer>().color;
        myColor.a = 0.45f;
        splatterSprite.GetComponent<SpriteRenderer>().color = myColor;
        splatterParticle.GetComponent<ParticleSystem>().startColor = myColor;

        myCollider = this.GetComponent<PolygonCollider2D>();
        GameObject toIgnore = GameObject.Find("DecorationColliders");
        BoxCollider2D[] ignoringColliders = toIgnore.GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D col in ignoringColliders)
        {
            Physics2D.IgnoreCollision(col, myCollider);
        }

        rigidbody = this.GetComponent<Rigidbody2D>();

        quat = Quaternion.identity;

    }

    // Update is called once per frame
    void Update()
    {
        

        quat.eulerAngles = new Vector3( 0,0,Random.Range(0, 360));

        if(cakeValue <= 0)
            Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        velocity = rigidbody.velocity;
        switch (coll.gameObject.tag)
        {
            case "Projectile":
                Object part = Instantiate(splatterParticle, myTransform.position, quat);
                Instantiate(splatterSprite, myTransform.position, myTransform.rotation);

                if (isProjectile)
                {
                    Cake cake = coll.gameObject.GetComponent<Cake>();
                    cakeValue -= cake.cakeValue;
                    if (this.cakeValue <= 0)
                    {
                        this.cakeValue = 1;
                        Destroy(gameObject);
                    }
                    Destroy(part, 2);

                    rigidbody.velocity = this.velocity;
                }
                break;
            case "Player":
                Character player = (Character)(coll.gameObject.GetComponent(typeof(Character)));
                Instantiate(splatterSprite, myTransform.position, myTransform.rotation);
                bool ateCake = player.EatCake(cakeValue);
                if (isProjectile)
                    player.takeDamage(cakeValue);
                if (ateCake || isProjectile)
                    Destroy(gameObject);
                break;
            case "Cake":
                //// Destroy(gameObject);
                //Object part2 = Instantiate(splatterParticle, myTransform.position, quat);
                //Destroy(gameObject);
                //Destroy(part2, 0.5f);
                break;
            case "Wall":
                Debug.Log(myTransform.position);
                Instantiate(splatterSprite, myTransform.position, quat);
                Destroy(gameObject);
                break;
            case "FrontWall":
                if (isProjectile)
                {
                    float offset = Random.value * coll.gameObject.GetComponent<BoxCollider2D>().size.y * 0.9f;
                    Vector3 pos = new Vector3(myTransform.position.x, myTransform.position.y + offset, myTransform.position.z);
                    Instantiate(splatterSprite, pos, quat);
                    Destroy(gameObject);
                }
                break;
            default:
                break;
        }
    }


    public void SetCakeValue(int value)
    {
        cakeValue = value;
    }

    public void SetCakeSize(int s)
    {
        size = s;
    }

}
