using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]

public class Knife : MonoBehaviour
{
    [SerializeField] Sprite throwSprite;
    [SerializeField] Sprite hitSprite;
    [SerializeField] PolygonCollider2D tipCollider;

    [SerializeField] Transform waypoint;
    [SerializeField] float throwSpeed = 10;

    GameObject targetObject;

    Rigidbody2D rb;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        if (waypoint != null)
        {
            Vector2 direction = (gameObject.transform.position - waypoint.position).normalized;
            rb.velocity = -direction * throwSpeed;

            if (throwSprite != null)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = throwSprite;
            }
        }
    }

    private void Update()
    {
        Vector2 moveDirection = rb.velocity;
        if (moveDirection != Vector2.zero && !rb.isKinematic)
        {
            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == waypoint.gameObject)
        {
            transform.SetParent(targetObject.transform);
            rb.isKinematic = true;

            rb.velocity = Vector2.zero;

            if (hitSprite != null)
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = hitSprite;
            }

            if (tipCollider != null)
            {
                tipCollider.enabled = false;
            }
        }
    }

    public void SetTarget(GameObject target)
    {
        targetObject = target;
    }

    public void SetWaypoint(Transform point)
    {
        waypoint = point;
    }
}
