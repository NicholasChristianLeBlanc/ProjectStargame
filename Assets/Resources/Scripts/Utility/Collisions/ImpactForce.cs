using UnityEngine;

public class ImpactForce : MonoBehaviour
{
    private Vector2 impactForce;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        impactForce = collision.relativeVelocity;
    }

    public Vector2 GetImpactForce()
    {
        return impactForce;
    }
}
