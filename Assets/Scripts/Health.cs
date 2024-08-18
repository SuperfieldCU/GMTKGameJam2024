using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    enum CharacterType { Player, Enemy, Object };

    [SerializeField] private CharacterType charType;

    [SerializeField] private int health;

    [SerializeField] private float invulnerabilityTime;

    private bool bIsInvulnerable;
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        StartCoroutine(Invulnerability());
    }

    IEnumerator Invulnerability()
    {
        bIsInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        bIsInvulnerable = false;
    }
}
