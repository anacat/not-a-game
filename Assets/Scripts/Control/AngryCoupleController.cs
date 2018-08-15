using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ItemTradeController))]
public class AngryCoupleController : MonoBehaviour
{
    public BoolVariable hasShownInitialScene;
    public BoolVariable canPlayerMove;
    public Animator storyTimeAnimator;

    private ItemTradeController _itemController;
    private bool _gaveItems;
    private bool _isRecievingItem;

    private void Awake()
    {
        _itemController = GetComponent<ItemTradeController>();
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

        yield return _itemController.RecieveItemAnimation(item, itemObject, () => Debug.Log("Congrations"));

        _itemController.neededItems.Clear();

        _isRecievingItem = false;

        //TODO: game end
    }

    private void OnDisable()
    {
        _itemController.RecieveItem -= RecieveItemAction;
    }
}
