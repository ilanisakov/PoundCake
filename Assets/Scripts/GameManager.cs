using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    [SerializeField]
    private GameObject playerPrefab1;
    [SerializeField]
    private GameObject playerPrefab2;
    [SerializeField]
    private GameObject playerPrefab3;
    [SerializeField]
    private GameObject playerPrefab4;

    enum GameState
    {
        InGame,
        GameOver
    };

    private int maxPlayers = 4;
    private int numPlayers;
    private int currentConnectedControllers;
    private GameState gameState;
    private GameObject[] players;
    private GameObject[] playerPrefabs;
    private GameObject[] playerPanels;
    private Vector3[] startingPositions;

    //Other Scripts
    mainCamera camera;

    public GameObject gameOverText;

    // Use this for initialization
    void Start()
    {

        numPlayers = PlayerPrefs.GetInt("numberPlayers");

        playerPrefabs = new GameObject[] { playerPrefab1, playerPrefab2, playerPrefab3, playerPrefab4 };

        currentConnectedControllers = Input.GetJoystickNames().Length;

        players = new GameObject[numPlayers];
        playerPanels = new GameObject[numPlayers];

        startingPositions = new Vector3[]
        {
            new Vector3(-5.5f,2.5f,0),
            new Vector3(5,1,0),
            new Vector3(.3f,6.75f,0),
            new Vector3(-3.55f,10.25f,0)
        };

        //Setting up Players
        for (int i = 0; i < numPlayers; i++)
        {
            string HUDName = "HUD_Player_" + (i +1);

            players[i] = (GameObject)Instantiate(playerPrefabs[i], startingPositions[i], Quaternion.identity);

            Character c = players[i].GetComponent<Character>();
            c.HUD = GameObject.Find(HUDName);
            c.cakeDelayScale = 0.20f;
            c.cakeSpeedScale = 1.15f;
            c.hasteSpeed = 6.0f;
            c.hasteSpeedDuration = 3.0f;
            c.healthRestore = 20;
            c.moveSpeedScale = 0.75f;
            c.rapidFireSpeed = 0.15f;
            c.rapidFireDuration = 2.5f;

            

            Controller controller = players[i].GetComponent<Controller>();
            controller.cakeSpeed = 5.5f;
            controller.cakeValue = 2;
            controller.dashPower = 1.0f;
            controller.hideDelay = 3.0f;
            controller.jumpPower = 4.0f; //does this do anything anymore?
            controller.maxSpeed = 4.0f;
            controller.shotDelay = 0.25f;
                      
            c.SetUp();

            playerPanels[i] = c.HUD;
        }

        camera = GameObject.Find("Camera").GetComponent<mainCamera>();

        camera.FindPlayers();

        createHUD();

        gameState = GameState.InGame;
        

        //Instantiate(playerPrefab, new Vector3(2, 2, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        switch(gameState)
        {
            case GameState.InGame:
                checkDead();
                break;
            case GameState.GameOver:
                gameOverText.GetComponent<Text>().text = "Game Over";
                break;
        }
    }

    void createHUD()
    {
        for(int i = numPlayers; i < maxPlayers; i++)
        {
            string HUDName = "HUD_Player_" + (i + 1);
            GameObject.Find(HUDName).SetActive(false);
        }

        for(int j = 0; j < playerPanels.Length; j++)
        {
            Color c = players[j].GetComponent<Character>().getColor();
            if (c != Color.white)
                playerPanels[j].GetComponent<HUDScript>().ChangeColor(c);
            else
                playerPanels[j].GetComponent<HUDScript>().ChangeColor(new Color(.945f, .412f, 1.0f));
        }

    }

    private void checkDead()
    {
        int num = numPlayers;
        foreach(GameObject p in players)
        {
            Character c = p.GetComponent<Character>();
            if(c.isDead)
            {
                num--;
            }
        }
        if(num <= 1)
        {
            gameState = GameState.GameOver;
        }
    }
}
