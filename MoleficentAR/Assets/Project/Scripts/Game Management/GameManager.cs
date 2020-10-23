using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance = null;

    bool InGame = false, InPractice = false;

    [SerializeField]
    bool[] PlayersReadyStatus = new bool[] { false, false, false, false };

    [SerializeField]
    int PlayersReady = 0, MaxPlayers = 2, SelectedPlayer = -1;

    public static GameManager getInstance()
    {
        return instance;
    }

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        DontDestroyOnLoad(gameObject);

    }

    public void SetGameStatus(bool ToSet)
    {
        if (ToSet)
        {
            Invoke("StartGame", 3f);
        }
        else
        {
            InGame = false;
        }

    }

    void StartGame()
    {
        SoundsManager.getInstance().PlayIngameMusic();

        PhysicalGameManager.getInstance().gameObject.transform.GetChild(0).gameObject.SetActive(false);

        if (NetworkManager.getInstance().IsServer())
        {
            ObstaclesManager.getInstance().StartObstaclesManager();
            PowerUpsManager.getInstance().StartPowerUpsManager();

        }

        for (int i = 0; i < 4; i++)
        {
            if (i == SelectedPlayer)
                transform.GetChild(3).GetChild(i).GetComponent<BaseController>().SelectPlayer(true);
            else
            {
                if (!InPractice)
                {
                    transform.GetChild(3).GetChild(i).GetComponent<NPCController>().enabled = true;
                    transform.GetChild(3).GetChild(i).GetComponent<NPCController>().StartSynchronize();
                }
                transform.GetChild(3).GetChild(i).GetComponent<CapsuleCollider>().enabled = false;
                Destroy(transform.GetChild(3).GetChild(i).GetComponent<Rigidbody>());
                
            }

        }

        var Platforms = gameObject.GetComponentsInChildren<FixedPlatform>();

        // Enable rigidbodies:
        foreach (var component in Platforms)
            component.Initiate();

        UIPlayerFollow.getInstance().SetPlayer(SelectedPlayer);

        Timer.getInstance().StartTimer();

        GameCanvas.getInstance().gameObject.transform.GetChild(0).GetChild(0).GetChild(0).GetChild(SelectedPlayer).gameObject.SetActive(true);
        GameCanvas.getInstance().gameObject.transform.GetChild(0).GetChild(1).GetChild(SelectedPlayer).gameObject.SetActive(true);
        GameCanvas.getInstance().gameObject.transform.GetChild(0).GetChild(0).GetChild(1).gameObject.SetActive(true);

        InGame = true;
    }

    public void SetPlayerReady(int PlayerNumber)
    {
        if (PlayersReadyStatus[PlayerNumber] == false) 
        {
            PlayersReadyStatus[PlayerNumber] = true;
            PlayersReady++;
            PhysicalGameManager.getInstance().transform.GetChild(0).GetChild(PlayerNumber).GetChild(0).gameObject.SetActive(true);
            if (PlayersReady == MaxPlayers) SetGameStatus(true);

        }
    }

    public void ActivatePlayer(int PlayerNumber)
    {
        SelectedPlayer = PlayerNumber;
        PhysicalGameManager.getInstance().transform.GetChild(1).gameObject.layer = 8 + PlayerNumber;
        PhysicalGameManager.getInstance().transform.GetChild(1).gameObject.GetComponent<PhysicalPlayer>().SetPlayer(transform.GetChild(3).GetChild(PlayerNumber).gameObject);

    }

    public void SetPlayerNumber(int ToSet, bool Practice)
    {
        MaxPlayers = ToSet;
        InPractice = Practice;
    }

    public int GetPlayerNumber()
    {
        return SelectedPlayer;
    }

}
