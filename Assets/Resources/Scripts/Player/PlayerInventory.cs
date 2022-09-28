using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] PlayerManager player;

    [SerializeField] List<InventoryItem> weapons = new List<InventoryItem>();
    [SerializeField] List<InventoryItem> items = new List<InventoryItem>();

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player").TryGetComponent<PlayerManager>(out PlayerManager pm)) {
            player = pm;
        }
    }

    public void UseItem(int index)
    {

    }
    public void UseItem(InventoryItem useItem)
    {
        if (useItem.GetIsWeapon())
        {
            if (player.GetWeapon() != useItem)
            {
                if (player.GetWeapon() != null)
                {
                    AddItem(player.GetWeapon(), false);
                }
                
                player.SetWeapon(useItem);
                RemoveItem(useItem);
            }
            else
            {
                Debug.LogError("Attempted equipping a weapon that the player already had equipped");
            }
        }
        else if (useItem.GetIsHealingItem())
        {
            if (player.gameObject.TryGetComponent<Health>(out Health h))
            {
                h.AddHealth(useItem.GetHealAmount());
                RemoveItem(useItem);
            }
        }
    }

    public void AddItem(InventoryItem newItem)
    {
        if (newItem != null)
        {
            if (newItem.GetIsHealingItem())
            {
                items.Add(newItem);
            }
            else if (newItem.GetIsWeapon())
            {
                bool duplicate = false;

                foreach (InventoryItem weapon in weapons)
                {
                    if (weapon == newItem)
                    {
                        duplicate = true;
                    }
                }

                if (!duplicate)
                {
                    weapons.Add(newItem);
                }
            }
        }
    }
    public void AddItem(InventoryItem newItem, bool checkDuplicate)
    {
        if (newItem != null)
        {
            if (newItem.GetIsHealingItem())
            {
                items.Add(newItem);
            }
            else if (newItem.GetIsWeapon())
            {
                if (checkDuplicate)
                {
                    bool duplicate = false;

                    foreach (InventoryItem weapon in weapons)
                    {
                        if (weapon == newItem || weapon == player.GetWeapon())
                        {
                            duplicate = true;
                        }
                    }

                    if (!duplicate)
                    {
                        weapons.Add(newItem);
                    }
                }
                else
                {
                    weapons.Add(newItem);
                }
            }
        }
    }

    public void RemoveItem(InventoryItem remove)
    {
        if (remove.GetIsHealingItem() && items.Contains(remove))
        {
            int index = 0;

            foreach(InventoryItem i in items)
            {
                if (i.GetName() == remove.GetName())
                {
                    break;
                }
                else
                {
                    index++;
                }
            }

            items.RemoveAt(index);
            items.Add(null);
        }
        else if (remove.GetIsWeapon() && weapons.Contains(remove))
        {
            int index = 0;

            foreach (InventoryItem i in weapons)
            {
                if (i.GetName() == remove.GetName())
                {
                    break;
                }
                else
                {
                    index++;
                }
            }

            weapons.RemoveAt(index);
        }
    }

    public List<InventoryItem> GetInventoryItems()
    {
        return items;
    }

    public List<InventoryItem> GetInventoryWeapons()
    {
        return weapons;
    }
}
