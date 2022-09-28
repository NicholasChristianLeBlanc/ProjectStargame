using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider2D))]

public class ForcedInteractable : MonoBehaviour
{
    [SerializeField] bool triggerDisables = true;
    [SerializeField] BoxCollider2D colliderTrigger;
    [SerializeField] UnityEvent m_OnInteraction;

    public void DoInteraction()
    {
        m_OnInteraction.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.TryGetComponent<InteractionInstigator>(out InteractionInstigator instigator))
            {
                instigator.SetForcedInteractable(this);

                // disables the trigger that causes the interaction to happen
                if (triggerDisables)
                {
                    colliderTrigger.enabled = false;
                }
            }
            else
            {
                Debug.LogError("No access to player Interaction Instigator in " + gameObject.name);
            }
        }
    }
}
