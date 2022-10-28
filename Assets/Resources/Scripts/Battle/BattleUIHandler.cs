using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;
using TMPro;

public class BattleUIHandler : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] GameObject player;
    Health playerHealth;
    float currentDisplayedHealth, currentDisplayedLoss;

    [Header("Healthbar")]
    [SerializeField] GameObject healthbarHolder;
    [SerializeField] Image healthbar;
    [SerializeField] Image healthloss;
    [SerializeField] TextMeshProUGUI healthText;
    [SerializeField] Transform selectionWaypoint;
    [SerializeField] Transform fightWaypoint;

    [Header("Interactions")]
    [SerializeField] GameObject selectionIcon;
    [SerializeField] TextMeshProUGUI collectionText;

    [Header("Buttons")]
    [SerializeField] GameObject startingElement;
    [SerializeField] Button btn_fight;
    [SerializeField] Button btn_act;
    [SerializeField] Button btn_items;
    [SerializeField] Button btn_run;

    [Header("Tabs")]
    [SerializeField] GameObject selectionZone;
    [SerializeField] GameObject fightTab;
    [SerializeField] GameObject actTab;
    [SerializeField] GameObject inventoryTab;

    [Header("Battle")]
    [SerializeField] GameObject battleBackground;
    [SerializeField] BattleHandler battleHandler;

    [Header("Debug")]
    [SerializeField] BattleCharacter character;

    // Handlers
    AttackHandler attackHandler;
    ActHandler actHandler;
    InventoryHandler inventoryHandler;

    bool characterAttacking;

    bool inTab = false;
    bool inFight = false;
    bool inAct = false;
    bool inInventory = false;
    
    bool tabSelected = false;

    bool moveHealth = false;

    Transform healthWaypoint;
    GameObject currentElement;

    float t = 0.0f;
    float t2 = 0.0f;
    float t3 = 0.0f;

    PlayerInputs c;

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (player.TryGetComponent<Health>(out Health h))
        {
            playerHealth = h;
        }
        else
        {
            Debug.LogError("Cannot find health component of player in " + gameObject.name);
        }

        healthText.text = playerHealth.GetCurrentHealth().ToString();
        currentDisplayedHealth = playerHealth.GetCurrentHealth();
        currentDisplayedLoss = playerHealth.GetCurrentHealth();

        healthbar.fillAmount = 1;

        if (startingElement != null)
        {
            currentElement = startingElement;
            MoveSelection(currentElement.transform);
        }

        if ( fightTab.TryGetComponent<AttackHandler>(out AttackHandler attack))
        {
            attackHandler = attack;
        }
        else
        {
            Debug.LogError("fight tab is missing a fight handler reference in " + gameObject.name);
        }

        if (actTab.TryGetComponent<ActHandler>(out ActHandler act))
        {
            actHandler = act;
        }
        else
        {
            Debug.LogError("act tab is missing an act handler reference in " + gameObject.name);
        }

        if (inventoryTab.TryGetComponent<InventoryHandler>(out InventoryHandler inventory))
        {
            inventoryHandler = inventory;
        }
        else
        {
            Debug.LogError("inventory tab is missing an inventory handler reference in " + gameObject.name);
        }

        c = new PlayerInputs();

        c.Menu.Left.performed += ctx => MoveLeft();
        c.Menu.Right.performed += ctx => MoveRight();
        c.Menu.Up.performed += ctx => MoveUp();
        c.Menu.Down.performed += ctx => MoveDown();
        c.Menu.Enter.performed += ctx => Enter();
        c.Menu.Back.performed += ctx => Back();

        c.Enable();
    }

    
    private void OnEnable()
    {
        if (character != null && attackHandler != null)
        {
            attackHandler.SetCharacter(character);
        }
        
        if (character != null && actHandler != null)
        {
            actHandler.SetCharacter(character);
        }

        if (character != null && character.AttacksFirst)
        {
            StartCharacterAttack();
        }
    }

    private void OnDisable()
    {
        
    }

    private void Update()
    {
        if (inFight || inAct || inInventory)
        {
            inTab = true;
        }
        else
        {
            inTab = false;
        }

        if (moveHealth)
        {
            if (healthbar != null)
            {
                healthbarHolder.transform.position = Vector2.Lerp(healthbar.transform.position, healthWaypoint.position, t3);

                if (t3 < 1)
                {
                    t3 += Time.deltaTime / 2;
                }
                else
                {
                    healthbarHolder.transform.position = healthWaypoint.position;

                    moveHealth = false;
                }
            }
        }

        healthText.text = Mathf.Ceil(playerHealth.GetCurrentHealth()).ToString();
        healthbar.fillAmount = Mathf.Lerp(currentDisplayedHealth / playerHealth.GetMaxHealth(), playerHealth.GetCurrentHealth() / playerHealth.GetMaxHealth(), t);

        if (!playerHealth.GetTakingDamage())
        {
            healthloss.fillAmount = Mathf.Lerp(currentDisplayedLoss / playerHealth.GetMaxHealth(), playerHealth.GetCurrentHealth() / playerHealth.GetMaxHealth(), t2);

            if (t2 < 1)
            {
                t2 += Time.deltaTime;
            }
            else
            {
                currentDisplayedLoss = currentDisplayedHealth;
                t2 = 0.0f;
            }
        }
        else
        {
            t2 = 0.0f;
        }
        
        if (t < 1)
        {
            t += Time.deltaTime;
        }
        else
        {
            currentDisplayedHealth = playerHealth.GetCurrentHealth();
            t = 0.0f;
        }

        if (playerHealth.GetCurrentHealth() > currentDisplayedHealth)
        {
            currentDisplayedHealth = playerHealth.GetCurrentHealth();
        }
        if (playerHealth.GetCurrentHealth() > currentDisplayedLoss)
        {
            currentDisplayedLoss = playerHealth.GetCurrentHealth();
        }
    }

    #region movements
    private void MoveSelection(Transform moveLocation)
    {
        if (moveLocation != null)
        {
            selectionIcon.transform.position = moveLocation.position;
            tabSelected = true;
        }
    }

    public void SetSelected(GameObject select)
    {
        if (select != null && !inInventory && !inAct)
        {
            currentElement = select;

            MoveSelection(currentElement.transform);
        }
    }
    public void SetSelectedPointer(GameObject select)
    {
        currentElement = select;

        MoveSelection(currentElement.transform);
    }

    private void Enter()
    {
        if (currentElement.TryGetComponent<Button>(out Button btn) && !inTab && tabSelected)
        {
            btn.onClick.Invoke();
        }
        else if (inInventory && inventoryHandler != null && !tabSelected)
        {
            inventoryHandler.Enter();
        }
        else if (inAct && actHandler != null && !tabSelected)
        {
            actHandler.Enter();
        }
    }

    private void Back()
    {
        if (inInventory)
        {
            if (inventoryTab != null)
            {
                inventoryTab.SetActive(false);
            }

            inInventory = false;

            SetSelected(btn_items.gameObject);
        }
        if (inAct)
        {
            if (actTab != null)
            {
                actTab.SetActive(false);
            }

            inAct = false;

            SetSelected(btn_act.gameObject);
        }
    }

    private void MoveLeft()
    {
        if (characterAttacking)
        {
            
        }
        else
        {
            if (currentElement != null && !inTab)
            {
                if (currentElement.TryGetComponent<MenuButtons>(out MenuButtons mb))
                {
                    if (mb.GetLeft() != null)
                    {
                        SetSelected(mb.GetLeft());
                    }
                }
            }
            else if (inInventory && inventoryHandler != null)
            {
                inventoryHandler.MoveLeft();
            }
            else if (inAct && actHandler != null)
            {
                actHandler.MoveLeft();
            }
        }
    }

    private void MoveRight()
    {
        if (currentElement != null && !inTab)
        {
            if (currentElement.TryGetComponent<MenuButtons>(out MenuButtons mb))
            {
                if (mb.GetRight() != null)
                {
                    SetSelected(mb.GetRight());
                }
            }
        }
        else if (inInventory && inventoryHandler != null)
        {
            inventoryHandler.MoveRight();
        }
        else if (inAct && actHandler != null)
        {
            actHandler.MoveRight();
        }
    }

    private void MoveUp()
    {
        if (currentElement != null && !inTab)
        {
            if (currentElement.TryGetComponent<MenuButtons>(out MenuButtons mb))
            {
                if (mb.GetUp() != null)
                {
                    SetSelected(mb.GetUp());
                }
            }
        }
        else if (inInventory && inventoryHandler != null)
        {
            inventoryHandler.MoveUp();
        }
        else if (inAct && actHandler != null)
        {
            actHandler.MoveUp();
        }
    }

    private void MoveDown()
    {
        if (currentElement != null && !inTab)
        {
            if (currentElement.TryGetComponent<MenuButtons>(out MenuButtons mb))
            {
                if (mb.GetDown() != null)
                {
                    SetSelected(mb.GetDown());
                }
            }
        }
        else if (inInventory && inventoryHandler != null)
        {
            inventoryHandler.MoveDown();
        }
        else if (inAct && actHandler != null)
        {
            actHandler.MoveDown();
        }
    }
    #endregion

    #region Tabs
    public void SetInFight(bool newBool)
    {
        inFight = newBool;

        if (inFight)
        {
            inTab = true;
            inAct = false;
            inInventory = false;
        }
    }

    public void SetInAct(bool newBool)
    {
        inAct = newBool;

        if (inAct)
        {
            inTab = true;
            inFight = false;
            inInventory = false;
        }
    }

    public void SetInInventory(bool newBool)
    {
        inInventory = newBool;
        
        if (inInventory)
        {
            inTab = true;
            inFight = false;
            inAct = false;
        }
    }

    public void SetTabSelected(bool newBool)
    {
        tabSelected = newBool;
    }
    #endregion

    public void SetCharacter(BattleCharacter newCharacter)
    {
        if (newCharacter != null)
        {
            character = newCharacter;

            if (actHandler != null)
            {
                actHandler.SetCharacter(character);
            }
        }
    }

    public void SetBattleHandler(BattleHandler bh)
    {
        battleHandler = bh;
        battleHandler.SetUiHandler(this);
    }

    public void AttemptRun()
    {
        if (character != null && character.CurrentSatisfaction >= character.MaxSatisfaction)
        {
            if (collectionText != null)
            {
                collectionText.text = "Got Away Safely";

                character.ResetSatisfaction();
            }
        }
        else if (character != null && character.CurrentSatisfaction < character.MaxSatisfaction)
        {
            if (collectionText != null)
            {

            }
        }
    }

    public void StartCharacterAttack()
    {
        if (battleHandler != null)
        {
            battleHandler.StartAttack(character.GetCurrentAttack());
        }

        characterAttacking = true;

        if (selectionZone != null)
        {
            selectionZone.SetActive(false);
        }

        if (battleBackground != null)
        {
            battleBackground.gameObject.SetActive(true);
        }

        if (fightWaypoint != null)
        {
            moveHealth = true;
            healthWaypoint = fightWaypoint;
        }
    }

    public void EndCharacterAttack()
    {
        characterAttacking = false;

        if (selectionZone != null)
        {
            selectionZone.SetActive(true);
        }

        if (battleBackground != null)
        {
            battleBackground.gameObject.SetActive(false);
        }

        if (selectionWaypoint != null)
        {
            moveHealth = true;
            healthWaypoint = selectionWaypoint;
        }

        if (character != null)
        {
            character.GetNextAttack();
        }
    }
}
