using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Battle/Choice")]
public class BattleChoice : ScriptableObject
{
    enum ResponseTypes
    {
        linear,
        random,
        cycle
    }

    [Header("Default Variables")]
    [SerializeField] private string m_ChoiceText;
    [SerializeField] private string[] m_Responses;
    [SerializeField] private float m_Satisfaction;

    [Header("Behavior Variables")]
    [SerializeField] [Tooltip("straight - switches responses numerically until the index reaches max in which case the response will stay the same " +
                              "\n\nrandom - chooses a random response from the list of responses" +
                              "\n\ncycle - similar to straight but when the index maxes out it resets the index at zero to start counting up again")] private ResponseTypes m_ResponseType;
    [SerializeField] bool m_SatisfactionScales = false;
    [SerializeField] [Range(-100.0f, 100.0f)] float m_SatisfactionScaling = 0.0f;

    private int m_CurrentResponseIndex = 0;

    public string ChoiceText => m_ChoiceText;
    public string[] Responses => m_Responses;
    public float Satisfaction => m_Satisfaction;

    public string GetResponse()
    {
        string returnResponse = Responses[m_CurrentResponseIndex];

        TryIncreaseIndex();
        ScaleSatisfaction();

        return returnResponse;
    }

    private void TryIncreaseIndex()
    {
        if (m_ResponseType == ResponseTypes.random)
        {
            m_CurrentResponseIndex = Random.Range(0, Responses.Length);
        }
        else
        {
            if (m_CurrentResponseIndex + 1 <= Responses.Length - 1)
            {
                m_CurrentResponseIndex++;
            }
            else
            {
                if (m_ResponseType == ResponseTypes.linear)
                {
                    m_CurrentResponseIndex = Responses.Length - 1;
                }
                else if (m_ResponseType == ResponseTypes.cycle)
                {
                    m_CurrentResponseIndex = 0;
                }
            }
        }
    }

    private void ScaleSatisfaction()
    {
        if (m_SatisfactionScales)
        {
            if (m_Satisfaction < 0)
            {
                if (m_SatisfactionScaling < 0)
                {
                    m_Satisfaction -= Mathf.Abs(m_Satisfaction * (m_SatisfactionScaling / 100));
                }
                else if (m_SatisfactionScaling > 0)
                {
                    m_Satisfaction += Mathf.Abs(m_Satisfaction * (m_SatisfactionScaling / 100));
                }
                else // scaling is set to zero
                {
                    m_SatisfactionScales = false;
                }
            }
            else if (m_Satisfaction > 0)
            {
                if (m_SatisfactionScaling < 0)
                {
                    m_Satisfaction -= Mathf.Abs(m_Satisfaction * (m_SatisfactionScaling / 100));
                }
                else if (m_SatisfactionScaling > 0)
                {
                    m_Satisfaction += Mathf.Abs(m_Satisfaction * (m_SatisfactionScaling / 100));
                }
                else // scaling is set to zero
                {
                    m_SatisfactionScales = false;
                }
            }
            else // Satisfaction is zero
            {
                if (m_SatisfactionScaling < 0)
                {
                    m_Satisfaction -= Mathf.Abs(m_SatisfactionScaling / 100);
                }
                else if (m_SatisfactionScaling > 0)
                {
                    m_Satisfaction += Mathf.Abs(m_SatisfactionScaling / 100);
                }
                else // scaling is set to zero
                {
                    m_SatisfactionScales = false;
                }
            }
        }
    }
}
