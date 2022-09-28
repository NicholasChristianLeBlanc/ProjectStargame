using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Inventory/InventoryItem")]
public class InventoryItem : ScriptableObject
{
    [SerializeField] string itemName;

    [Header("Healing Variables")]
    [SerializeField] [Range(0.0f, 100.0f)] float healAmount = 5.0f;
    [SerializeField] bool isHealingItem = false;

    [Header("Weapon Variables")]
    [SerializeField] [Range(0.0f, 100.0f)] float weaponDamage = 5.0f;
    [SerializeField] bool isWeapon = false;

    private void OnValidate()
    {
        if (isHealingItem)
        {
            isWeapon = false;
        }
        
        if (isWeapon)
        {
            isHealingItem = false;
        }
    }

    public string GetName()
    {
        return itemName;
    }

    public float GetHealAmount()
    {
        return healAmount;
    }

    public bool GetIsHealingItem()
    {
        return isHealingItem;
    }

    public float GetWeaponDamage()
    {
        return weaponDamage;
    }

    public bool GetIsWeapon()
    {
        return isWeapon;
    }
}
