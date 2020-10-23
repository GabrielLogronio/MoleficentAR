using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : FixedPlatform
{
    float MovingSpeed = 3f, StayTime = 1.5f;

    bool Moving = false;

    int CurrentPosition = 0;

    [SerializeField]
    Vector3[] Positions;

    // Start is called before the first frame update
    override protected void Start()
    {
        //transform.position = Positions[0];
        //ChangeTargetPosition();
    }
    public override void Initiate()
    {
        transform.position = Positions[0];
        ChangeTargetPosition();
    }

    // Update is called once per frame
    override protected void Update()
    {
        if (Moving)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, Positions[CurrentPosition], MovingSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.localPosition, Positions[CurrentPosition]) < 0.1f)
            {
                Moving = false;
                Invoke("ChangeTargetPosition", StayTime);

            }
        }
    }

    void ChangeTargetPosition()
    {
        Moving = true;
        CurrentPosition++;
        if (CurrentPosition >= Positions.Length) CurrentPosition = 0;
    }
}
