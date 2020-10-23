using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BaseController : MonoBehaviour
{
    protected enum PlayerState { Idle, Run, Fall, SpecialJump };
	protected PlayerState CurrentState = PlayerState.Idle;
	protected enum SpecialState { None, OnFire, Frozen, Confused, Bounce };
	protected SpecialState CurrentSpecialState = SpecialState.None;

	[SerializeField]
    protected Camera cam;

    [SerializeField]
    protected LayerMask GroundLayer;

    [SerializeField]
    protected Joystick PlayerJoystick;

    //--------------------------------------------------- INTERNAL COMPONENTS ---------------------------------------------------//
    protected Rigidbody rb;
    protected CapsuleCollider coll;
    protected Animator anim;

    //--------------------------------------------------- INTERNAL PARAMETERS ---------------------------------------------------//
    protected bool IsGrounded, Moving, SpecialJumped, ActivePlayer = false;
    protected bool InvertedX = false, InvertedY = false;
    protected Vector3 dir = Vector3.zero;
    protected Vector3 StartingPosition;

    //--------------------------------------------------- CONTROL PARAMETERS ---------------------------------------------------//
    protected float MovementSpeed = 4.5f;
    protected float RotationSpeed = 0.25f;
    protected float JumpForce = 550f;
    protected float JumpSlow = 1.15f;
    public int CurrentSpawnPointID = -1;
    public Vector3 CurrentSpawnPosition = Vector3.zero;
    protected float StatusDuration = 10f;

    protected void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        anim = GetComponent<Animator>();

        StartingPosition = transform.localPosition;
    }

    protected virtual void Update()
    {
        UpdateGroundedStatus();
        UpdateDirection();

        if (IsGrounded)
        {
			// The player just landed
            if (CurrentState == PlayerState.Fall)
            {
                SpecialJumped = false;
                anim.SetBool("SpecialJump", false);
                JumpSlow = 1.25f;
            }

			// The player has input movement
			if (Moving)
            {
                CurrentState = PlayerState.Run;
                anim.SetBool("Moving", true);

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), RotationSpeed);
                transform.position += dir * MovementSpeed * Time.deltaTime;
            }
            else
            {
				// THe player has NO input movement

                if (CurrentSpecialState == SpecialState.OnFire)
                {
                    CurrentState = PlayerState.Run;
                    anim.SetBool("Moving", true);

                    transform.position += transform.forward * MovementSpeed * Time.deltaTime;

                }
                else
                {
                    CurrentState = PlayerState.Idle;

                    anim.SetBool("Moving", false);
                }

            }

			if (CurrentSpecialState == SpecialState.Bounce)
			{
                rb.velocity = Vector3.zero;
				rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
				transform.parent = GameManager.getInstance().transform.GetChild(3);
			}

		}

        if (transform.position.y < -1f) transform.localPosition = StartingPosition;
    }

    //--------------------------------------------------- TOOL FUNCTIONS ---------------------------------------------------//

    protected void UpdateGroundedStatus()
    {
        IsGrounded = Physics.CheckCapsule(coll.bounds.center, new Vector3(coll.bounds.center.x, coll.bounds.min.y, coll.bounds.center.z), coll.radius * 0.9f, GroundLayer);
        anim.SetBool("IsGrounded", IsGrounded);
    }

    protected void UpdateDirection()
    {
        float InputX, InputZ;

        if (CurrentSpecialState == SpecialState.Confused)
        {
            InputX = PlayerJoystick.Horizontal;
            InputZ = -PlayerJoystick.Vertical;
        }
        else
        {
            if(InvertedX) InputX = -PlayerJoystick.Horizontal;
            else InputX = PlayerJoystick.Horizontal;
            if (InvertedY) InputZ = -PlayerJoystick.Vertical;
            else InputZ = PlayerJoystick.Vertical;

        }


        Moving = Mathf.Sqrt(InputX * InputX + InputZ * InputZ) > 0;

        Vector3 CamForward = cam.transform.forward,
                CamRight = cam.transform.right;

        CamForward.y = CamRight.y = 0;

        dir = (CamForward * InputZ + CamRight * InputX).normalized;
    }

    /*protected void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 12 && IsGrounded)
        {
            transform.parent = collision.gameObject.transform;

        }
    }*/

    protected void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == 12)// && collision.gameObject == transform.parent.gameObject)
        {
            transform.parent = GameManager.getInstance().transform.GetChild(3);

        }
    }
    protected void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.layer == 12 && IsGrounded)
        {
            transform.parent = collision.gameObject.transform;

        }
    }

    public bool SetRespawnPoint(int NewSpawnPointID, Vector3 NewSpawnPosition) 
    {
        if (NewSpawnPointID > CurrentSpawnPointID)
        {
            CurrentSpawnPointID = NewSpawnPointID;
            CurrentSpawnPosition = NewSpawnPosition;
            return true;
        }
        else return false;
    }

    protected abstract void SynchronizePosition();

    public abstract int GetPlayerNumber();

    //--------------------------------------------------- INPUT FUNCTIONS ---------------------------------------------------//

    public virtual void JumpPressed()
    {
        if (IsGrounded)
        {
            rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
            transform.parent = GameManager.getInstance().transform.GetChild(3);
        }
    }

    public void RespawnPressed() 
    {
        transform.parent = GameManager.getInstance().transform.GetChild(3);

        if (CurrentSpawnPointID > -1)
            transform.localPosition = CurrentSpawnPosition;
    }

    public void SelectPlayer(bool ToSet)
    {
        ActivePlayer = ToSet;
    }

    public void SwitchX() 
    {
        InvertedX = !InvertedX;
    }
    public void SwitchY()
    {
        InvertedY = !InvertedY;
    }

    //--------------------------------------------------- OBSTACLES FUNCTIONS ---------------------------------------------------//

    public void SetOnFire()
    {
        if (CurrentSpecialState == SpecialState.None) 
        {
            SoundsManager.getInstance().PlayOnFireEffect();
            CurrentSpecialState = SpecialState.OnFire;
            transform.GetChild(2).gameObject.SetActive(true);

            Invoke("CureOnFire", StatusDuration);
        }
    }

    void CureOnFire()
    {
		CurrentSpecialState = SpecialState.None;
		transform.GetChild(2).gameObject.SetActive(false);

    }

    public void SetFrozen()
    {
        if (CurrentSpecialState == SpecialState.None)
        {
            SoundsManager.getInstance().PlayFrozenEffect();
            CurrentSpecialState = SpecialState.Frozen;
            transform.GetChild(3).gameObject.SetActive(true);

            Invoke("CureFrozen", StatusDuration);
        }
    }

    void CureFrozen()
    {
		CurrentSpecialState = SpecialState.None;
		transform.GetChild(3).gameObject.SetActive(false);

    }

    public void SetConfused()
    {
        if (CurrentSpecialState == SpecialState.None)
        {
            SoundsManager.getInstance().PlayConfusedEffect();
            CurrentSpecialState = SpecialState.Confused;
            transform.GetChild(4).gameObject.SetActive(true);

            Invoke("CureConfused", StatusDuration);
        }
    }

    void CureConfused()
    {
		CurrentSpecialState = SpecialState.None;
		transform.GetChild(4).gameObject.SetActive(false);

    }

	public void SetBounce()
	{
        if (CurrentSpecialState == SpecialState.None)
        {
            SoundsManager.getInstance().PlayBounceEffect();
            CurrentSpecialState = SpecialState.Bounce;
            transform.GetChild(5).gameObject.SetActive(true);

            Invoke("CureBounce", StatusDuration);
        }
	}

	void CureBounce()
	{
		CurrentSpecialState = SpecialState.None;
		transform.GetChild(5).gameObject.SetActive(false);

	}

}
