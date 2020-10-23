using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SirEalController : BaseController
{
    bool Attached = false;

    public override void JumpPressed()
    {
		if (ActivePlayer && CurrentSpecialState != SpecialState.Frozen)
		{
			base.JumpPressed();
            if (!IsGrounded && Attached)
            {
                rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                anim.SetBool("Attached", false);
                rb.useGravity = true;
                Attached = false;

            }
        }
    }

    new void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12 && IsGrounded)
        {
            transform.parent = collision.gameObject.transform;

        }

        if (ActivePlayer && CurrentSpecialState != SpecialState.Frozen)
		{
			if (collision.gameObject.layer == 13 && !IsGrounded && !SpecialJumped)
            {
                SpecialJumped = true;
                anim.SetBool("Attached", true);
                Attached = true;
                rb.useGravity = false;
                rb.velocity = Vector3.zero;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(-collision.contacts[0].normal), RotationSpeed * 10);

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

                if (dir != Vector3.zero && !Attached)
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
        string ToSend = "SEP|" + Math.Round(transform.position.x,2) + "|" + Math.Round(transform.position.y,2) + "|" + Math.Round(transform.position.z,2);
        NetworkManager.getInstance().StringMessageToAll(ToSend);
    }

    public override int GetPlayerNumber()
    {
        return 1;
    }

}
