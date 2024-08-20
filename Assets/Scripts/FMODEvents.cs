using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{

    [field: Header("Player Footsteps")]
    [field: SerializeField] public EventReference playerFootsteps { get; private set; }

    [field: Header("Player Attack")]
    [field: SerializeField] public EventReference playerAttack { get; private set; }

    [field: Header("Player Die")]
    [field: SerializeField] public EventReference playerDie { get; private set; }


    [field: Header("Micro Footsteps")]
    [field: SerializeField] public EventReference microFootsteps { get; private set; }

    [field: Header("Micro Attack")]
    [field: SerializeField] public EventReference microAttack { get; private set; }

    [field: Header("Micro Die")]
    [field: SerializeField] public EventReference microDie { get; private set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }



    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD events instance in the scene.");
        }
        instance = this;
    }


}

