using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField]
    GameObject Camera;

    public float Correction = -10f;

    // Update is called once per frame
    void Update()
    {
        Vector3 Displacement = Camera.transform.forward * Correction;
        Displacement.y = Correction;

        transform.position = Camera.transform.position + Displacement;
    }
}
