using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour 
{
	public GameObject inventoryGrid;
	public GameObject itemPrefab;
	public Inventory inventory;

	private void Awake()
	{
		PopulateGrid();
	}

	public void PopulateGrid()
	{
		foreach(InventoryItem i in inventory.listOfItems)
		{
			var obj = Instantiate(itemPrefab);
			obj.transform.SetParent(inventoryGrid.transform, false);
			obj.GetComponent<Image>().sprite = i.GetItemSprite();
		}
	}
}
