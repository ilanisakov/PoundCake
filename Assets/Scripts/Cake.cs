using UnityEngine;
using System.Collections;

public class Cake : MonoBehaviour {

    public int cakeValue = 20;

    public bool isProjectile;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        switch (coll.gameObject.tag)
        {
            case "Player":
                Character player = (Character)(coll.gameObject.GetComponent(typeof(Character)));
                bool ateCake = player.EatCake(cakeValue);
                if (isProjectile)
                    player.takeDamage(5);
                if (ateCake || isProjectile)
                    Destroy(gameObject);
                break;
            case "Ground":
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }

}
