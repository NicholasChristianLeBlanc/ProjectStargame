using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackStatus))]

public class KnifeThrow : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] GameObject battlePlayer;
    [SerializeField] BattleHandler battleHandler;

    [Header("References")]
    [SerializeField] GameObject target;
    [SerializeField] GameObject knife;
    [SerializeField] int knivesToThrow = 7;
    [SerializeField] Transform[] waypoints;
    [SerializeField] Transform[] spawnpoints;

    [Header("Delays")]
    [SerializeField] [Range(0.0f, 10.0f)] float spawnDelay;
    [SerializeField] [Range(0.0f, 10.0f)] float throwDelay;

    [Header("Randomized Delay")]
    [SerializeField] bool throwRandomized = false;
    [SerializeField] [Range(0.0f, 10.0f)] float min_ThrowDelay;
    [SerializeField] [Range(0.0f, 10.0f)] float max_ThrowDelay;

    AttackStatus status;

    float spawnCounter = 0;
    float throwCounter = 0;
    float despawnCounter = 2;

    int knivesThrown = 0;

    bool slowing = false;

    int index = 0;

    private void Awake()
    {
        status = gameObject.GetComponent<AttackStatus>();
    }

    private void OnEnable()
    {
        if (gameObject.GetComponentInParent<BattleHandler>() != null)
        {
            battleHandler = gameObject.GetComponentInParent<BattleHandler>();

            battlePlayer = battleHandler.GetBattlePlayer();

            if (target != null)
            {
                //battlePlayer.transform.SetParent(target.transform);
            }
        }
        else
        {
            Debug.LogError("Unable to find Battlehandler in parent object");
        }

        spawnCounter = spawnDelay;
        throwCounter = throwDelay;
    }

    private void Update()
    {
        if (spawnCounter > 0)
        {
            spawnCounter -= Time.deltaTime;
        }
        else
        {
            if (throwCounter > 0)
            {
                throwCounter -= Time.deltaTime;
            }
            else
            {
                if (knivesThrown < knivesToThrow)
                {
                    SpawnKnife();
                    throwCounter = throwDelay;
                }
            }

            if (slowing)
            {
                if (target.GetComponent<Spin>().GetSpeed() <= 0)
                {
                    if (despawnCounter > 0)
                    {
                        despawnCounter -= Time.deltaTime;
                    }
                    else
                    {
                        status.FinishAttack();
                    }
                }
            }
        }
    }

    public void SpawnKnife()
    {
        if (knife != null)
        {
            GameObject newKnife = Instantiate(knife, this.transform);
            
            int i = Random.Range(0, spawnpoints.Length);
            Vector2 spawnLocation = spawnpoints[i].position;

            if (Random.value > 0.5)
            {
                if (newKnife.transform.position.y < battlePlayer.transform.position.y) // knife spawned below the player
                {
                    spawnLocation.y += Random.Range(0.0f, 10.0f);
                }
                else // knife spawned above the player
                {
                    spawnLocation.y -= Random.Range(0.0f, 10.0f);
                }
            }
            else
            {
                if (newKnife.transform.position.x < battlePlayer.transform.position.x) // knife spawned below the player
                {
                    spawnLocation.x += Random.Range(0.0f, 10.0f);
                }
                else // knife spawned above the player
                {
                    spawnLocation.x -= Random.Range(0.0f, 10.0f);
                }
            }

            newKnife.transform.position = spawnLocation;

            if (newKnife.TryGetComponent<Knife>(out Knife k))
            {
                index++;

                if (index >= waypoints.Length)
                {
                    index = 0;
                }

                k.SetWaypoint(waypoints[index]);
                k.SetTarget(target);
            }

            newKnife.SetActive(false); 
            newKnife.SetActive(true);

            knivesThrown++;

            if (knivesThrown >= knivesToThrow)
            {
                if (target.TryGetComponent<Spin>(out Spin spin))
                {
                    spin.StopSpinning();
                    slowing = true;
                }
            }
        }
    }
}
