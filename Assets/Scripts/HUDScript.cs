using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour {

    private Image cakeBar;
    private Image healthBar;
    private Image banner;
    private GameObject[] flames;
    private Text healthText;
    private float cakeAmount;
    private float healthAmount;
    private float maxHealth, minHealth;
    private Color flameColor;

	// Use this for initialization
	void Start () {

        GameObject child = this.transform.GetChild(4).gameObject;

        healthBar = child.transform.Find("HealthBar").GetComponent<Image>();
        cakeBar = child.transform.Find("CakeBar_Filling").GetComponent<Image>();
        healthText = child.transform.Find("HealthText").GetComponent<Text>();
        banner = this.gameObject.GetComponentInChildren<Image>();

        flames = new GameObject[3];

        for(int i = 0; i < 3; i++)
        {
            flames[i] = this.transform.GetChild(i + 1).gameObject;
            //Color c = flames[i].GetComponent<Image>().color;
            //c[3] = .5f;
            flames[i].GetComponent<Image>().color = Color.black;
        }

        maxHealth = .91f;
        minHealth = .091f;

        cakeAmount = 0;
        healthAmount = maxHealth;

        flameColor = Color.black;
	}
	
	// Update is called once per frame
	void Update () {
        healthBar.fillAmount = healthAmount;
        cakeBar.fillAmount = cakeAmount;
	}

    public void UpdateHealth(float health, float maxhealth)
    {
        float healthP = health / maxhealth;
        healthAmount = Remap(healthP, 0, maxHealth, minHealth, maxHealth);
        healthText.text = health.ToString();
    }
    public void UpdateCake(float cakeP, float maxCake)
    {
        cakeAmount = cakeP;
    }

    public void ChangeColor(Color c)
    {
        Color newC = c;
        cakeBar.color = newC;
        newC.a = 0.55f;
        banner.color = newC;
        
    }

    private float Remap (float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    public void PowerUpHUD(int power, float cooldown)
    {
        //0 haste, 1 health, 2 rapid fire
        switch (power)
        {
            case 0:
                flameColor = Color.blue;
                break;
            case 1:
                flameColor = Color.red;
                break;
            case 2:
                flameColor = Color.green;
                break;
            default:
                flameColor = Color.black;
                break;
        };

        foreach(GameObject f in flames)
        {
            f.GetComponent<Image>().color = flameColor;
        }

        StartCoroutine(PowerDown(cooldown));

    }

    IEnumerator PowerDown(float cooldown)
    {
        float div = cooldown / 3.0f;
        yield return new WaitForSeconds(div);
        flames[2].GetComponent<Image>().color = Color.black;
        yield return new WaitForSeconds(div);
        flames[1].GetComponent<Image>().color = Color.black;
        yield return new WaitForSeconds(div);
        flames[0].GetComponent<Image>().color = Color.black;
    }
}
