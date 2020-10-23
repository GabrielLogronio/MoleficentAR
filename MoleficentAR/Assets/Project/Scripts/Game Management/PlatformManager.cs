using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    static PlatformManager instance = null;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this);

    }

    public static PlatformManager getInstance()
    {
        return instance;
    }

    [SerializeField]
    GameObject[] Platforms;

    [SerializeField]
    float ChangeTime;

    [SerializeField]
    bool[,] PlatformConfigurations;
    bool Started = false;

    int ConfigurationNumbers = 10, CurrentConfiguration = -1;

    public void InitializeConfiguration()
    {
        PlatformConfigurations = new bool[ConfigurationNumbers, Platforms.Length];
        string ConfigurationString = "PC|";
        for (int Configuration = 0; Configuration < ConfigurationNumbers; Configuration++)
        {
            for (int Platform = 0; Platform < Platforms.Length; Platform++)
            {
                if (Random.value > 0.5f)
                {
                    PlatformConfigurations[Configuration, Platform] = true;
                    ConfigurationString += "T";
                }
                else
                {
                    PlatformConfigurations[Configuration, Platform] = false;
                    ConfigurationString += "F";
                }
            }
            ConfigurationString += "|";

        }

        NetworkManager.getInstance().StringMessageToAll(ConfigurationString);

        Invoke("SwitchConfiguration", ChangeTime);

    }

    public void SetConfiguration(string[] ToSet)
    {
        PlatformConfigurations = new bool[ToSet.Length - 1, ToSet[0].Length];

        for (int Configuration = 1; Configuration < ToSet.Length; Configuration++)
        {
            for (int Platform = 0; Platform < ToSet[Configuration].Length; Platform++)
            {
                if (ToSet[Configuration][Platform] == 'T') PlatformConfigurations[Configuration, Platform] = true;
                else PlatformConfigurations[Configuration, Platform] = false;
            }
        }

        Invoke("SwitchConfiguration", ChangeTime);

    }

    void SwitchConfiguration()
    {
        CurrentConfiguration++;

        if (CurrentConfiguration == ConfigurationNumbers) CurrentConfiguration = 0;

        for (int Platform = 0; Platform < PlatformConfigurations.GetLength(1); Platform++)
        {
            Platforms[Platform].SetActive(PlatformConfigurations[CurrentConfiguration, Platform]);
        }

        Invoke("SwitchConfiguration", ChangeTime);

    }

}
