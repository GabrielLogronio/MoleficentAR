using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public class NetworkServerManager : NetworkManager
{
    [SerializeField]
    Text IPAddressText;

    [SerializeField]
    int CurrentPlayers = 1;

    NPCController[] NPCPlayers = new NPCController[4];

    int PlayerNumber = 0, maxPlayers = 2;

    bool Practice = false;

    public override void  Activate()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        NetworkServer.Reset();
        NetworkServer.Listen(25000);

        NetworkServer.RegisterHandler(888, StringMessageReceiver);
        NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnected);

        IPAddressText.text = "Waiting for players...\n[IP: " + LocalIPAddress() + "]";

    }

    public void Tryout(int SelectedPlayerNumber)
    {
        if (instance == null) instance = this;
        else Destroy(this);

        PlayerNumber = SelectedPlayerNumber;
        Practice = true;

        NetworkServer.Reset();
        NetworkServer.Listen(25000);

        NetworkServer.RegisterHandler(888, StringMessageReceiver);
        NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnected);

        Invoke("StartGame", 0.5f);
    }

    void OnClientConnected(NetworkMessage NetMsg)
    {
        if (CurrentPlayers < 4)
        {
            CurrentPlayers++;
            ConnectionManager.getInstance().ConnectedPlayer(CurrentPlayers - 1);
            StringMessageToPlayer(NetMsg.conn.connectionId, "PN|" + NetMsg.conn.connectionId + "|" + LocalIPAddress());
            StringMessageToClients("CN|" + NetMsg.conn.connectionId);

            if (CurrentPlayers == maxPlayers) StartGame();
        }
    }

    public void StartGame()
    {
        StringMessageToClients("ST");
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene("Game Scene");
        SoundsManager.getInstance().StopAllSounds();
        Invoke("ActivatePlayer", 1.5f);

    }

    protected override void ActivatePlayer()
    {
        if (Practice) GameManager.getInstance().SetPlayerNumber(1, true);
        else GameManager.getInstance().SetPlayerNumber(maxPlayers, false);
        GameManager.getInstance().ActivatePlayer(PlayerNumber);
        for (int i = 0; i < 4; i++) NPCPlayers[i] = GameManager.getInstance().transform.GetChild(3).GetChild(i).gameObject.GetComponent<NPCController>();

        // PlatformManager.getInstance().InitializeConfiguration();

    }

    public override void StringMessageToPlayer(int i, string ToSend)
    {
        StringMessage msg = new StringMessage();
        msg.value = ToSend;
        NetworkServer.SendToClient(i, 888, msg);
    }

    public override void StringMessageToAll(string ToSend)
    {
        StringMessageToClients(ToSend);
        StringMessageReader(ToSend);

    }

    public override void StringMessageToClients(string ToSend) 
    {
        for (int i = 1; i < CurrentPlayers; i++)
            StringMessageToPlayer(i, ToSend);
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
            case "RD":
                StringMessageToClients(msg);
                int PlayerReady;
                if (Int32.TryParse(deltas[1], out PlayerReady))
                {
                    GameManager.getInstance().SetPlayerReady(PlayerReady);
                }
                break;

            case "WIN":
                StringMessageToClients(msg);
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
            // ------------------------------------------------------ TO MODIFY, OPTIMIZE FOR 2 PLAYERS ------------------------------------------------------
            case "SBP":
                // StringMessageToClients(msg);
                NPCPlayers[0].UpdatePosition(float.Parse(deltas[1]), float.Parse(deltas[2]), float.Parse(deltas[3]));
                break;
            case "SEP":
                // StringMessageToClients(msg);
                NPCPlayers[1].UpdatePosition(float.Parse(deltas[1]), float.Parse(deltas[2]), float.Parse(deltas[3]));
                break;
            case "SLP":
                // StringMessageToClients(msg);
                NPCPlayers[2].UpdatePosition(float.Parse(deltas[1]), float.Parse(deltas[2]), float.Parse(deltas[3]));
                break;
            case "SSP":
                // StringMessageToClients(msg);
                NPCPlayers[3].UpdatePosition(float.Parse(deltas[1]), float.Parse(deltas[2]), float.Parse(deltas[3]));
                break;

            // ------------------------------------------------------ POWERS EFFECTS ------------------------------------------------------
            case "FP":
                StringMessageToClients(msg);
                int FirePowerSafePlayer;
                if (Int32.TryParse(deltas[1], out FirePowerSafePlayer))
                {
                    PowerUpsManager.getInstance().transform.GetChild(1).gameObject.SetActive(false);
                    GameCanvas.getInstance().transform.GetChild(3).GetChild(5).gameObject.SetActive(false);

                    for (int i = 0; i < 4; i++)
                    {
                        if (i != FirePowerSafePlayer)
                        {
                            if (i == PlayerNumber) GameManager.getInstance().transform.GetChild(3).GetChild(i).GetComponent<BaseController>().SetOnFire();
                            else GameManager.getInstance().transform.GetChild(3).GetChild(i).GetComponent<NPCController>().SetOnFire();
                        }

                    }
                }
                break;

            case "IP":
                StringMessageToClients(msg);
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
                StringMessageToClients(msg);
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
                StringMessageToClients(msg);
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

    public string LocalIPAddress() { IPHostEntry host; string localIP = ""; host = Dns.GetHostEntry(Dns.GetHostName()); foreach (IPAddress ip in host.AddressList) { if (ip.AddressFamily == AddressFamily.InterNetwork) { localIP = ip.ToString(); break; } } return localIP; }

}
