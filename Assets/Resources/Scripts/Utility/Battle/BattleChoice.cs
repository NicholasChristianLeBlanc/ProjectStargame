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

    [SerializeField] private string m_ChoiceText;
    [SerializeField] private string[] m_Responses;
    [SerializeField] private float m_Satisfaction;

    [SerializeField] [Tooltip("straight - switches responses numerically until the index reaches max in which case the response will stay the same " +
                              "\n\nrandom - chooses a random response from the list of responses" +
                              "\n\ncycle - similar to straight but when the index maxes out it resets the index at zero to start counting up again")] private ResponseTypes m_ResponseType;

    private int m_CurrentResponseIndex = 0;

    public string ChoiceText => m_ChoiceText;
    public string[] Responses => m_Responses;
    public float Satisfaction => m_Satisfaction;

    public string GetResponse()
    {
        string returnResponse = Responses[m_CurrentResponseIndex];

        TryIncreaseIndex();

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
}
