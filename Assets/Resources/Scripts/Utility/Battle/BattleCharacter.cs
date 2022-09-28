using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Battle Character")]
public class BattleCharacter : ScriptableObject
{
    [SerializeField] private Sprite m_CharacterImage;
    [SerializeField] private string m_CharacterName;

    [SerializeField] private string[] m_ActChoices; // what options the player will have when they choose "act" in battle with this character
    [SerializeField] private float[] m_ActSatisfaction; // how much satisfaction the character will gain from the choices

    [SerializeField] private float m_MaxSatisfaction;
    [SerializeField] private float m_CurrentSatisfaction;

    [SerializeField] [Range(0.0f, 1000.0f)] private float m_MaxHealth;
    [SerializeField] [Range(0.0f, 1000.0f)] private float m_CurrentHealth;

    public Sprite CharacterImage => m_CharacterImage;
    public string CharacterName => m_CharacterName;

    public string[] ActChoices => m_ActChoices;
    public float[] ActSatisfaction => m_ActSatisfaction;

    public float MaxSatisfaction => m_MaxSatisfaction;
    public float CurrentSatisfaction => m_CurrentSatisfaction;



    public void SetMaxSatisfaction(float newSatisfaction)
    {
        m_MaxSatisfaction = newSatisfaction;
    }

    public void PerformAct(int choice)
    {
        if (choice <= m_ActChoices.Length - 1 && choice <= m_ActSatisfaction.Length - 1)
        {
            m_CurrentSatisfaction += m_ActSatisfaction[choice];
        }
    }

    public void ResetSatisfaction()
    {
        m_CurrentSatisfaction = 0;
    }
}
