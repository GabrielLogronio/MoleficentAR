using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System;
using UnityEngine.SceneManagement;

public class NetworkClientManager : NetworkManager
{
    [SerializeField]
    InputField ServerIP;

    [SerializeField]
    Text IPAddressText;
    [SerializeField]
    NPCController[] NPCPlayers = new NPCController[4];

    int PlayerNumber = -1;

    NetworkClient client;

    public override void Activate()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        client = new NetworkClient();
        client.Connect(ServerIP.text, 25000);
        client.RegisterHandler(888, StringMessageReceiver);

    }

    public override void StringMessageToAll(string ToSend)
    {
        StringMessage msg = new StringMessage();
        msg.value = ToSend;
        client.Send(888, msg);
    }

    public override void StringMessageToPlayer(int i, string ToSend)
    {
        StringMessage msg = new StringMessage();
        msg.value = i + "|" + ToSend;
        client.Send(888, msg);
    }

    public override void StringMessageToClients(string ToSend)
    {
        Debug.Log("Client can't send messages to other clients");
    }

    protected override void ActivatePlayer()
    {
        GameManager.getInstance().ActivatePlayer(PlayerNumber);
        for (int i = 0; i < 4; i++) 
        {
            if (i != PlayerNumber) 
            {
                GameManager.getInstance().transform.GetChild(3).GetChild(i).gameObject.GetComponent<NPCController>().enabled = true;
                NPCPlayers[i] = GameManager.getInstance().transform.GetChild(3).GetChild(i).gameObject.GetComponent<NPCController>();

            }
        }
    }

    protected override void StringMessageReceiver(NetworkMessage NetMsg)
    {
        string msg = NetMsg.ReadMessage<StringMessage>().value;
        StringMessageReader(msg);

    }

    protected override void StringMessageReader(string msg)
    {
        string[] deltas = msg.Split('|');

        switch (deltas[0])
        {
            // ------------------------------------------------------ GAME STATUS UPDATE ------------------------------------------------------
            case "PN":
                int ToSetID;
                if (Int32.TryParse(deltas[1], out ToSetID))
                {
                    PlayerNumber = ToSetID;
                    ConnectionManager.getInstance().Connected();
                    IPAddressText.text = "Waiting for players...\n[IP: " + deltas[2] + "]";
                }
                break;
            case "CN":
                int PlayerNumb;
                if (Int32.TryParse(deltas[1], out PlayerNumb))
                {
                    ConnectionManager.getInstance().ConnectedPlayer(PlayerNumb);
                }
                break;
            case "ST":
                DontDestroyOnLoad(gameObject);
                SceneManager.LoadScene("Game Scene");
                SoundsManager.getInstance().StopAllSounds();
                Invoke("ActivatePlayer", 1.5f);
                break;
            case "RD":
                int PlayerReady;
                if (Int32.TryParse(deltas[1], out PlayerReady))
                {
                    GameManager.getInstance().SetPlayerReady(PlayerReady);
                }
                break;
            case "PC":
                PlatformManager.getInstance().SetConfiguration(deltas);
                break;
            case "WIN":
                int WinnerNumb;
                if (Int32.TryParse(deltas[1], out WinnerNumb))
                {
                    GameCanvas.getInstance().transform.GetChild(0).gameObject.SetActive(false);
                    GameCanvas.getInstance().transform.GetChild(1).gameObject.SetActive(false);
                    GameCanvas.getInstance().transform.GetChild(2).gameObject.SetActive(true);
                    GameCanvas.getInstance().transform.GetChild(2).GetChild(WinnerNumb + 1).gameObject.SetActive(true);

                    Time.timeScale = 0f;

                }
                break;

            // ------------------------------------------------------ PLAYERS POSITIONS UPDATE ------------------------------------------------------
            case "SBP":
                if(PlayerNumber != 0) NPCPlayers[0].UpdatePosition(float.Parse(deltas[1]), float.Parse(deltas[2]), float.Parse(deltas[3]));
                break;
            case "SEP":
                if (PlayerNumber != 1) NPCPlayers[1].UpdatePosition(float.Parse(deltas[1]), float.Parse(deltas[2]), float.Parse(deltas[3]));
                break;
            case "SLP":
                if (PlayerNumber != 2) NPCPlayers[2].UpdatePosition(float.Parse(deltas[1]), float.Parse(deltas[2]), float.Parse(deltas[3]));
                break;
            case "SSP":
                if (PlayerNumber != 3) NPCPlayers[3].UpdatePosition(float.Parse(deltas[1]), float.Parse(deltas[2]), float.Parse(deltas[3]));
                break;

            // ------------------------------------------------------ POWERS STATUS UPDATE ------------------------------------------------------
            case "PWR":
                int SelectedPower;
                if (Int32.TryParse(deltas[1], out SelectedPower))
                {
                    PowerUpsManager.getInstance().Warning(SelectedPower, float.Parse(deltas[2]), float.Parse(deltas[3]));
                }
                break;
            case "OBS":
                int SelectedObstacle;
                if (Int32.TryParse(deltas[1], out SelectedObstacle))
                {
                    ObstaclesManager.getInstance().Warning(SelectedObstacle);
                }
                break;

            // ------------------------------------------------------ POWERS EFFECTS ------------------------------------------------------
            case "FP":
                int FirePowerSafePlayer;
                if (Int32.TryParse(deltas[1], out FirePowerSafePlayer))
                {
                    PowerUpsManager.getInstance().transform.GetChild(1).gameObject.SetActive(false);
                    GameCanvas.getInstance().transform.GetChild(3).GetChild(5).gameObject.SetActive(false);

                    for (int i = 0; i < 4; i++) 
                    {
                        if (i != FirePowerSafePlayer)
                        {
                            if(i == PlayerNumber) GameManager.getInstance().transform.GetChild(3).GetChild(i).GetComponent<BaseController>().SetOnFire();
                            else GameManager.getInstance().transform.GetChild(3).GetChild(i).GetComponent<NPCController>().SetOnFire();
                        }

                    }

                }
                break;

            case "IP":
                int IcePowerSafePlayer;
                if (Int32.TryParse(deltas[1], out IcePowerSafePlayer))
                {
                    PowerUpsManager.getInstance().transform.GetChild(0).gameObject.SetActive(false);
                    GameCanvas.getInstance().transform.GetChild(3).GetChild(5).gameObject.SetActive(false);

                    for (int i = 0; i < 4; i++)
                    {
                        if (i != IcePowerSafePlayer)
                        {
                            if (i == PlayerNumber) GameManager.getInstance().transform.GetChild(3).GetChild(i).GetComponent<BaseController>().SetFrozen();
                            else GameManager.getInstance().transform.GetChild(3).GetChild(i).GetComponent<NPCController>().SetFrozen();
                        }

                    }
                }

                break;
            case "CP":
                int ConfusionPowerSafePlayer;
                if (Int32.TryParse(deltas[1], out ConfusionPowerSafePlayer))
                {
                    PowerUpsManager.getInstance().transform.GetChild(2).gameObject.SetActive(false);
                    GameCanvas.getInstance().transform.GetChild(3).GetChild(5).gameObject.SetActive(false);

                    for (int i = 0; i < 4; i++)
                    {
                        if (i != ConfusionPowerSafePlayer)
                        {
                            if (i == PlayerNumber) GameManager.getInstance().transform.GetChild(3).GetChild(i).GetComponent<BaseController>().SetConfused();
                            else GameManager.getInstance().transform.GetChild(3).GetChild(i).GetComponent<NPCController>().SetConfused();
                        }

                    }
                }
                break;

            case "BP":
                int BouncePowerSafePlayer;
                if (Int32.TryParse(deltas[1], out BouncePowerSafePlayer))
                {
                    PowerUpsManager.getInstance().transform.GetChild(3).gameObject.SetActive(false);
                    GameCanvas.getInstance().transform.GetChild(3).GetChild(5).gameObject.SetActive(false);

                    for (int i = 0; i < 4; i++)
                    {
                        if (i != BouncePowerSafePlayer)
                        {
                            if (i == PlayerNumber) GameManager.getInstance().transform.GetChild(3).GetChild(i).GetComponent<BaseController>().SetBounce();
                            else GameManager.getInstance().transform.GetChild(3).GetChild(i).GetComponent<NPCController>().SetBounce();
                        }

                    }
                }
                break;

            default:
                Debug.Log("Error - message not recognized: " + msg);
                break;
        }
    }

}
