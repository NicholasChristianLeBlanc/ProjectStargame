using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryHandler : MonoBehaviour
{
    [SerializeField] BattleUIHandler battleHandler;

    [Header("Selection References")]
    [SerializeField] GameObject startingElement;
    [SerializeField] GameObject selectionIcon;
    [SerializeField] float selectionOffsetH = 0.0f;
    [SerializeField] float selectionOffsetV = 0.0f;
    
    [Header("YesNo Screen References")]
    [SerializeField] GameObject yesNoScreen;
    [SerializeField] TextMeshProUGUI yesNoUseItemText;
    [SerializeField] GameObject yesButton;

    [Header("Item References")]
    [SerializeField] TextMeshProUGUI[] itemTexts = new TextMeshProUGUI[8];
    [SerializeField] Button[] itemButtons = new Button[8];
    [SerializeField] Button btn_Back;

    private GameObject currentElement;
    private PlayerInventory inventory;

    private InventoryItem currentItem;

    private void Awake()
    {
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent<PlayerInventory>(out PlayerInventory inv))
        {
            inventory = inv;
        }

        if (startingElement != null)
        {
            currentElement = startingElement;
        }
        else
        {
            startingElement = itemButtons[0].gameObject;
            currentElement = startingElement;
        }
    }

    private void OnEnable()
    {
        if (inventory != null)
        {
            int index = 0;

            foreach (InventoryItem i in inventory.GetInventoryItems())
            {
                if (i != null)
                {
                    itemTexts[index].text = i.GetName();

                    if (itemButtons[index].TryGetComponent<ItemHolder>(out ItemHolder holder))
                    {
                        holder.SetItemReference(i);
                    }
                }
                else
                {
                    itemTexts[index].text = "Empty";

                    if (itemButtons[index].TryGetComponent<ItemHolder>(out ItemHolder holder))
                    {
                        holder.SetItemReference(null);
                    }
                }

                index++;

                if (index >= itemTexts.Length)
                {
                    index = itemTexts.Length - 1;
                }
            }
        }
    }

    private void OnDisable()
    {
        if (yesNoScreen != null)
        {
            yesNoScreen.SetActive(false);
        }
    }

    public void Enter()
    {
        if (currentElement.TryGetComponent<Button>(out Button btn))
        {
            btn.onClick.Invoke();
        }
    }

    private void MoveSelection(Transform moveLocation)
    {
        if (moveLocation != null)
        {
            Vector2 pos = moveLocation.position;
            pos.x += selectionOffsetH;
            pos.y += selectionOffsetV;

            selectionIcon.transform.position = pos;

            if (battleHandler != null)
            {
                battleHandler.SetTabSelected(false);
            }
        }
    }

    public void SetSelected(GameObject select)
    {
        currentElement = select;

        MoveSelection(currentElement.transform);
    }

    public void MoveLeft()
    {
        if (currentElement != null)
        {
            if (currentElement.TryGetComponent<MenuButtons>(out MenuButtons mb))
            {
                if (mb.GetLeft() != null)
                {
                    SetSelected(mb.GetLeft());
                }
            }
        }
    }

    public void MoveRight()
    {
        if (currentElement != null)
        {
            if (currentElement.TryGetComponent<MenuButtons>(out MenuButtons mb))
            {
                if (mb.GetRight() != null)
                {
                    SetSelected(mb.GetRight());
                }
            }
        }
    }

    public void MoveUp()
    {
        if (currentElement != null)
        {
            if (currentElement.TryGetComponent<MenuButtons>(out MenuButtons mb))
            {
                if (mb.GetUp() != null)
                {
                    SetSelected(mb.GetUp());
                }
            }
        }
    }

    public void MoveDown()
    {
        if (currentElement != null)
        {
            if (currentElement.TryGetComponent<MenuButtons>(out MenuButtons mb))
            {
                if (mb.GetDown() != null)
                {
                    SetSelected(mb.GetDown());
                }
            }
        }
    }

    public void OpenYesNo(InventoryItem itemToUse)
    {
        if (itemToUse != null)
        {
            currentItem = itemToUse;

            if (yesNoUseItemText != null)
            {
                if (itemToUse.GetIsHealingItem())
                {
                    yesNoUseItemText.text = ("Are you sure you want to use " + itemToUse.GetName() + " to heal " + itemToUse.GetHealAmount() + " health?");
                }
                else if (itemToUse.GetIsWeapon())
                {
                    yesNoUseItemText.text = ("Are you sure you want to equip " + itemToUse.GetName() + "?");
                }
            }

            if (yesNoScreen != null)
            {
                yesNoScreen.SetActive(true);

                if (yesButton != null)
                {
                    SetSelected(yesButton);
                }
            }
        }
    }
    public void OpenYesNo(ItemHolder holder)
    {
        if (holder.GetItem() != null)
        {
            currentItem = holder.GetItem();

            if (yesNoUseItemText != null)
            {
                InventoryItem itemToUse = holder.GetItem();

                if (itemToUse.GetIsHealingItem())
                {
                    yesNoUseItemText.text = ("Are you sure you want to use " + itemToUse.GetName() + " to heal " + itemToUse.GetHealAmount() + " health?");
                }
                else if (itemToUse.GetIsWeapon())
                {
                    yesNoUseItemText.text = ("Are you sure you want to equip " + itemToUse.GetName() + "?");
                }
            }

            if (yesNoScreen != null)
            {
                yesNoScreen.SetActive(true);

                if (yesButton != null)
                {
                    SetSelected(yesButton);
                }
            }
        }
    }

    public void UseItem()
    {
        inventory.UseItem(currentItem);
    }
    public void UseItem(int index)
    {
        inventory.UseItem(index);
    }
    
}
