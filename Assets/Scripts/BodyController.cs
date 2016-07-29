using UnityEngine;
using System.Collections;

public class BodyController : MonoBehaviour
{
    public float speed;
    public GameObject snakeHead;

    private Rigidbody snakeBody;
    private PlayerController playControl;
    private float moveVert; //Move vertical
    private float moveHoriz; //Move horizontal
    private int[,] direcValues; //Values for vertical and horizontal and rotation for a direction
    private int lastDirec; //Last direction snake was going

    void Start () {
        snakeBody = GetComponent<Rigidbody>();
        snakeBody.freezeRotation = true;
        playControl = snakeHead.GetComponent<PlayerController>();
        lastDirec = 0;

        //Values of moveVert and moveHoriz and rotation when turning to a new direction
        //From north, east, south, west
        direcValues = new int[4, 2] { { 1, 0 }, { 0, 1 }, { -1, 0 }, { 0, -1 } };
        turnTo(lastDirec);
    }
	
	void Update () {
	    if(snakeBody.transform.position.Equals(playControl.LastTurnPos))
        {
            lastDirec = playControl.LastDirec;
            turnTo(lastDirec);
        }
	}

    void turnTo(int lastDirec)
    {
        this.lastDirec = lastDirec + 4;
        this.lastDirec %= 4;
        moveVert = direcValues[this.lastDirec, 0];
        moveHoriz = direcValues[this.lastDirec, 1];
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveHoriz, 0.0f, moveVert);
        snakeBody.velocity = movement * speed;
    }
}
