using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour 
{
	public GridLayoutGroup inventoryGrid;
	public GameObject itemPrefab;
	public Inventory inventory;

	private void Awake()
	{
		PopulateGrid();
	}

	public void PopulateGrid()
	{
		inventoryGrid.enabled = true;
		
		foreach(InventoryItem i in inventory.GetListOfItems())
		{
			var obj = Instantiate(itemPrefab);
			obj.transform.SetParent(inventoryGrid.transform, false);
			obj.GetComponent<DraggableItem>().SetObject(i);
		}

		StartCoroutine(DisableGrid());
	}

	private IEnumerator DisableGrid()
	{
		yield return new WaitForEndOfFrame();
		inventoryGrid.enabled = false;
	}
}
