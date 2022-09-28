using System.Collections.Generic;
using UnityEngine;

public class InteractionInstigator : MonoBehaviour
{
    private List<Interactable> m_NearbyInteractables = new List<Interactable>();

    ForcedInteractable forceInteract = null;

    bool interacted = false;

    public bool HasNearbyInteractables()
    {
        return m_NearbyInteractables.Count != 0;
    }

    private Interactable GetClosestInteractable()
    {
        Interactable closestInteractable = null;

        foreach (Interactable interactable in m_NearbyInteractables)
        {
            if (closestInteractable == null)
            {
                closestInteractable = interactable;
                continue;
            }
            else if (Vector3.Distance(interactable.gameObject.transform.position, gameObject.transform.position) < Vector3.Distance(closestInteractable.gameObject.transform.position, gameObject.transform.position))
            {
                closestInteractable = interactable;
            }
        }

        return closestInteractable;
    }

    public void SetForcedInteractable(ForcedInteractable newInteractable)
    {
        forceInteract = newInteractable;
    }

    private void Update()
    {
        if (forceInteract != null)
        {
            forceInteract.DoInteraction();
            
            interacted = false;
            forceInteract = null;
        }
        else
        {
            if (HasNearbyInteractables() && interacted)
            {
                GetClosestInteractable().DoInteraction();
                interacted = false;
            }
        }
    }

    public void SetInteracted(bool newBool)
    {
        interacted = newBool;
    }

    // 3D
    private void OnTriggerEnter(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable != null)
        {
            m_NearbyInteractables.Add(interactable);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable != null)
        {
            m_NearbyInteractables.Remove(interactable);
        }
    }

    // 2D
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Interactable interactable = collision.GetComponent<Interactable>();

        if (interactable != null)
        {
            m_NearbyInteractables.Add(interactable);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Interactable interactable = collision.GetComponent<Interactable>();

        if (interactable != null)
        {
            m_NearbyInteractables.Remove(interactable);
        }
    }
}
