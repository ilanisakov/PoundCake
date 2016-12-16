using UnityEngine;
using System.Collections;

public class PowerUP : MonoBehaviour
{

    public int cakeValue;
    public float downTime = 2.0f;

    private GameObject CakeImage;
    private GameObject OrbImage;
    private bool active;

    // Use this for initialization
    void Start()
    {
        OrbImage = this.transform.GetChild(0).gameObject;
        CakeImage = this.transform.GetChild(1).gameObject;
        active = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (active)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(disable());
                Character c = other.gameObject.GetComponent<Character>();
                c.EatCake(cakeValue);
                c.newPowerUp();
                
            }
        }
    }

    IEnumerator disable()
    {
        hide(false);
        yield return new WaitForSeconds(downTime);
        hide(true);

    }

    private void hide(bool showing)
    {
        OrbImage.SetActive(showing);
        CakeImage.SetActive(showing);
        active = showing;
    }
}
