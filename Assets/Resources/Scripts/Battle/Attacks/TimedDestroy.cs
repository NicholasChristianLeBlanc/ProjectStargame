using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDestroy : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 10.0f)] float destroyDelay;

    float counter;

    private void OnEnable()
    {
        counter = destroyDelay;
    }

    private void Update()
    {
        if (counter > 0)
        {
            counter -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
