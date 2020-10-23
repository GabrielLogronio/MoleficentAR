using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalGameManager : MonoBehaviour
{
    static PhysicalGameManager instance = null;

    public static PhysicalGameManager getInstance()
    {
        return instance;
    }

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        DontDestroyOnLoad(gameObject);

    }

}
