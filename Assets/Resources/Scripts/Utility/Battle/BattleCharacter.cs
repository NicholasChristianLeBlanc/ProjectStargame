using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Battle Character")]
public class BattleCharacter : ScriptableObject
{
    [Header("Attributes")]
    [SerializeField] private Sprite m_CharacterImage;
    [SerializeField] private string m_CharacterName;

    [Header("Lists")]
    [SerializeField] private BattleChoice[] m_BattleChoices;    // All the choices the player has to interact with this Character
    [SerializeField] private GameObject[] m_attacks;    // Every attack that this Character has

    [Header("Satisfaction")]
    [SerializeField] private float m_MaxSatisfaction;   // how much satisfaction the player must accumulate in able for them to run from the battle
    [SerializeField] private float m_CurrentSatisfaction;   // The current satisfaction this Character has towards the player

    [Header("Health")]
    [SerializeField] [Range(0.0f, 1000.0f)] private float m_MaxHealth;
    [SerializeField] [Range(0.0f, 1000.0f)] private float m_CurrentHealth;

    [Header("Defense")]
    [SerializeField] [Range(0.0f, 100.0f)] private float m_Defense;

    [Header("Debug")]
    [SerializeField] bool m_AttacksFirst = false;   // if the first thing this Character does is attack the player instead of letting the character choose an option first
    [SerializeField] bool m_TakesDamage = true;
    [SerializeField] bool m_GainsSatisfaction = true;

    int lastChoice = 0;
    int currentAttack = 0;

    public Sprite CharacterImage => m_CharacterImage;
    public string CharacterName => m_CharacterName;

    public BattleChoice[] BattleChoices => m_BattleChoices;

    public GameObject[] Attacks => m_attacks;

    public float MaxSatisfaction => m_MaxSatisfaction;
    public float CurrentSatisfaction => m_CurrentSatisfaction;

    public float MaxHealth => m_MaxHealth;
    public float CurrentHealth => m_CurrentHealth;

    public float Defense => m_Defense;

    public bool AttacksFirst => m_AttacksFirst;

    private void OnValidate()
    {
        if (currentAttack >= m_attacks.Length)
        {
            currentAttack = 0;
        }
    }

    public void SetMaxSatisfaction(float newSatisfaction)
    {
        m_MaxSatisfaction = newSatisfaction;
    }

    public void PerformAct(int choice)
    {
        if (m_GainsSatisfaction)
        {
            if (choice <= m_BattleChoices.Length - 1)
            {
                m_CurrentSatisfaction += m_BattleChoices[choice].Satisfaction;

                lastChoice = choice;
            }
        }
    }

    public void ResetHealth()
    {
        m_CurrentHealth = m_MaxHealth;
    }

    public void ResetSatisfaction()
    {
        m_CurrentSatisfaction = 0;
    }
    public void ResetSatisfaction(bool decreaseMax, float amountToDecrease)
    {
        m_CurrentSatisfaction = 0;

        if (decreaseMax)
        {
            if (MaxSatisfaction - amountToDecrease <= 0)
            {
                SetMaxSatisfaction(0);
            }
            else
            {
                SetMaxSatisfaction(MaxSatisfaction - amountToDecrease);
            }
        }
    }

    public string GetResponse()
    {
        return BattleChoices[lastChoice].GetResponse();
    }

    public void Damage(float damageToDeal)
    {
        if (damageToDeal > 0 && m_TakesDamage)
        {
            if (Defense >= 100)
            {
                m_CurrentHealth -= 1;
            }
            else if (Defense < 100 && Defense > 0)
            {
                m_CurrentHealth -= damageToDeal * (1 - (Defense / 100));
            }
            else
            {
                m_CurrentHealth -= damageToDeal;
            }
        }
    }

    public GameObject GetCurrentAttack()
    {
        return m_attacks[currentAttack];
    }

    public void GetNextAttack()
    {
        if (currentAttack + 1 <= m_attacks.Length - 1)
        {
            currentAttack++;
        }
        else
        {
            currentAttack = 0;
        }
    }
}
