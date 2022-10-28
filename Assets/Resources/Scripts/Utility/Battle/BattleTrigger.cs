using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    [SerializeField] private FlowChannel m_FlowChannel;
    [SerializeField] private FlowState m_BattleState;

    [SerializeField] bool triggerOnCollision = false;

    [SerializeField] List<BattleCharacter> triggerCharacters = new List<BattleCharacter>();

    GameObject Player;
    PlayerManager playerManager;

    private void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");

        if (Player.TryGetComponent<PlayerManager>(out PlayerManager manager))
        {
            playerManager = manager;
        }
    }

    private void LateUpdate()
    {
        if (triggerCharacters.Count > 0)
        {
            foreach (BattleCharacter character in triggerCharacters)
            {
                if (character.CurrentHealth <= 0)
                {
                    triggerCharacters.Remove(character);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggerOnCollision && collision.gameObject == Player)
        {
            Trigger();
        }
    }

    public void Trigger()
    {
        if (playerManager != null)
        {
            BattleCharacter chosenCharacter = null;

            if (triggerCharacters.Count > 1)
            {
                int rand = Random.Range(0, triggerCharacters.Count);

                if (triggerCharacters[rand])
                {
                    chosenCharacter = triggerCharacters[rand];
                }
            }
            else if (triggerCharacters.Count <= 1)
            {
                chosenCharacter = triggerCharacters[0];
            }

            if (chosenCharacter != null)
            {
                playerManager.StartBattle(chosenCharacter);
            }
        }
    }
}
