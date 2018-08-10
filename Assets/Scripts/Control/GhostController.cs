//Controls the puzzle momments in the game (where the player has to trade objects to gain other objects)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostController : MonoBehaviour
{
    public Inventory playerInventory;
    public InventorySystem invertorySystem;
    public GameController controller;

    [Header("UI")]
    public CanvasGroup speechBubble;
    public Image item1;
    public Image item2;

    [Header("PuzzleItems")]
    public List<InventoryItem> neededItems;
    public List<InventoryItem> returnedItems;

    private int _missingItems = 2;
    private Animator _animator;
    private PlayerController _playerController;

    private void Awake()
    {
        item1.sprite = neededItems[0].GetItemSprite();
        item2.sprite = neededItems[1].GetItemSprite();

        _animator = GetComponent<Animator>();
        _playerController = controller.player;
    }

    private void Update()
    {
        SpeechBubbleDistanceCheck();
    }

    private void SpeechBubbleDistanceCheck()
    {
        if (speechBubble != null)
        {
            if (_animator.GetBool("speechbubble") && Vector2.Distance(_playerController.transform.position, transform.position) > 2)
            {
                _animator.SetBool("speechbubble", false);
            }
            else if (!_animator.GetBool("speechbubble") && Vector2.Distance(_playerController.transform.position, transform.position) < 2)
            {
                _animator.SetBool("speechbubble", true);
            }
        }
    }

    private IEnumerator GiveItems()
    {
        _animator.SetTrigger("interact");

        yield return new WaitForSeconds(0.5f); //wait for animation

        Destroy(speechBubble.gameObject);

        playerInventory.GetListOfItems().AddRange(returnedItems);
        invertorySystem.PopulateGrid();

        Destroy(this);
    }

    public bool RecieveItem(InventoryItem item, GameObject itemObject)
    {
        if (neededItems.Contains(item))
        {
            StartCoroutine(RecieveItemAnimation(item, itemObject));

            return true;
        }

        return false;
    }

    private IEnumerator RecieveItemAnimation(InventoryItem item, GameObject itemObject)
    {
        _animator.SetTrigger("interact");

        yield return new WaitForSeconds(0.5f);

        _missingItems--;

        playerInventory.RemoveItem(item);
        Destroy(itemObject.gameObject);

        int n = neededItems.IndexOf(item);
        if (n == 0)
        {
            Destroy(item1.gameObject);
        }
        else if (n == 1)
        {
            Destroy(item2.gameObject);
        }

        if (_missingItems == 0)
        {
            StartCoroutine(GiveItems());
        }
    }
}