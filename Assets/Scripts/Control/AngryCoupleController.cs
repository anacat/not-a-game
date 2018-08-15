using System.Collections;
using DoodleStudio95;
using DoodleStudio95Examples;
using UnityEngine;

[RequireComponent(typeof(ItemTradeController))]
public class AngryCoupleController : MonoBehaviour
{
    public BoolVariable hasShownInitialScene;
    public BoolVariable canPlayerMove;
    public IntVariable numberOfFinalItems;
    public Animator storyTimeAnimator;
    public ChainAnimations storyAnimator;
    public Animator bigHeartAnimator;

    private ItemTradeController _itemController;
    private SpriteRenderer _spriteRenderer;
    private bool _gaveItems;
    private bool _isRecievingItem;

    private void Awake()
    {
        _itemController = GetComponent<ItemTradeController>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        _itemController.RecieveItem += RecieveItemAction;
    }

    public void MoveAndRunStory()
    {
        StartCoroutine(RunActions());
    }

    private void Update()
    {
        if (hasShownInitialScene.GetValue() && _gaveItems && !_isRecievingItem)
        {
            _itemController.SpeechBubbleDistanceCheck();
        }
    }

    private IEnumerator RunActions()
    {
        yield return _itemController.GetPlayer().MovePlayer(transform.localPosition);

        if (!hasShownInitialScene.GetValue())
        {
            yield return RunStoryTime();
        }

        if (_itemController.DidntGiveItems())
        {
            yield return _itemController.GiveItems();
            _gaveItems = true;
        }
    }

    private IEnumerator RunStoryTime()
    {
        canPlayerMove.SetValue(false);

        storyTimeAnimator.SetBool("startStory", true);

        yield return AnimationHelper.WaitForAnimation(storyTimeAnimator);

        storyAnimator.Play();

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));

        storyTimeAnimator.SetBool("startStory", false);

        hasShownInitialScene.SetValue(true);

        canPlayerMove.SetValue(true);
    }

    private void RecieveItemAction(InventoryItem item, GameObject itemObject)
    {
        StartCoroutine(RecieveItemAnimation(item, itemObject));
    }

    private IEnumerator RecieveItemAnimation(InventoryItem item, GameObject itemObject)
    {
        _isRecievingItem = true;

        yield return _itemController.RecieveItemAnimation(item, itemObject);

        _itemController.neededItems.Clear();
        _itemController.DestroySpeechBubble();

        numberOfFinalItems.SetValue(numberOfFinalItems.GetValue() + 1);

        _isRecievingItem = false;

        if(numberOfFinalItems.GetValue() == 2)
        {
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
            bigHeartAnimator.SetTrigger("startStory");

            canPlayerMove.SetValue(false);
            _itemController.canPlayerInteract.SetValue(false);
        }
    }

    private void OnDisable()
    {
        _itemController.RecieveItem -= RecieveItemAction;
    }
}
