using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float Speed;
    public GameObject Pickup;
    public GameObject SnakeBody;
    public Text ScoreText;
    
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
    //The two values under are to prevent button spam
    private bool justTurned;
    private float keyDelay;
    private GameObject lastBody;
    private int score;

    void Start()
    {
        lastBody = SnakeBody;
        snakeHead = GetComponent<Rigidbody>();
        snakeHead.freezeRotation = true;
        lastDirec = 0;
        score = 2;
        SetCounterText();

        //Values of moveVert and moveHoriz and rotation when turning to a new direction
        //From north, east, south, west
        direcValues = new int[4, 3] {{1, 0, 45}, {0, 1, 135}, {-1, 0, 225}, {0, -1, 315}};
        Turn(lastDirec);
        justTurned = false;
    }

    void Update()
    {
        ResetTimer();
        if (!justTurned)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                lastDirec--;
                Turn(lastDirec);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                lastDirec++;
                Turn(lastDirec);
            }
        }
    }

    //Countdown of key press delay
    private void ResetTimer()
    {
        if (keyDelay > 0)
        {
            keyDelay -= Time.deltaTime;
        }
        else
        {
            keyDelay = 0;
            justTurned = false;
        }
    }

    private void Turn(int lastDirec)
    {
        keyDelay = 0.26f; //0.26 seconds of delay
        justTurned = true;

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
        snakeHead.velocity = movement * Speed;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            other.gameObject.SetActive(false);
            SpawnBody();
            SpawnPickup();

            score++;
            SetCounterText();
        }
    }

    private void SpawnBody()
    {
        //Spawn new body part
        //Spawn distances also from north east south west
        float[,] spawnDistances = new float[4, 2] {{0f, -1.3f}, {-1.3f, 0f}, {0f, 1.3f}, {1.3f, 0f}};
        int lastDirection = lastBody.GetComponent<BodyController>().LastDirec;

        float x = lastBody.transform.position.x + spawnDistances[lastDirection, 0];
        float z = lastBody.transform.position.z + spawnDistances[lastDirection, 1];
        //Y value will always stay as 0.4
        Vector3 spawnPos = new Vector3(x, 0.4f, z);
        GameObject newBody = (GameObject)Instantiate(SnakeBody, spawnPos, Quaternion.identity);
        newBody.SetActive(true);
        newBody.transform.Rotate(new Vector3(0, 45, 0));
        newBody.GetComponent<BodyController>().LastDirec = lastDirection;
        lastBody = newBody;
    }

    private void SpawnPickup()
    {
        //Spawn pickup
        //Check for other objects in range of spawning position
        Vector3 spawnPos;
        do
        {
            spawnPos = GenPosition();
        } while (Physics.CheckSphere(spawnPos, 1f));

        GameObject newPickup = (GameObject)Instantiate(Pickup, spawnPos, Quaternion.identity);
        newPickup.SetActive(true);
    }

    private Vector3 GenPosition()
    {
        int x = Random.Range(-9, 10);
        float y = 0.5f;
        int z = Random.Range(-9, 10);
        return new Vector3(x, y, z);
    }

    private void SetCounterText()
    {
        ScoreText.text = "Score: " + score;
    }
}