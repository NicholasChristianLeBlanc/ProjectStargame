using UnityEngine;
using UnityEngine.InputSystem; // Uses Unity's new input system.
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof (Movement))]
[RequireComponent(typeof (Health))]
[RequireComponent(typeof (InteractionInstigator))]
[RequireComponent(typeof (DialogueInstigator))]
[RequireComponent(typeof (FlowListener))]

public class PlayerManager : MonoBehaviour
{
    Movement moveManager;
    PlayerInputs c;

    [SerializeField] Camera mainCam;

    [Header("UI")]
    [SerializeField] private UIDialogueTextBoxController textController;
    [SerializeField] [Range(0.0f, 2.5f)] private float dialogueCounter = 0.1f;
    InteractionInstigator interaction;
    bool inDialogue = false;
    float d_counter;

    [Header("Battle")]
    [SerializeField] GameObject battlePrefab;
    [SerializeField] BattleUIHandler battleUIHandler;
    [SerializeField] GameObject battleUi;
    bool inBattle = false;
    GameObject battle;
    BattleHandler battleHandler;

    [Header("Inventory")]
    [SerializeField] InventoryItem currentWeapon;

    // Movement Variables
    bool movementEnabled = true;
    float horizontalMove = 0;
    float verticalMove = 0;

    // Start is called before the first frame update
    void Awake()
    {
        moveManager = GetComponent<Movement>();

        interaction = GetComponent<InteractionInstigator>();

        if (battlePrefab != null && battlePrefab.TryGetComponent<BattleHandler>(out BattleHandler bh))
        {
            battleHandler = bh;
        }

        c = new PlayerInputs();

        c.Player.Horizontal.performed += RecieveHorizontalMovement;
        c.Player.Horizontal.canceled += CancelHorizontalMovement;
        c.Player.Vertical.performed += RecieveVerticalMovement;
        c.Player.Vertical.canceled += CancelVerticalMovement;
        c.Player.Interact.performed += ctx => RecieveInteract();
        c.Player.Interact.canceled += ctx => CancelInteract();

        c.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (movementEnabled && !inBattle)
        {
            moveManager.MoveHorizontal(horizontalMove);
            moveManager.MoveVertical(verticalMove);
        }
        else if (inBattle)
        {
            if (battleHandler != null)
            {
                battleHandler.MoveHorizontal(horizontalMove);
                battleHandler.MoveVertical(verticalMove);

                //Debug.Log("Horizontal: " + horizontalMove + " Vertical: " + verticalMove);
            }
        }

        if (d_counter > 0)
        {
            d_counter -= Time.deltaTime;
        }
    }

    void RecieveHorizontalMovement(InputAction.CallbackContext input)
    {
        if (input.ReadValue<float>() != 0)
        {
            horizontalMove = input.ReadValue<float>();
        }
        else
        {
            horizontalMove = 0;
        }
    }
    void CancelHorizontalMovement(InputAction.CallbackContext ctx)
    {
        horizontalMove = 0;
    }

    void RecieveVerticalMovement(InputAction.CallbackContext input)
    {
        if (input.ReadValue<float>() != 0)
        {
            if (input.ReadValue<float>() > 1)
            {
                verticalMove = 1;
            }
            else if (input.ReadValue<float>() < -1)
            {
                verticalMove = -1;
            }
            else
            {
                verticalMove = input.ReadValue<float>();
            }
        }
        else
        {
            verticalMove = 0;
        }
    }
    void CancelVerticalMovement(InputAction.CallbackContext ctx)
    {
        verticalMove = 0;
    }

    void RecieveInteract()
    {
        if (!inDialogue)
        {
            interaction.SetInteracted(true);
        }
        else
        {
            if (d_counter <= 0)
            {
                textController.SetInteract(true);
                d_counter = dialogueCounter;
            }
        }
    }
    void CancelInteract()
    {
        if (!inDialogue)
        {
            interaction.SetInteracted(false);
        }
        else
        {
            textController.SetInteract(false);
        }
    }

    public void SetInDialogue(bool newBool)
    {
        inDialogue = newBool;

        if (inDialogue)
        {
            SetMovementEnabled(false);
            d_counter = dialogueCounter;

            horizontalMove = 0;
            verticalMove = 0;

            moveManager.MoveHorizontal(horizontalMove);
            moveManager.MoveVertical(verticalMove);
        }
    }

    public void StartBattle(BattleCharacter battleCharacter)
    {
        if (battleUi != null)
        {
            if (battlePrefab != null)
            {
                battle = Instantiate(battlePrefab, Vector3.zero, Quaternion.identity);

                if (battle != null && battle.TryGetComponent<BattleHandler>(out BattleHandler bh))
                {
                    battleHandler = bh;
                }
            }

            if (battleUIHandler != null)
            {
                battleUIHandler.SetCharacter(battleCharacter);

                if (battleHandler != null)
                {
                    battleUIHandler.SetBattleHandler(battleHandler);
                    battleHandler.SetUiHandler(battleUIHandler);
                }
            }
            
            battleUi.SetActive(true);

            SetInBattle(true);
        }

        if (mainCam != null && mainCam.gameObject.TryGetComponent<AudioListener>(out AudioListener listener))
        {
            listener.enabled = false;
        }

        if (gameObject.TryGetComponent<BoxCollider2D>(out BoxCollider2D bc2d))
        {
            bc2d.enabled = false;
        }
    }

    public void EndBattle()
    {
        if (battleUi != null)
        {
            if (battle != null)
            {
                Destroy(battle);

                battleHandler = null;
            }

            battleUi.SetActive(false);

            SetInBattle(false);
        }

        if (mainCam != null && mainCam.gameObject.TryGetComponent<AudioListener>(out AudioListener listener))
        {
            listener.enabled = true;
        }

        if (gameObject.TryGetComponent<BoxCollider2D>(out BoxCollider2D bc2d))
        {
            bc2d.enabled = true;
        }
    }

    private void SetInBattle(bool newBool)
    {
        inBattle = newBool;

        if (inBattle)
        {
            SetMovementEnabled(false);
            SetInDialogue(false);

            horizontalMove = 0;
            verticalMove = 0;

            moveManager.MoveHorizontal(horizontalMove);
            moveManager.MoveVertical(verticalMove);
        }
    }

    public void SetWeapon(InventoryItem newWeapon)
    {
        if (newWeapon.GetIsWeapon())
        {
            currentWeapon = newWeapon;
        }
    }
    public InventoryItem GetWeapon()
    {
        return currentWeapon;
    }

    public void SetMovementEnabled(bool newBool)
    {
        movementEnabled = newBool;
    }
}
