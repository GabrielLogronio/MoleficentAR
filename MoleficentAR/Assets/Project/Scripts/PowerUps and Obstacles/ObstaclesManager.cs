using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesManager : MonoBehaviour
{
    static ObstaclesManager instance = null;

    public static ObstaclesManager getInstance()
    {
        return instance;
    }

    float TimeBetween = 60f, Duration = 10f, Countdown = 3f;

    int CurrentActive = -1, Max = -1;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this);

    }

    public void StartObstaclesManager()
    {
        Max = transform.childCount;
        Invoke("Warning", 57f);

    }

    void Warning() 
    {
        float RandomSelector = Random.value * Max;
        int newActive = (int)RandomSelector;

        NetworkManager.getInstance().StringMessageToAll("OBS|" + newActive);
        Invoke("Warning", TimeBetween);

        Warning(newActive);

    }

    public void Warning(int Selected)
    {
        CurrentActive = Selected;

        SoundsManager.getInstance().PlayObstacleAlarm();
        GameCanvas.getInstance().transform.GetChild(3).GetChild(CurrentActive).gameObject.GetComponent<FadeOutImage>().Activate();
        GameCanvas.getInstance().transform.GetChild(3).GetChild(4).gameObject.SetActive(true);

        Invoke("Activate", Countdown);
    }

    void Activate()
    {
        transform.GetChild(CurrentActive).gameObject.SetActive(true);
        GameCanvas.getInstance().transform.GetChild(3).GetChild(CurrentActive).gameObject.GetComponent<FadeOutImage>().Deactivate();
        Invoke("Deactivate", Duration);
    }

    void Deactivate()
    {
        GameCanvas.getInstance().transform.GetChild(3).GetChild(4).gameObject.SetActive(false);
        transform.GetChild(CurrentActive).transform.rotation = Quaternion.identity;
        transform.GetChild(CurrentActive).gameObject.SetActive(false);
        
    }

}
