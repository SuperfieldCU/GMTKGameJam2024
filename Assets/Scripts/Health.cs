using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public enum ECharacterType { Player, Enemy, Object };

    [SerializeField] private ECharacterType charType;

    [SerializeField]
    private int maxHealth;

    [SerializeField]
    private Slider healthSlider;

    private int healthHolder;
    public int health
    {
        get { return healthHolder; }
        set
        {
            if (healthHolder == value) return;
            healthHolder = value;
            if (OnHealthChanged != null)
                OnHealthChanged(healthHolder);
        }
    }

    [SerializeField] private float invulnerabilityTime;

    private bool bIsInvulnerable;

    public delegate void OnHealthChangedDelegate(int newHealth);
    public event OnHealthChangedDelegate OnHealthChanged;

    private void Awake()
    {
        health = maxHealth;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = health;
    }

    public void TakeDamage(int damage)
    {
        if (!bIsInvulnerable)
        {
            health -= damage;
            healthSlider.value = health;
            StartCoroutine(Invulnerability());
        }
    }

    IEnumerator Invulnerability()
    {
        bIsInvulnerable = true;
        yield return new WaitForSeconds(invulnerabilityTime);
        bIsInvulnerable = false;
    }
}
