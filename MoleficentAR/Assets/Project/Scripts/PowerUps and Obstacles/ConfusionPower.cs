using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfusionPower : BasePowerUp
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer >= 8 && other.gameObject.layer <= 11 && other.gameObject.tag == "PhysicalPlayer")
        {
            NetworkManager.getInstance().StringMessageToAll("CP|" + (other.gameObject.layer - 8));
            SoundsManager.getInstance().PlayConfusedEffect();
            // GameCanvas.getInstance().transform.GetChild(3).GetChild(5).gameObject.SetActive(false);
            // gameObject.SetActive(false);

        }
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * 10f * Time.deltaTime);

    }
}
