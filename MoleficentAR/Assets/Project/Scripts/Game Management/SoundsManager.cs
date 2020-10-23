using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    static SoundsManager instance = null;

    [SerializeField]
    AudioClip[] Clips;

    AudioSource ASource;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(this);
        DontDestroyOnLoad(gameObject);

        ASource = GetComponent<AudioSource>();

    }

    public static SoundsManager getInstance() 
    {
        return instance;
    }

    public void PlayMenuMusic() 
    {
        ASource.PlayOneShot(Clips[0], 0.5f);
    }
    public void PlayIngameMusic()
    {
        ASource.PlayOneShot(Clips[1], 0.5f);
    }
    public void PlayMenuSelectSound()
    {
        ASource.PlayOneShot(Clips[2]);
    }
    public void PlayOnFireEffect() 
    {
        ASource.PlayOneShot(Clips[3]);
    }
    public void PlayFrozenEffect()
    {
        ASource.PlayOneShot(Clips[4]);
    }
    public void PlayConfusedEffect()
    {
        ASource.PlayOneShot(Clips[5]);
    }
    public void PlayBounceEffect()
    {
        ASource.PlayOneShot(Clips[6]);
    }
    public void PlayObstacleAlarm() 
    {
        ASource.PlayOneShot(Clips[7]);
    }
    public void PlayPowerUpAlarm()
    {
        ASource.PlayOneShot(Clips[8]);
    }
    public void PlayWinner()
    {
        ASource.PlayOneShot(Clips[9]);
    }
    public void StopAllSounds() 
    {
        ASource.Stop();
    }
}
