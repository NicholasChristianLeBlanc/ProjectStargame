using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] [Range(0, 100)] private float maxHealth = 100;
    [SerializeField] [Range(0, 100)] private float currentHealth = 100;

    [Header("Shield")]
    [SerializeField] private bool shieldEnabled = false;
    [SerializeField] [Range(0, 50)] private float maxShield = 50;
    [SerializeField] [Range(0, 50)] private float currentShield = 50;

    bool takingDamage = false;
    float damageDelay = 1;
    float damageCounter;

    private void OnValidate()
    {
        if (currentHealth != maxHealth)
        {
            takingDamage = true;
            damageCounter = damageDelay;
        }
    }

    private void Update()
    {
        if (damageCounter > 0)
        {
            damageCounter -= Time.deltaTime;
        }
        else
        {
            takingDamage = false;
        }
    }

    // Current Health
    public void SetCurrentHealth(float health) // to set a specific health amount
    {
        currentHealth = health;

        if (health > maxHealth)
        {
            maxHealth = health;
        }
    }
    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    // Max health
    public void SetMaxHealth(float newMax) // used for setting a new max health
    {
        maxHealth = newMax;

        if (currentHealth > newMax)
        {
            currentHealth = newMax;
        }
    }
    public float GetMaxHealth()
    {
        return maxHealth;
    }

    // Current Shield
    public void SetCurrentShield(float shield) // for setting the current shield to a specific amount
    {
        currentShield = shield;

        if (shield > maxShield)
        {
            maxShield = shield;
        }
    }
    public float GetCurrentShield()
    {
        return currentShield;
    }

    // Max Shield
    public void SetMaxShield(float newMax) // can be used to set a new max shield amount
    {
        maxShield = newMax;

        if (currentShield >= newMax)
        {
            currentShield = newMax;
        }
    }
    public float GetMaxShield()
    {
        return maxShield;
    }

    public void DealDamage(float damage) // for physical attacks
    {
        if (damage > 0)
        {
            if (currentShield > 0 && shieldEnabled)
            {
                currentShield -= damage;

                if (currentShield < 0)
                {
                    currentHealth -= 0 - currentShield;
                }
            }
            else
            {
                currentHealth -= damage;
            }

            takingDamage = true;
            damageCounter = damageDelay;
        }
    }

    public void RemoveJustHealth(float removeAmount) // for non-physical attacks
    {
        if (removeAmount > 0)
        {
            currentHealth -= removeAmount;

            takingDamage = true;
            damageCounter = damageDelay;
        }
    }

    public void AddHealth(float healAmount) // for healing (duh)
    {
        currentHealth += healAmount;

        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public bool GetTakingDamage()
    {
        return takingDamage;
    }
}
