using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float speed;
    public GameObject pickup;
    public GameObject snakeBody;
    public Text scoreText;

    private Rigidbody snakeHead;
    private float moveVert; 
    private float moveHoriz;
    //Values for vertical and horizontal and rotation for a direction
    private int[,] direcValues;
    //Last direction snake was going
    private int lastDirec; 
    public int LastDirec
    {
        get { return lastDirec; }
    }
    //Last turn position
    private Vector3 lastTurnPos; 
    public Vector3 LastTurnPos
    {
        get { return lastTurnPos; }
    }
    private int score;

    void Start()
    {
        snakeHead = GetComponent<Rigidbody>();
        snakeHead.freezeRotation = true;
        lastDirec = 0;
        score = 2;
        SetCounterText();

        //Values of moveVert and moveHoriz and rotation when turning to a new direction
        //From north, east, south, west
        direcValues = new int[4, 3] { { 1, 0, 45 }, { 0, 1, 135 }, { -1, 0, 225 }, { 0, -1, 315 } };
        turnTo(lastDirec);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            lastDirec--;
            turnTo(lastDirec);
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            lastDirec++;
            turnTo(lastDirec);
        }
    }

    void turnTo(int lastDirec)
    {
        lastTurnPos = snakeHead.transform.position;
        this.lastDirec = lastDirec + 4;
        this.lastDirec %= 4;
        moveVert = direcValues[this.lastDirec, 0];
        moveHoriz = direcValues[this.lastDirec, 1];
        snakeHead.MoveRotation(Quaternion.Euler(0f, direcValues[this.lastDirec, 2], 0f));
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(moveHoriz, 0.0f, moveVert);
        snakeHead.velocity = movement * speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            Vector3 spawnPos = GenPosition();
            GameObject newPickup = (GameObject)Instantiate(pickup, spawnPos, Quaternion.identity);
            newPickup.SetActive(true);

            spawnPos = new Vector3(0f, 0f, 0f);
            GameObject newBody = (GameObject)Instantiate(snakeBody, spawnPos, Quaternion.identity);
            newBody.SetActive(true);
            newBody.transform.Rotate(new Vector3(0, 45, 0));

            score++;
            SetCounterText();
        }
    }

    Vector3 GenPosition()
    {
        int x = Random.Range(-9, 10);
        float y = 0.5f;
        int z = Random.Range(-9, 10);
        return new Vector3(x, y, z);
    }

    void SetCounterText()
    {
        scoreText.text = "Score: " + score.ToString();
    }

    public int GetLastDirec()
    {
        return lastDirec;
    }
}