using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] [Range(0, 5)] private float timeScale = 3;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = timeScale;
    }
}
