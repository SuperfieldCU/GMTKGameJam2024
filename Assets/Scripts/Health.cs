using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public enum ECharacterType { Player, Enemy, Object };

    [SerializeField] private ECharacterType charType;

    [SerializeField] private int health;

    [SerializeField] private float invulnerabilityTime;

    private bool bIsInvulnerable;
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Health: " + health);
        StartCoroutine(Invulnerability());
    }

    IEnumerator Invulnerability()
    {
        bIsInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        bIsInvulnerable = false;
    }
}
