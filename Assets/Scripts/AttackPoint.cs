using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPoint : MonoBehaviour
{
    enum AttackType { Melee, Ranged };

    [SerializeField]
    private AttackType attackType;
}
