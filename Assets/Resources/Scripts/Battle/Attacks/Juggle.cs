using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackStatus))]

public class Juggle : MonoBehaviour
{
    [SerializeField] [Range(1, 100)] int numberToThrow = 15;

    [SerializeField] Throw[] throwObjects = new Throw[2];
    [SerializeField] float[] throwDelays = new float[2];
    [SerializeField] bool objectsHandleDelay = false;

    [SerializeField] [Range(0.0f, 10.0f)] float attackDelay = 2;
    [SerializeField] [Range(0.0f, 10.0f)] float finishDelay = 1;

    AttackStatus status;

    float[] delays;
    float spawnTimer = 0;
    float endTimer = 0;

    int i;
    int thrownObjects = 0;

    bool attackOver = false;

    private void Awake()
    {
        status = gameObject.GetComponent<AttackStatus>();

        if (objectsHandleDelay)
        {
            if (throwObjects != null)
            {
                i = 0;
                foreach (Throw to in throwObjects)
                {
                    to.SetSpawnDelay(throwDelays[i], true);

                    i++;
                }
            }
        }
        else
        {
            foreach (Throw to in throwObjects)
            {
                to.SetSpawnDelay(0, false);
            }
        }

        delays = new float[throwDelays.Length];

        i = 0;
        foreach (float counter in throwDelays)
        {
            delays[i] = counter;
        }
    }

    private void OnEnable()
    {
        thrownObjects = 0;
        spawnTimer = attackDelay;

        attackOver = false;
    }

    private void Update()
    {
        if (spawnTimer > 0)
        {
            spawnTimer -= Time.deltaTime;
        }
        else
        {
            if (!objectsHandleDelay && thrownObjects < numberToThrow)
            {
                i = 0;
                foreach (float counter in delays)
                {
                    if (counter <= 0)
                    {
                        throwObjects[i].SpawnProjectile();
                        thrownObjects++;

                        delays[i] = throwDelays[i];
                    }
                    else
                    {
                        delays[i] -= Time.deltaTime;
                    }

                    i++;
                }
            }
            else if (thrownObjects >= numberToThrow && !attackOver)
            {
                endTimer = finishDelay;
                attackOver = true;
            }
        }

        if (attackOver && endTimer > 0)
        {
            endTimer -= Time.deltaTime;
        }
        else if (attackOver && endTimer <= 0)
        {
            EndAttack();
        }
    }

    private void EndAttack()
    {
        status.FinishAttack();
    }
}
