using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField]
    int ID;

    void OnTriggerEnter(Collider other) 
    {
        if (other.gameObject.layer == gameObject.layer && other.gameObject.tag != "PhysicalPlayer") 
        {
            if (other.gameObject.GetComponent<BaseController>().SetRespawnPoint(ID, transform.localPosition))
                gameObject.transform.GetChild(2).gameObject.SetActive(true);

        }
    }

}
