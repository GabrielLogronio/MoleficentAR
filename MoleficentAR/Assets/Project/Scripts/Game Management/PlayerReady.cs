using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReady : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == gameObject.layer && other.gameObject.tag == "PhysicalPlayer")
        {
            NetworkManager.getInstance().StringMessageToAll("RD|" + (gameObject.layer - 8));
            GetComponent<BoxCollider>().enabled = false;
            Destroy(GetComponent<Rigidbody>());            
            //GameManager.getInstance().SetPlayerReady(gameObject.layer - 8);          

        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == gameObject.layer && other.gameObject.tag == "PhysicalPlayer")
        {
            NetworkManager.getInstance().StringMessageToAll("RD|" + (gameObject.layer - 8));
            GetComponent<BoxCollider>().enabled = false;
            Destroy(GetComponent<Rigidbody>());              
            //GameManager.getInstance().SetPlayerReady(gameObject.layer - 8);

        }
    }

}
