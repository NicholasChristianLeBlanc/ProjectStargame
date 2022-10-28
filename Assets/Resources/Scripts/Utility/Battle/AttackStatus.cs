using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStatus : MonoBehaviour
{
    bool attackOver = false;

    private void OnEnable()
    {
        attackOver = false;
    }

    public void FinishAttack()
    {
        attackOver = true;
    }

    public bool GetAttackFinished()
    {
        return attackOver;
    }
}
