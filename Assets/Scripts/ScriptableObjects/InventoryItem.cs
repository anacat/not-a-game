using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    [SerializeField]
    private string itemName;

    [SerializeField]
    private Sprite itemSprite;

    public string GetItemName()
    {
        return itemName;
    }

    public Sprite GetItemSprite()
    {
        return itemSprite;
    }
}
