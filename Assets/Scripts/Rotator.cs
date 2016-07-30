using UnityEngine;

public class Rotator : MonoBehaviour
{
    void Update()
    {
        //Rotates pickups
        transform.Rotate(new Vector3(30, 30, 30) * Time.deltaTime);
    }
}