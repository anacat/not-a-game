using System.Collections;
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

        foreach (Transform t in inventoryGrid.transform)
        {
            Destroy(t.gameObject);
        }

        foreach (InventoryItem i in inventory.GetListOfItems())
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
