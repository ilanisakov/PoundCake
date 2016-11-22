using UnityEngine;
using System.Collections;

public class mainCamera : MonoBehaviour {

    Camera mainC;
    public int numPlayers;

    private Vector3 position;
    private float size;
    private float minSize = 4.5f;
    private float maxSize = 8.0f;
    private float minX, maxX, minY, maxY, midX, midY, disX, disY;
    private float offset = 1.0f;
    private float depth;
    private Vector3[] playerPositions;

    // Use this for initialization
    void Start()
    {
        mainC = this.GetComponent<Camera>();

        position = transform.position;
        size = this.GetComponent<Camera>().orthographicSize;
        depth = position.z;

        playerPositions = new Vector3[numPlayers];

        UpdatePositions();

        minX = playerPositions[0].x;
        maxX = playerPositions[0].x;
        minY = playerPositions[0].y;
        maxY = playerPositions[0].y;

    }

    // Update is called once per frame
    void Update()
    {

        UpdatePositions();

        //find min and max values
        foreach (Vector3 pos in playerPositions)
        {
            if (pos.x >= maxX) { maxX = pos.x; }
            else { minX = pos.x; }

            if (pos.y >= maxY) { maxY = pos.y; }
            else { minY = pos.y; }

        }

        //get center of the mins & maxs
        disX = (maxX - minX);
        disY = (maxY - minY);
        midX = minX + ((maxX - minX) / 2.0f);
        midY = minY + ((maxY - minY) / 2.0f);

        size = (disX > disY) ? disX + 2 * offset : disY + 2 * offset;

        if(size < minSize) { size = minSize; }
        else if(size > maxSize) { size = maxSize; }

        mainC.orthographicSize = size;
        

        Vector3 Midpoint = new Vector3(midX, midY, depth);

        minX = playerPositions[0].x;
        maxX = playerPositions[0].x;
        minY = playerPositions[0].y;
        maxY = playerPositions[0].y;

        this.transform.position = Midpoint;

    }

    void UpdatePositions()
    {
        for (int i = 0; i < numPlayers; i++)
        {
            string name = "Player_" + (i + 1);
            playerPositions[i] = GameObject.Find(name).transform.position;
            //Debug.Log(name + ":" + playerPositions[i]);
        }
    }
}
