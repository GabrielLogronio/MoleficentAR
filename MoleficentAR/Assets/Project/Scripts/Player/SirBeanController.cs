using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SirBeanController : BaseController
{
    public override void JumpPressed()
    {
        if (ActivePlayer && CurrentSpecialState != SpecialState.Frozen)
        {
            base.JumpPressed();

            if (!IsGrounded && !SpecialJumped)
            {
                anim.SetBool("SpecialJump", true);
                SpecialJumped = true;
                JumpSlow = 2.25f;
                rb.velocity = Vector3.zero;
                rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);

            }
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

            SynchronizePosition();

        }
    }

    protected override void SynchronizePosition()
    {
        string ToSend = "SBP|" + Math.Round(transform.position.x, 2) + "|" + Math.Round(transform.position.y, 2) + "|" + Math.Round(transform.position.z, 2);
        NetworkManager.getInstance().StringMessageToAll(ToSend);
    }

    public override int GetPlayerNumber()
    {
        return 0;
    }
}
