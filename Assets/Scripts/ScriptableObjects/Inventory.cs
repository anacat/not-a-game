using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Inventory")]
public class Inventory : ScriptableObject
{
    [SerializeField]
    private List<InventoryItem> _listOfItems;

    public List<InventoryItem> GetListOfItems()
    {
        return _listOfItems;
    }

    public void AddItem(InventoryItem item)
    {
        _listOfItems.Add(item);
    }

    public void RemoveItem(InventoryItem item)
    {
        _listOfItems.Remove(item);
    }

    public void ClearInventory()
    {
        _listOfItems.Clear();
    }
}
