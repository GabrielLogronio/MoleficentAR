using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGuide : MonoBehaviour
{
    [SerializeField]
    GameObject NextSphere;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == gameObject.layer && other.gameObject.tag != "PhysicalPlayer")
        {
            SoundsManager.getInstance().PlayMenuSelectSound();
            NextSphere.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
