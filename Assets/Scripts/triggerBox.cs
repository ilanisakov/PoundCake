using UnityEngine;
using System.Collections;

public class triggerBox : MonoBehaviour {

    private BoxCollider2D myCollider;

	// Use this for initialization
	void Start () {
        myCollider = this.GetComponent<BoxCollider2D>();
	}
	
    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("In the triggerBox");
        if (myCollider.enabled)
        {
            Debug.Log("collider enabled");
            if(other.tag == "Player")
            {
                other.GetComponent<Character>().takeDamage(10);
            }
        }   
    }
}
