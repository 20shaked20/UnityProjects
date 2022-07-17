using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour
{
    public AudioSource BGM;
    public AudioClip OutdoorsBGM;

    void Start()
    {
        ChangeBGM(OutdoorsBGM);
    }

    public void ChangeBGM(AudioClip music)
    {
        Debug.Log("Change Music");
        BGM.Stop();
        BGM.clip = music;
        BGM.Play();
    }
}
