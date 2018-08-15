using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemTradeController : MonoBehaviour
{
    public delegate void RecieveItemAction(InventoryItem item, GameObject itemObject);
    public event RecieveItemAction RecieveItem;

    public GameController controller;
    public BoolVariable canPlayerMove;
    public BoolVariable canPlayerInteract;

    [Header("Inventory")]
    public Inventory playerInventory;
    public InventorySystem inventorySystem;

    [Header("Puzzle Items")]
    public List<InventoryItem> neededItems;
    public List<InventoryItem> returnedItems;

    [Header("UI")]
    public CanvasGroup speechBubble;

    private PlayerController _playerController;
    private Animator _ghostAnimator;
    private bool _isGivingItem;    
    private bool _isRecievingItem;
    private bool _isCoroutineRunning;

    private List<Image> _speechBubbleImages = new List<Image>();

    private void Awake()
    {
        _playerController = controller.player;
        _ghostAnimator = GetComponent<Animator>();

        speechBubble.alpha = 0;
    }

    private void Start()
    {
        PopulateSpeechBubble();
    }

    private void PopulateSpeechBubble()
    {
        _speechBubbleImages.AddRange(speechBubble.GetComponentsInChildren<Image>());

        //Starts with 1 to ignore the dialogue bubble image
        for(int i = 1; i <= neededItems.Count; i++)
        {
            _speechBubbleImages[i].sprite = neededItems[i - 1].GetItemSprite();
        }
    }

    public void SpeechBubbleDistanceCheck()
    {
        if (speechBubble != null)
        {
            if (_ghostAnimator.GetBool("speechbubble") && Vector2.Distance(_playerController.transform.position, transform.position) > 2)
            {
                _ghostAnimator.SetBool("speechbubble", false);
                canPlayerInteract.SetValue(false);
            }
            else if (!_isCoroutineRunning && !_ghostAnimator.GetBool("speechbubble") && Vector2.Distance(_playerController.transform.position, transform.position) < 2)
            {
                StartCoroutine(SpeechBubbleAnimation());
            }
        }
    }

    private IEnumerator SpeechBubbleAnimation()
    {
        _isCoroutineRunning = true;

        _playerController.GetSpriteRenderer().flipX = _playerController.transform.position.x > transform.position.x;
        _playerController.GetAnimator().SetTrigger("npcInteract");

        yield return AnimationHelper.WaitForAnimation(_playerController.GetAnimator());

        _ghostAnimator.SetBool("speechbubble", true);
        canPlayerInteract.SetValue(true);

        _isCoroutineRunning = false;
    }

    public bool RecievedItem(InventoryItem item, GameObject itemObject)
    {
        if (neededItems.Contains(item) && RecieveItem != null && !_isRecievingItem)
        {
            RecieveItem(item, itemObject);

            return true;
        }

        return false;
    }

    public IEnumerator RecieveItemAnimation(InventoryItem item, GameObject itemObject, Action action)
    {
        yield return RecieveItemAnimation(item, itemObject);

        action();
    }

    public IEnumerator RecieveItemAnimation(InventoryItem item, GameObject itemObject)
    {
        _isRecievingItem = true;
        canPlayerMove.SetValue(false);

        _playerController.GetAnimator().SetTrigger("objectInteract");

        yield return AnimationHelper.WaitForAnimation(_playerController.GetAnimator());

        _ghostAnimator.SetTrigger("interact");

        yield return AnimationHelper.WaitForAnimation(_ghostAnimator);

        playerInventory.RemoveItem(item);
        Destroy(itemObject.gameObject);

        inventorySystem.PopulateGrid();

        canPlayerMove.SetValue(true);
        _isRecievingItem = false;
    }

    public bool DidntGiveItems()
    {
        return returnedItems != null && returnedItems.Count != 0 && !_isGivingItem;
    }

    public IEnumerator GiveItems()
    {
        canPlayerMove.SetValue(false);

        _isGivingItem = true;

        _ghostAnimator.SetTrigger("interact");

        yield return AnimationHelper.WaitForAnimation(_ghostAnimator);

        _playerController.GetAnimator().SetTrigger("objectInteract");

        yield return AnimationHelper.WaitForAnimation(_playerController.GetAnimator());

        playerInventory.GetListOfItems().AddRange(returnedItems);
        inventorySystem.PopulateGrid();

        returnedItems.Clear();

        _isGivingItem = false;

        canPlayerMove.SetValue(true);
    }

    public void DestroySpeechItem(int index)
    {
        Destroy(_speechBubbleImages[index + 1].gameObject);
    }

    public void DestroySpeechBubble()
    {
        if (speechBubble != null)
        {
            Destroy(speechBubble.gameObject);
        }
    }

    public PlayerController GetPlayer()
    {
        return _playerController;
    }
}
