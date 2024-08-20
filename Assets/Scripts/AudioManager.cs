using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;


public class AudioManager : MonoBehaviour
{

    private EventInstance ambienceEventInstance;

    private EventInstance musicEventInstance;

    public static AudioManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio manager");
        }
        instance = this;
    }

    private void Start()
    {
        //    InitializeAmbience(FMODEvents.instance.ambientSound);
        InitializeMusic(FMODEvents.instance.music);
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateInstance(musicEventReference);
        musicEventInstance.start();
    }




    //private void InitializeAmbience(EventReference ambienceEventReference)
    //{
    //    ambienceEventInstance = CreateInstance(ambienceEventReference);
    //    ambienceEventInstance.start();
    //}

    //public void SetAmbienceParameter(string parameterName,float parameterValue)
    //{
    //   ambienceEventInstance.setParameterByName(parameterName, parameterValue);
    //}

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
        // call one shots as AudioManager.instance.PlayOneShot(FMODEvents.instance.soundName, this.transform.position);
    }

    public EventInstance CreateInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }
}

