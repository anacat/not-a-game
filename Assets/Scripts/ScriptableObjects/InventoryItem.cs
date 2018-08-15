using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    [SerializeField]
    private string itemName;

    [SerializeField]
    private Sprite itemSprite;
    private GameObject _gameObject;

    public string GetItemName()
    {
        return itemName;
    }

    public Sprite GetItemSprite()
    {
        return itemSprite;
    }

    public GameObject ItemGameObject
    {
        get
        {
            return _gameObject;
        }
        set
        {
            _gameObject = value;
        }
    }
}
