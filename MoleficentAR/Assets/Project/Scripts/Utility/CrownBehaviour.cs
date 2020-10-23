using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrownBehaviour : MonoBehaviour
{
    [SerializeField]
    float MinX, MaxX, MinZ, MaxZ, PauseTime, MovementSpeed, RotationSpeed;

    Vector3 Destination;

    void Start()
    {
        //CalculateNewDestination();

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * RotationSpeed);

        //if (Vector3.Distance(transform.position, Destination) < 0.1f) Invoke("CalculateNewDestination", PauseTime);
        //else transform.localPosition = Vector3.MoveTowards(transform.localPosition, Destination, Time.deltaTime * MovementSpeed);

    }

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.layer >= 8 && other.gameObject.layer <= 11 && other.gameObject.tag != "PhysicalPlayer") 
        {
            SoundsManager.getInstance().StopAllSounds();
            SoundsManager.getInstance().PlayWinner();
            NetworkManager.getInstance().StringMessageToAll("WIN|" + (other.gameObject.layer - 8));

        }
    }

    void CalculateNewDestination()
    {
        Destination = new Vector3(Random.Range(MinX, MaxX), transform.position.y, Random.Range(MinZ, MaxZ));
    }
}
