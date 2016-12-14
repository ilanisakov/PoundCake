using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PreGame : MonoBehaviour {


    //private string ConnectController = "---Connect a Controller---";
    //private string JoinGame = "---Press A to Join---";

    //Notes: Change bool to an ENUM

    public GameObject playerPrefab;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject playerPrefab4;

    public GameObject middleText;

    //Player Panel Struct
    private struct PlayerPanel
    {
        public GameObject colorPicker;
        public Image myBackground;
        public Image myIcon;
        public Image myPointer;
        public Text myText;
        public Color myColor;
        public int currentColor;
        public bool canGoNext;
        private bool inUse;
        private bool connected;
        private bool ready;
        public bool Connected
        {
            get { return connected; }
            set
            {
                if (!value)
                    myText.text = "---Connect a Controller---";
                else
                {
                    if(!InUse && !ready)
                        myText.text = "---Press A to Join---";
                }
                connected = value;
            }
        }
        public bool InUse
        {
            get { return inUse; }
            set
            {
                inUse = value;

                if(inUse)
                {
                    myText.text = "";
                    myIcon.enabled = true;
                    colorPicker.SetActive(true);
                    myPointer.enabled = true;
                    
                }
                else
                {
                    if(ready)
                    {
                        myText.text = "READY";
                    }
                    else
                        myIcon.enabled = false;

                    myPointer.enabled = false;
                    colorPicker.SetActive(false);
                }
            }
        }
        public bool Ready
        {
            get { return ready; }
            set
            {
                ready = value;
                if(ready)
                {
                    myBackground.color = myIcon.color;
                }
                else
                {
                    myBackground.color = new Color(1.0f, 1.0f, 1.0f, 0.6667f);
                }
            }
        }
    }

    private int currentConnectedControllers;
    private int maxColors = 6;

    private PlayerPanel[] playerPanels;
    private ArrayList selectedColors;

    // Use this for initialization
    void Start () {
        playerPanels = new PlayerPanel[4];

        //setting color locks
        selectedColors = new ArrayList();

        for(int i = 0; i < 4; i++)
        {
            string panelName = "Player" + (i + 1) +"Panel";
            playerPanels[i] = new PlayerPanel();
            playerPanels[i].myBackground = GameObject.Find("Canvas/" + panelName).GetComponent<Image>();
            playerPanels[i].myText = GameObject.Find("Canvas/" + panelName + "/Text").GetComponent<Text>();
            playerPanels[i].myIcon = GameObject.Find("Canvas/" + panelName + "/PlayerIcon").GetComponent<Image>();
            playerPanels[i].myPointer = GameObject.Find("Canvas/" + panelName + "/ColorPicker/Pointer").GetComponent<Image>();
            playerPanels[i].myPointer.rectTransform.localPosition = new Vector3(playerPanels[i].myPointer.rectTransform.localPosition.x + 75, playerPanels[i].myPointer.rectTransform.localPosition.y, 0);
            playerPanels[i].colorPicker = GameObject.Find("Canvas/" + panelName + "/ColorPicker");
            playerPanels[i].colorPicker.SetActive(false);
            playerPanels[i].myIcon.enabled = false;
            playerPanels[i].myPointer.enabled = false;
            playerPanels[i].InUse = false;
            playerPanels[i].Connected = false;
            playerPanels[i].canGoNext = true;
            playerPanels[i].currentColor = 0;
            playerPanels[i].myColor = new Color(0,0,0);



        }

        UpdateCurrentControllers();
    }
	
	// Update is called once per frame
	void Update () {
        UpdateCurrentControllers();

        checkReady();

        for(int i = 0; i < 4; i++)
        {
            PlayerPanel p = playerPanels[i];
            if(p.InUse)
            {
                if(p.canGoNext && Input.GetAxis("Horizontal_" + (i+1)) != 0)
                {
                    StartCoroutine(nextColor(i, Input.GetAxis("Horizontal_" + (i + 1))));
                }

            }
        }

        //Press A to Enter Game
        if(Input.GetButtonDown("Submit_1") && !playerPanels[0].Ready)
        {
            if (!playerPanels[0].InUse)
            {
                playerPanels[0].InUse = true;
                if (selectedColors.Contains(playerPanels[0].currentColor))
                    goNextColor(0, 1);    
            }
            else
            {
                playerPanels[0].Ready = true;
                selectedColors.Add(playerPanels[0].currentColor);
                playerPanels[0].InUse = false;
            }

        }
        else if (Input.GetButtonDown("Submit_2") && !playerPanels[1].Ready)
        {
            if (!playerPanels[1].InUse)
            {
                playerPanels[1].InUse = true;
                if (selectedColors.Contains(playerPanels[1].currentColor))
                    goNextColor(1, 1);
            }
            else
            {
                playerPanels[1].Ready = true;
                selectedColors.Add(playerPanels[1].currentColor);
                playerPanels[1].InUse = false;
            }
        }
        else if (Input.GetButtonDown("Submit_3") && !playerPanels[2].Ready)
        {
            if (!playerPanels[2].InUse)
            {
                playerPanels[2].InUse = true;
                if (selectedColors.Contains(playerPanels[2].currentColor))
                    goNextColor(2, 1);
            }
            else
            {
                playerPanels[2].Ready = true;
                selectedColors.Add(playerPanels[1].currentColor);
                playerPanels[2].InUse = false;
            }
        }
        else if (Input.GetButtonDown("Submit_4") && !playerPanels[3].Ready)
        {
            if (!playerPanels[3].InUse)
            {
                playerPanels[3].InUse = true;
                if (selectedColors.Contains(playerPanels[3].currentColor))
                    goNextColor(3, 1);
            }
            else
            {
                playerPanels[3].Ready = true;
                selectedColors.Add(playerPanels[1].currentColor);
                playerPanels[3].InUse = false;
            }
        }

        //Press B to Back Out
        if (Input.GetButtonDown("Fire2_1"))
        {
            if (playerPanels[0].InUse)
                playerPanels[0].InUse = false;
            else
            {
                playerPanels[0].Ready = false;
                selectedColors.Remove(playerPanels[0].currentColor);
                playerPanels[0].InUse = true;
            }
        }
        else if (Input.GetButtonDown("Fire2_2"))
        {
            if (playerPanels[1].InUse)
                playerPanels[1].InUse = false;
            else
            {
                playerPanels[1].Ready = false;
                selectedColors.Remove(playerPanels[1].currentColor);
                playerPanels[1].InUse = true;
            }
        }
        else if (Input.GetButtonDown("Fire2_3"))
        {
            if (playerPanels[2].InUse)
                playerPanels[2].InUse = false;
            else
            {
                playerPanels[2].Ready = false;
                selectedColors.Remove(playerPanels[2].currentColor);
                playerPanels[2].InUse = true;
            }
        }
        else if (Input.GetButtonDown("Fire2_4"))
        {
            if (playerPanels[3].InUse)
                playerPanels[3].InUse = false;
            else
            {
                playerPanels[3].Ready = false;
                selectedColors.Remove(playerPanels[3].currentColor);
                playerPanels[3].InUse = true;
            }
        }

        //Pressing Start
        if (Input.GetButtonDown("Start") && checkReady())
        {
            PlayerPrefs.SetInt("numberPlayers", currentConnectedControllers);


            playerPrefab.GetComponent<SpriteRenderer>().color = playerPanels[0].myIcon.color;
            playerPrefab2.GetComponent<SpriteRenderer>().color = playerPanels[1].myIcon.color;
            playerPrefab3.GetComponent<SpriteRenderer>().color = playerPanels[2].myIcon.color;
            playerPrefab4.GetComponent<SpriteRenderer>().color = playerPanels[3].myIcon.color;

            SceneManager.LoadScene("TEST");
        }
    }

    void UpdateCurrentControllers()
    {
        currentConnectedControllers = 0;
        int currentController = 0;
        foreach (string name in Input.GetJoystickNames())
        {
            if (name == "Controller (Xbox One For Windows)")
            {
                playerPanels[currentController].Connected = true;
                currentConnectedControllers++;
            }
            else if(name == "")
            {
                playerPanels[currentController].Connected = false;
            }

            if(currentController < 3)
                currentController++;
                
        }
        
    }

    IEnumerator nextColor(int player, float direction)
    {
        goNextColor(player,direction);
        playerPanels[player].canGoNext = false;
        yield return new WaitForSeconds(0.25f);
        playerPanels[player].canGoNext = true;
    }

    void goNextColor(int player, float direction)
    {
        int curColor = playerPanels[player].currentColor;

        if (player == 0 || player == 2)
        {
            if (direction > 0)//moving right
            {
                
                do
                {
                    curColor++;
                    if (curColor >= maxColors)
                    {
                        curColor = 0;
                    }
                } while (selectedColors.Contains(curColor));

                
            }
            else
            {
                do
                {
                    curColor--;
                    if (curColor < 0)
                    {
                        curColor = 5;
                    }
                } while (selectedColors.Contains(curColor));
                
            }
        }
        else
        {
            if (direction > 0)//moving left
            {

                do
                {
                    curColor--;
                    if (curColor < 0)
                    {
                        curColor = 5;
                    }
                } while (selectedColors.Contains(curColor));
              
            }
            else
            {
                do
                {
                    curColor++;
                    if (curColor >= maxColors)
                    {
                        curColor = 0;
                    }
                } while (selectedColors.Contains(curColor));
              
            }
        }

        playerPanels[player].currentColor = curColor;
        playerPanels[player].myPointer.rectTransform.localPosition = new Vector3(
            125 + (curColor * 25),
            playerPanels[player].myPointer.rectTransform.localPosition.y, 
            0);

        switch (curColor)
        {
            case 0:
                playerPanels[player].myIcon.color = new Color(1.0f, 1.0f, 1.0f);
                break;
            case 1:
                playerPanels[player].myIcon.color = new Color(1.0f, 0.9686f, 0.0f);
                break;
            case 2:
                playerPanels[player].myIcon.color = new Color(0.1137f, 1.0f, 0.0f);
                break;
            case 3:
                playerPanels[player].myIcon.color = new Color(0.0353f, 1.0f, 0.8392f);
                break;
            case 4:
                playerPanels[player].myIcon.color = new Color(0.0f, 0.0f, 1.0f);
                break;
            case 5:
                playerPanels[player].myIcon.color = new Color(.4863f, 0.0f, 0.698f);
                break; 
        }

    }

    bool isReady()
    {
        int num = 0;
        foreach(PlayerPanel p in playerPanels)
        {
            if (p.Ready)
                num++;
        }

        if (currentConnectedControllers == num)
        {
           
            return true;
        }
        else
        {
            
            return false;
        }
    }

    bool checkReady()
    {
        foreach(PlayerPanel p in playerPanels)
        {
            if(p.Connected && !p.Ready)
            {
                middleText.GetComponent<Text>().text = "Waiting for players...";
                return false;
            }
        }
        middleText.GetComponent<Text>().text = "Press Start to Begin";
        return true;
    }
    
}
