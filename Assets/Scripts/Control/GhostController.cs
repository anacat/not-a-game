using System.Collections;
using UnityEngine;

[RequireComponent(typeof(ItemTradeController))]
public class GhostController : MonoBehaviour
{
    private ItemTradeController _itemController;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private int _missingItems;
    private bool _isRecievingItem;

    private void Awake()
    {
        _itemController = GetComponent<ItemTradeController>();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _missingItems = _itemController.neededItems.Count;
    }

    private void OnEnable()
    {
        _itemController.RecieveItem += RecieveItemAction;
    }

    private void Update()
    {
        if (!_isRecievingItem)
        {
            _itemController.SpeechBubbleDistanceCheck();
        }
    }

    private void RecieveItemAction(InventoryItem item, GameObject itemObject)
    {
        StartCoroutine(RecieveItemAnimation(item, itemObject));
    }

    private IEnumerator RecieveItemAnimation(InventoryItem item, GameObject itemObject)
    {
        _missingItems--;
        _itemController.canPlayerMove.SetValue(false);
        _isRecievingItem = true;

        yield return _itemController.RecieveItemAnimation(item, itemObject);

        int n = _itemController.neededItems.IndexOf(item);

        _itemController.DestroySpeechItem(n);

        if (_missingItems == 0)
        {
            _animator.SetBool("happy", true);

            yield return _itemController.GiveItems();

            _animator.SetBool("speechbubble", false);

            yield return AnimationHelper.WaitForAnimation(_animator, 1);

            _itemController.neededItems.Clear();
            _itemController.DestroySpeechBubble();

            yield return FadeOut();
        }

        _itemController.canPlayerMove.SetValue(true);
        _isRecievingItem = false;

        //Allows player to move again before destroying the gameObject
        if (_missingItems == 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        _itemController.RecieveItem -= RecieveItemAction;
    }

    private IEnumerator FadeOut()
    {
        float timeStarted;
        float timeToFade = 1f;
        float deltaTime;
        float percentageDone = 0;
        float currentAlpha = _spriteRenderer.color.a;

        timeStarted = Time.time;

        while (percentageDone < 1)
        {
            deltaTime = Time.time - timeStarted;
            percentageDone = deltaTime / timeToFade;

            _spriteRenderer.color = new Vector4(_spriteRenderer.color.r, _spriteRenderer.color.g, _spriteRenderer.color.b, Mathf.Lerp(currentAlpha, 0, percentageDone));

            yield return null;
        }
    }
}