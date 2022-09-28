using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Battle Character")]
public class BattleCharacter : ScriptableObject
{
    [SerializeField] private Sprite m_CharacterImage;
    [SerializeField] private string m_CharacterName;

    [SerializeField] private BattleChoice[] m_BattleChoices;

    [SerializeField] private float m_MaxSatisfaction;
    [SerializeField] private float m_CurrentSatisfaction;

    [SerializeField] [Range(0.0f, 1000.0f)] private float m_MaxHealth;
    [SerializeField] [Range(0.0f, 1000.0f)] private float m_CurrentHealth;

    int lastChoice = 0;

    public Sprite CharacterImage => m_CharacterImage;
    public string CharacterName => m_CharacterName;

    public BattleChoice[] BattleChoices => m_BattleChoices;

    public float MaxSatisfaction => m_MaxSatisfaction;
    public float CurrentSatisfaction => m_CurrentSatisfaction;

    public void SetMaxSatisfaction(float newSatisfaction)
    {
        m_MaxSatisfaction = newSatisfaction;
    }

    public void PerformAct(int choice)
    {
        if (choice <= m_BattleChoices.Length - 1)
        {
            m_CurrentSatisfaction += m_BattleChoices[choice].Satisfaction;

            lastChoice = choice;
        }
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
}
