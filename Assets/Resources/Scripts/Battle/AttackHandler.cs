using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackHandler : MonoBehaviour
{
    [SerializeField] GameObject Player;
    [SerializeField] BattleCharacter character;

    [SerializeField] BattleUIHandler uiHandler;

    PlayerManager playerManager;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        if (Player != null && Player.TryGetComponent<PlayerManager>(out PlayerManager pm)) 
        {
            playerManager = pm;
        }
    }

    public void SetCharacter(BattleCharacter newChar)
    {
        if (newChar != null)
        {
            character = newChar;
        }
        
    }

    public void Attack()
    {
        if (character != null)
        {
            character.Damage(playerManager.GetWeapon().GetWeaponDamage());
        }

        if (uiHandler != null)
        {
            uiHandler.StartCharacterAttack();
        }
    }
}
