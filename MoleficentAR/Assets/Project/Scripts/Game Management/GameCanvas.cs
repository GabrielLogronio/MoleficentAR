using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    static GameCanvas instance = null;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this);

    }

    public static GameCanvas getInstance()
    {
        return instance;
    }
}
