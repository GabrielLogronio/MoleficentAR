using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTester : MonoBehaviour
{
    public int SelectedPlayer;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("StartGame", 3f);

    }

    void StartGame()
    {
        if (SelectedPlayer >= 0 && SelectedPlayer < 4)
        {
            GameManager.getInstance().ActivatePlayer(SelectedPlayer);

        }
    }

}
