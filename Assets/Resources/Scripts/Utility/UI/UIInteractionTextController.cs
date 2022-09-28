using UnityEngine;
using TMPro;

public class UIInteractionTextController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Text;
    [SerializeField] private bool ShowTextInEditor = true;
    [SerializeField] private InteractionInstigator m_WatchedInteractionInstigator;

    private void OnValidate()
    {
        if (ShowTextInEditor)
        {
            m_Text.enabled = true;
        }
        else
        {
            m_Text.enabled = false;
        }
    }

    void Update()
    {
        //This is overkill it could be handled with events.
        m_Text.enabled = m_WatchedInteractionInstigator.enabled && m_WatchedInteractionInstigator.HasNearbyInteractables();
    }
}
