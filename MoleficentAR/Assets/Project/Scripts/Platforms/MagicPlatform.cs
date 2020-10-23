using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicPlatform : FixedPlatform
{
    [SerializeField]
    bool CurrentlyActive;

    float SwitchTimer = 5f;

    // Start is called before the first frame update
    override protected void Start()
    {
        //Switch();
    }

    public override void Initiate()
    {
        Switch();
    }

    void Switch()
    {
        CurrentlyActive = !CurrentlyActive;

        Invoke("Switch", SwitchTimer);

        if (CurrentlyActive)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);

            foreach (Transform child in transform)
            {
                if (child.GetSiblingIndex() > 1) child.parent = GameManager.getInstance().transform.GetChild(3);
            }

        }

    }

}
