using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalPlayer : MonoBehaviour
{
    [SerializeField]
    GameObject Player;

    public void IceWallEffect()
    {
        Player.GetComponent<BaseController>().SetFrozen();

    }

    public void FireWallEffect()
    {
        Player.GetComponent<BaseController>().SetOnFire();

    }

    public void ConfusionWallEffect()
    {
        Player.GetComponent<BaseController>().SetConfused();

    }

	public void BounceWallEffect()
	{
		Player.GetComponent<BaseController>().SetBounce();

	}

	public void SetPlayer(GameObject ToSet)
    {
        Player = ToSet;
    }

}
