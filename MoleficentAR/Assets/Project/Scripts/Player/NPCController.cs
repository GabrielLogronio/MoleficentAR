using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField]
    RuntimeAnimatorController NPCAnim;

    [SerializeField]
    protected LayerMask GroundLayer;

    Animator anim;
    CapsuleCollider coll;

    Vector3 CurrentPlayerPosition, OldPlayerPosition;

    float MovementSpeed = 4.5f, RotationSpeed = 0.25f, StatusDuration = 10f;


    bool setNPC = false;

    private void Start()
    {
        CurrentPlayerPosition = transform.position;
        OldPlayerPosition = transform.position;

    }

    private void Update()
    {
        if (setNPC)
        {
            bool Grounded = Physics.CheckCapsule(coll.bounds.center, new Vector3(coll.bounds.center.x, coll.bounds.min.y, coll.bounds.center.z), coll.radius * 0.9f, GroundLayer);
            anim.SetBool("IsGrounded", Grounded);

            Vector3 TargetPosition = CurrentPlayerPosition + (CurrentPlayerPosition - OldPlayerPosition);
            Vector3 TargetRotation = (CurrentPlayerPosition - transform.position);
            TargetRotation.y = 0f;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(TargetRotation), RotationSpeed);

            if (Vector3.Distance(transform.position, CurrentPlayerPosition) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, TargetPosition, MovementSpeed * Time.deltaTime);
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(CurrentPlayerPosition.x - transform.position.x, transform.position.y, CurrentPlayerPosition.z - transform.position.z).normalized), RotationSpeed);
                anim.SetBool("Moving", true);
            }
            else anim.SetBool("Moving", false);


        }
    }

    public void StartSynchronize()
    {
        anim = GetComponent<Animator>();
        anim.runtimeAnimatorController = NPCAnim as RuntimeAnimatorController;

        coll = GetComponent<CapsuleCollider>();
        GetComponent<BaseController>().enabled = false;

        setNPC = true;

    }

    public void UpdatePosition(float X, float Y, float Z)
    {
        OldPlayerPosition = CurrentPlayerPosition;
        CurrentPlayerPosition = new Vector3(X, Y, Z);

    }

    public void SetOnFire()
    {
            SoundsManager.getInstance().PlayOnFireEffect();
            transform.GetChild(2).gameObject.SetActive(true);

            Invoke("CureOnFire", StatusDuration);
    
    }

    void CureOnFire()
    {
        transform.GetChild(2).gameObject.SetActive(false);

    }

    public void SetFrozen()
    {
        SoundsManager.getInstance().PlayFrozenEffect();
        transform.GetChild(3).gameObject.SetActive(true);

        Invoke("CureFrozen", StatusDuration);
    }

    void CureFrozen()
    {
        transform.GetChild(3).gameObject.SetActive(false);

    }

    public void SetConfused()
    {
        SoundsManager.getInstance().PlayConfusedEffect();
        transform.GetChild(4).gameObject.SetActive(true);

        Invoke("CureConfused", StatusDuration);
    }

    void CureConfused()
    {
        transform.GetChild(4).gameObject.SetActive(false);

    }

    public void SetBounce()
    {
        SoundsManager.getInstance().PlayBounceEffect();
        transform.GetChild(5).gameObject.SetActive(true);

        Invoke("CureBounce", StatusDuration);
    }

    void CureBounce()
    {
        transform.GetChild(5).gameObject.SetActive(false);

    }
}
