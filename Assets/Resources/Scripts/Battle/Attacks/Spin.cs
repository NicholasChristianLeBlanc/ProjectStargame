using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class Spin : MonoBehaviour
{
    [SerializeField] float maxSpeed = 5;
    [SerializeField] float acceleration = 0.1f;
    [SerializeField] float currentSpeed = 0;

    Rigidbody2D rb;

    bool slow = false;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (slow)
        {
            currentSpeed -= acceleration * Time.fixedDeltaTime;

            if (currentSpeed <= 0)
            {
                currentSpeed = 0;
            }
        }
        else
        {
            currentSpeed += acceleration * Time.fixedDeltaTime;

            if (currentSpeed > maxSpeed)
            {
                currentSpeed = maxSpeed;
            }
        }
        
        rb.angularVelocity = currentSpeed;
    }

    public void StopSpinning()
    {
        slow = true;
    }

    public float GetSpeed()
    {
        return currentSpeed;
    }
}
