using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleHandler : MonoBehaviour
{
    [SerializeField] GameObject battlePlayer;
    [SerializeField] BattleUIHandler uiHandler;

    GameObject attack;

    Health playerHealth;
    Movement playerMovement;

    private void Awake()
    {
        if (battlePlayer != null && battlePlayer.TryGetComponent<Movement>(out Movement move))
        {
            playerMovement = move;
        }

        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent<Health>(out Health h))
        {
            playerHealth = h;
        }
    }

    private void LateUpdate()
    {
        GetAttackStatus();
    }

    public void MoveVertical(float scale)
    {
        if (playerMovement != null)
        {
            playerMovement.MoveVertical(scale);
        }
    }

    public void MoveHorizontal(float scale)
    {
        if (playerMovement != null)
        {
            playerMovement.MoveHorizontal(scale);
        }
    }

    public void StartAttack(GameObject newAttack)
    {
        if (newAttack != null)
        {
            attack = Instantiate(newAttack, gameObject.transform);

            if (battlePlayer != null)
            {
                battlePlayer.transform.position = transform.position;
            }
        }
    }

    public void EndAttack()
    {
        Destroy(attack);
        uiHandler.EndCharacterAttack();
    }

    public void GetAttackStatus()
    {
        if (attack != null && attack.TryGetComponent<AttackStatus>(out AttackStatus status))
        {
            if (status.GetAttackFinished())
            {
                EndAttack();
            }
        }
    }

    public void SetUiHandler(BattleUIHandler newUI)
    {
        uiHandler = newUI;
    }

    public GameObject GetBattlePlayer()
    {
        return battlePlayer;
    }
}
