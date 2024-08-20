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

    [field: Header("Player Shrink")]
    [field: SerializeField] public EventReference playerShrink { get; private set; }


    [field: Header("Ant Footsteps")]
    [field: SerializeField] public EventReference antFootsteps { get; private set; }

    [field: Header("Ant Attack")]
    [field: SerializeField] public EventReference antAttack { get; private set; }

    [field: Header("Ant Die")]
    [field: SerializeField] public EventReference antDie { get; private set; }


    [field: Header("Micro Footsteps")]
    [field: SerializeField] public EventReference microFootsteps { get; private set; }

    [field: Header("Micro Attack")]
    [field: SerializeField] public EventReference microAttack { get; private set; }

    [field: Header("Micro Die")]
    [field: SerializeField] public EventReference microDie { get; private set; }


    [field: Header("Rat Footsteps")]
    [field: SerializeField] public EventReference ratFootsteps { get; private set; }

    [field: Header("Rat Attack")]
    [field: SerializeField] public EventReference ratAttack { get; private set; }

    [field: Header("Rat Die")]
    [field: SerializeField] public EventReference ratDie { get; private set; }


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

