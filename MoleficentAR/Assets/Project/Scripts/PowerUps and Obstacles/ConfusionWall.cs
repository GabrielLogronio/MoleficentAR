using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfusionWall : BasePowerUp
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer >= 8 && other.gameObject.layer <= 11 && other.gameObject.tag == "PhysicalPlayer")
        {
            if (other.gameObject.GetComponent<PhysicalPlayer>())
                other.gameObject.GetComponent<PhysicalPlayer>().ConfusionWallEffect();
        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * 5f * Time.deltaTime);

    }
}