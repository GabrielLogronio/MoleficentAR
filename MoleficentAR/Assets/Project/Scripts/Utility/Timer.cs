using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer instance = null;

    Text TimerText;

    float CurrentTime = 0f;

    bool Started = false;

    void Start() 
    {
        if (instance == null) instance = this;
        else Destroy(this);

        TimerText = GetComponent<Text>();
    }

    public static Timer getInstance() 
    {
        return instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Started) CurrentTime += Time.deltaTime;

        TimerText.text = Mathf.Floor(CurrentTime / 60).ToString("00") + ":" + (CurrentTime % 60).ToString("00");

    }

    public void StartTimer() 
    {
        Started = true;
    }

}
