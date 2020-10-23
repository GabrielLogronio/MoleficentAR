using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SirLoinController : BaseController
{
    float FallingSpeed = -0.4f;
    bool Gliding = false;

    public override void JumpPressed()
    {
		if (ActivePlayer && CurrentSpecialState != SpecialState.Frozen)
		{
			base.JumpPressed();

            if (!IsGrounded)
            {
                Gliding = true;
                anim.SetBool("Gliding", true);

            }
        }
    }

    public void JumpReleased()
    {
		if (ActivePlayer && CurrentSpecialState != SpecialState.Frozen)
		{
			Gliding = false;
            anim.SetBool("Gliding", false);

        }
    
    }

    protected override void Update()
    {
		if (ActivePlayer && CurrentSpecialState != SpecialState.Frozen)
		{
			base.Update();

            if (!IsGrounded)
            {
                CurrentState = PlayerState.Fall;

                if (dir != Vector3.zero)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), RotationSpeed);
                    transform.position += dir * MovementSpeed / JumpSlow * Time.deltaTime;

                }
            }

            if (IsGrounded)
            {
                Gliding = false;
                anim.SetBool("Gliding", false);

            }

            if (Gliding) rb.velocity = new Vector3(0, FallingSpeed, 0);

            SynchronizePosition();

        }
    }

    protected override void SynchronizePosition()
    {
        string ToSend = "SLP|" + Math.Round(transform.position.x,2) + "|" + Math.Round(transform.position.y,2) + "|" + Math.Round(transform.position.z,2);
        NetworkManager.getInstance().StringMessageToAll(ToSend);
    }

    public override int GetPlayerNumber()
    {
        return 2;
    }
}
