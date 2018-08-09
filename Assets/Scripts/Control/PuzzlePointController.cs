//Controls the puzzle momments in the game (where the player has to trade objects to gain other objects)
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PuzzlePointController : MonoBehaviour
{
    public Inventory playerInventory;
    public InventorySystem invertorySystem;

    [Header("UI")]
    public GameObject speechBubble;
    public Image item1;
    public Image item2; 

    [Header("PuzzleItems")]
    public List<InventoryItem> neededItems;
    public List<InventoryItem> returnedItems;

    private int _missingItems = 2;

    public void Awake() 
    {
        item1.sprite = neededItems[0].GetItemSprite();
        item2.sprite = neededItems[1].GetItemSprite();
    }

    public void GiveItems()
    {
        Debug.Log("Trading");
        Destroy(speechBubble);

        playerInventory.GetListOfItems().AddRange(returnedItems);
        invertorySystem.PopulateGrid();
    }

    public bool RecieveItem(InventoryItem item, GameObject itemObject)
    {
        Debug.Log("item " + item.GetItemName());

        if(neededItems.Contains(item))
        {
            _missingItems--;

            playerInventory.RemoveItem(item);
            Destroy(itemObject);

            int n = neededItems.IndexOf(item);
            if(n == 0)
            {
                Destroy(item1.gameObject);
            }
            else if( n == 1)
            {
                Destroy(item2.gameObject);
            }

            if(_missingItems == 0)
            {
                GiveItems();
            }

            return true;           
        }
        
        return false;
    }
}