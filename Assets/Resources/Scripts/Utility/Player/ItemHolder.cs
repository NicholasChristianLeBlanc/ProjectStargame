using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] InventoryItem itemReference;

    public InventoryItem GetItem()
    {
        return itemReference;
    }

    public void SetItemReference(InventoryItem newItem)
    {
        itemReference = newItem;
    }
}
