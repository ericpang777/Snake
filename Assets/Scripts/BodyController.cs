using System;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    public float Speed;
    public GameObject SnakeHead;

    private Rigidbody snakeBody;
    private PlayerController playControl;
    private float moveVert; 
    private float moveHoriz;
    //Values for vertical and horizontal and rotation for a direction
    private int[,] direcValues;
    //Last direction snake was going
    private int lastDirec;
    public Vector3 currentPos;
    public Vector3 lastTurnPos;

    void Start () {
        snakeBody = GetComponent<Rigidbody>();
        snakeBody.freezeRotation = true;
        playControl = SnakeHead.GetComponent<PlayerController>();
        lastDirec = playControl.LastDirec;

        //Values of moveVert and moveHoriz and rotation when turning to a new direction
        //From north, east, south, west
        direcValues = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
        TurnTo(lastDirec);
    }
	
	void Update ()
	{
	    lastTurnPos = playControl.LastTurnPos;
	    lastTurnPos = Round(lastTurnPos);
	    currentPos = snakeBody.transform.position;
	    currentPos = Round(currentPos);
	    if(currentPos.Equals(lastTurnPos))
        {
            lastDirec = playControl.LastDirec;
            TurnTo(lastDirec);
        }
	}

    Vector3 Round(Vector3 v3)
    {
        //Rounds to nearest fifth
        float x = Mathf.Round(v3.x * 10f) / 10;
        float y = Mathf.Round(v3.y * 10f) / 10;
        float z = Mathf.Round(v3.z * 10f) / 10;
        return new Vector3(x, y, z);
    }

    void TurnTo(int lastDirec)
    {
        this.lastDirec = lastDirec + 4;
        this.lastDirec %= 4;
        moveVert = direcValues[this.lastDirec, 0];
        moveHoriz = direcValues[this.lastDirec, 1];
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveHoriz, 0.0f, moveVert);
        snakeBody.velocity = movement * Speed;
    }
}
