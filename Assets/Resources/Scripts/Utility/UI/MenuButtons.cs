using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour
{
    [SerializeField] GameObject leftElement; // the ui element to the left of this one
    [SerializeField] GameObject rightElement; // the ui element to the right of this one
    [SerializeField] GameObject upElement; // the ui element above this one
    [SerializeField] GameObject downElement; // the ui element below this one

    public GameObject GetLeft()
    {
        return leftElement;
    }

    public GameObject GetRight()
    {
        return rightElement;
    }

    public GameObject GetUp()
    {
        return upElement;
    }

    public GameObject GetDown()
    {
        return downElement;
    }
}
