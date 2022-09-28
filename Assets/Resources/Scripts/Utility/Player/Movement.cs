using UnityEngine;

[RequireComponent(typeof (Rigidbody2D))]

public class Movement : MonoBehaviour
{
    [Header("Base Movement")]
    [SerializeField] [Range(0, 10)] private float movementSpeed = 2;
    [SerializeField] [Range(0, 10)] private float runSpeed = 3;

    private Rigidbody2D rb;

    private float horizontalSpeed = 0.0f;
    private float verticalSpeed = 0.0f;
    private bool running = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalSpeed, verticalSpeed);
    }

    public void MoveHorizontal(float scale)
    {
        if (!running)
        {
            horizontalSpeed = movementSpeed * scale;
        }
        else
        {
            horizontalSpeed = runSpeed * scale;
        }
    }

    public void MoveVertical(float scale)
    {
        if (!running)
        {
            verticalSpeed = movementSpeed * scale;
        }
        else
        {
            verticalSpeed = runSpeed * scale;
        }
    }

    public void Run()
    {
        running = !running;
    }
    public void Run(bool isRunning)
    {
        running = isRunning;
    }
}
