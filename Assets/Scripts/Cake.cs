using UnityEngine;
using System.Collections;

public class Cake : MonoBehaviour {

    public int cakeValue = 20;

    public bool isProjectile;
    public GameObject splatterParticle;
    public GameObject splatterSprite;

    private Transform myTransform;
    private Color myColor;
    private PolygonCollider2D myCollider;

	// Use this for initialization
	void Start () {
        myTransform = this.GetComponent<Rigidbody2D>().transform;
        myColor = this.GetComponent<SpriteRenderer>().color;
        myColor.a = 0.45f;

        splatterSprite.GetComponent<SpriteRenderer>().color = myColor;
        splatterParticle.GetComponent<ParticleSystem>().startColor = myColor;

        myCollider = this.GetComponent<PolygonCollider2D>();
        GameObject toIgnore = GameObject.Find("DecorationColliders");
        BoxCollider2D[] ignoringColliders = toIgnore.GetComponents<BoxCollider2D>();
        foreach(BoxCollider2D col in ignoringColliders)
        {
            Physics2D.IgnoreCollision(col, myCollider);
        }
        
        

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        switch (coll.gameObject.tag)
        {
            case "Projectile":
                Debug.Log("Sploosh");
                Object part = Instantiate(splatterParticle, myTransform.position, myTransform.rotation);
                Instantiate(splatterSprite, myTransform.position, myTransform.rotation);
                Destroy(gameObject);
                Destroy(coll.gameObject);
                Destroy(part,2);
                break;
            case "Player":
                Character player = (Character)(coll.gameObject.GetComponent(typeof(Character)));
                bool ateCake = player.EatCake(cakeValue);
                if (isProjectile)
                    player.takeDamage(5);
                if (ateCake || isProjectile)
                    Destroy(gameObject);
                break;
            case "Cake":
                // Destroy(gameObject);
                Object part2 = Instantiate(splatterParticle, myTransform.position, myTransform.rotation);
                Destroy(gameObject);
                Destroy(part2, 2);
                break;
            case "Wall":
                Debug.Log(myTransform.position);
                Instantiate(splatterSprite, myTransform.position, myTransform.rotation);
                Destroy(gameObject);
                break;
            case "FrontWall":
                float offset = Random.value * coll.gameObject.GetComponent<BoxCollider2D>().size.y * 0.9f;
                Vector3 pos = new Vector3(myTransform.position.x, myTransform.position.y + offset, myTransform.position.z);
                Instantiate(splatterSprite, pos, myTransform.rotation);
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }

    public void SetCakeValue(int value)
    {
        cakeValue = value;
    }

}
