using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField]
    private float maxHeight;

    [SerializeField]
    private float minHeight;
    
    public float GetMaxHeight()
    {
        return maxHeight;
    }

    public float GetMinheight()
    {
        return minHeight;
    }
}
