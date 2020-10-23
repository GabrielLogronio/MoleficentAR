using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUpsManager : MonoBehaviour
{
    protected static PowerUpsManager instance = null;

    public static PowerUpsManager getInstance()
    {
        return instance;
    }

    float FixedHeight = 15f, FixedDistance = 50f;

    float TimeBetween = 60f, Duration = 20f, Countdown = 3f;

    int CurrentActive = -1, Max = -1;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this);

    }

    public void StartPowerUpsManager()
    {
        Max = transform.childCount;
        Invoke("Warning", 87f);

    }

    void Warning()
    {
        float RandomSelector = UnityEngine.Random.value * Max;
        int newActive = (int)RandomSelector;

        CalculatePosition(newActive);

        Invoke("Warning", TimeBetween);
    }

    public void Warning(int Selected, float X, float Z)
    {
        CurrentActive = Selected;

        transform.GetChild(CurrentActive).position = new Vector3(X, FixedHeight, Z);

        SoundsManager.getInstance().PlayPowerUpAlarm();
        GameCanvas.getInstance().transform.GetChild(3).GetChild(CurrentActive).gameObject.GetComponent<FadeOutImage>().Activate();
        GameCanvas.getInstance().transform.GetChild(3).GetChild(5).gameObject.SetActive(true);

        Invoke("Activate", Countdown);
    }
    void CalculatePosition(int Selected)
    {
        float X = UnityEngine.Random.Range(-FixedDistance, FixedDistance);
        float Z;
        if (UnityEngine.Random.value > 0.5f) Z = Mathf.Sqrt((FixedDistance * FixedDistance) - (X * X));
        else Z = Mathf.Sqrt((FixedDistance * FixedDistance) - (X * X)) * (-1);
        NetworkManager.getInstance().StringMessageToClients("PWR|" + Selected + "|" + Math.Round(X, 2) + "|" + Math.Round(Z, 2));

        Warning(Selected, X, Z);

    }

    void Activate()
    {
        transform.GetChild(CurrentActive).gameObject.SetActive(true);
        GameCanvas.getInstance().transform.GetChild(3).GetChild(CurrentActive).gameObject.GetComponent<FadeOutImage>().Deactivate();
        Invoke("Deactivate", Duration);
    }



    void Deactivate()
    {
        GameCanvas.getInstance().transform.GetChild(3).GetChild(5).gameObject.SetActive(false);
        transform.GetChild(CurrentActive).gameObject.SetActive(false);

    }

}
