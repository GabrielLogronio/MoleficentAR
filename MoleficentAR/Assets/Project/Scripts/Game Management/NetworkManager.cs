using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public abstract class NetworkManager : MonoBehaviour
{
    protected static NetworkManager instance = null;

    public static NetworkManager getInstance()
    {
        return instance;
    }

    public bool IsServer()
    {
        return instance is NetworkServerManager;
    }

    public abstract void Activate();

    protected abstract void ActivatePlayer();

    public abstract void StringMessageToAll(string ToSend);

    public abstract void StringMessageToClients(string ToSend);

    public abstract void StringMessageToPlayer(int PlayerNumber, string ToSend);

    protected abstract void StringMessageReceiver(NetworkMessage NetMsg);

    protected abstract void StringMessageReader(string msg);

}
