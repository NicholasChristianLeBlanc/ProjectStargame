using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throw : MonoBehaviour
{
    [SerializeField] GameObject projectile;
    [SerializeField] bool handleDelay = true;
    [SerializeField] float spawnDelay;

    [SerializeField] Vector2 throwSpeed;
    [SerializeField] float min_horizontalOffset, max_horizontalOffset;

    float counter;
    float horizontalOffset;

    private void OnValidate()
    {
        if (min_horizontalOffset > max_horizontalOffset)
        {
            max_horizontalOffset = min_horizontalOffset;
        }

        if (max_horizontalOffset < min_horizontalOffset)
        {
            min_horizontalOffset = max_horizontalOffset;
        }
    }

    private void Start()
    {
        if (spawnDelay > 0)
        {
            counter = spawnDelay;
        }
    }

    private void Update()
    {
        if (handleDelay)
        {
            if (counter > 0)
            {
                counter -= Time.deltaTime;
            }
            else
            {
                if (projectile != null)
                {
                    GameObject item = Instantiate(projectile, this.transform);

                    Vector2 spawnPoint = this.transform.position;
                    spawnPoint.x += Random.Range(min_horizontalOffset, max_horizontalOffset);

                    item.transform.position = spawnPoint;

                    if (item.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                    {
                        rb.velocity = throwSpeed;
                    }

                    item.SetActive(true);

                    counter = spawnDelay;
                }
            }
        }
    }

    public void SetSpawnDelay(float delay, bool handlesDelay)
    {
        counter = delay;
        handleDelay = handlesDelay;
    }

    public void SpawnProjectile()
    {
        if (projectile != null)
        {
            GameObject item = Instantiate(projectile, this.transform);

            Vector2 spawnPoint = this.transform.position;

            float offset = Random.Range(min_horizontalOffset, max_horizontalOffset);

            Vector2 newSpeed = throwSpeed;

            if (newSpeed.x < 0) // Throwing Left
            {
                //newSpeed.x += (1 - offset) / max_horizontalOffset;
                //newSpeed.y += (1 - offset) / max_horizontalOffset;

                newSpeed.x -= offset;
                newSpeed.y += offset / 1.5f;
            }
            else if (newSpeed.x > 0) // Throwing Right
            {
                newSpeed.x += -offset;
                newSpeed.y += -offset / 1.5f;
            }

            spawnPoint.x += offset;

            item.transform.position = spawnPoint;

            item.SetActive(true);

            if (item.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            {
                rb.velocity = newSpeed;

                if (newSpeed.x < 0)
                {
                    rb.angularVelocity = 50f;
                }
                else
                {
                    rb.angularVelocity = -50f;
                }
            }
        }
    }
}
