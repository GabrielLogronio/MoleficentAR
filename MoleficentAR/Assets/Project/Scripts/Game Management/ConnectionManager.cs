using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionManager : MonoBehaviour
{
    static ConnectionManager instance = null;

    [SerializeField]
    GameObject NetworkServerInstance, NetworkClientInstance, Canvas;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this);

        SoundsManager.getInstance().PlayMenuMusic();

    }

    public static ConnectionManager getInstance()
    {
        return instance;
    }

    public void ConnectedPlayer(int Num)
    {
        for (int i = 0; i <= Num; i++)
        {
            Canvas.transform.GetChild(2).GetChild(i + 1).gameObject.SetActive(true);

        }
    }

    public void CreateGamePressed()
    {
        SoundsManager.getInstance().PlayMenuSelectSound();

        NetworkServerInstance.SetActive(true);
        NetworkServerInstance.GetComponent<NetworkServerManager>().Activate();

        Canvas.transform.GetChild(1).gameObject.SetActive(false);
        Canvas.transform.GetChild(2).gameObject.SetActive(true);

    }

    public void JoinGamePressed()
    {
        SoundsManager.getInstance().PlayMenuSelectSound();

        NetworkClientInstance.SetActive(true);
        NetworkClientInstance.GetComponent<NetworkClientManager>().Activate();
        Canvas.transform.GetChild(1).gameObject.SetActive(false);
        Canvas.transform.GetChild(5).gameObject.SetActive(true);

    }

    public void PracticePressed()
    {
        SoundsManager.getInstance().PlayMenuSelectSound();

        Canvas.transform.GetChild(1).gameObject.SetActive(false);
        Canvas.transform.GetChild(3).gameObject.SetActive(true);

    }

    public void CharacterSelected(int PlayerNumber)
    {
        SoundsManager.getInstance().PlayMenuSelectSound();

        Canvas.transform.GetChild(3).gameObject.SetActive(false);
        Canvas.transform.GetChild(4).gameObject.SetActive(true);
        NetworkServerInstance.SetActive(true);
        NetworkServerInstance.GetComponent<NetworkServerManager>().Tryout(PlayerNumber);

    }

    public void Connected()
    {
        Canvas.transform.GetChild(1).gameObject.SetActive(false);
        Canvas.transform.GetChild(2).gameObject.SetActive(true);
        Canvas.transform.GetChild(5).gameObject.SetActive(false);

    }

    public void BackButton()
    {
        // if (NetworkServerInstance) NetworkServerManager.getInstance().StringMessageToAll("QT");
        // else NetworkClientManager.getInstance().StringMessageToAll("QT");
        SoundsManager.getInstance().PlayMenuSelectSound();
        SoundsManager.getInstance().StopAllSounds();

        SceneManager.LoadScene("Connection Scene");

    }
}
