using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public AnimationCurve movementCurve;
    public BoolVariable canPlayerInteract;

    private InventoryItem _item;
    private Image _image;
    private Vector2 _initialPosition;

    public void SetObject(InventoryItem item)
    {
        _item = item;
        _image = GetComponent<Image>();

        _image.sprite = item.GetItemSprite();

        StartCoroutine(GetInitialPosition());
    }

    private IEnumerator GetInitialPosition()
    {
        yield return new WaitForEndOfFrame();

        _initialPosition = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        _image.raycastTarget = false;

        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Vector2 origin = Camera.main.ScreenToWorldPoint(eventData.position);
        RaycastHit2D[] hit = Physics2D.RaycastAll(origin, Vector2.zero);

        if (hit.Length != 0)
        {
            foreach (RaycastHit2D r in hit)
            {
                if ((r.collider.CompareTag("Ghost") || r.collider.CompareTag("CoupleGhost")) && !IsGhostAndWantsThisItem(r))
                {
                    StartCoroutine(GotoInitialPosition());
                }
            }
        }
    }

    private bool IsGhostAndWantsThisItem(RaycastHit2D hit)
    {
        if (canPlayerInteract.GetValue())
        {
            var itemController = hit.collider.GetComponentInChildren<ItemTradeController>(true);

            if (itemController != null)
            {
                return itemController.RecievedItem(_item, gameObject);
            }
        }

        return false;
    }

    private IEnumerator GotoInitialPosition()
    {
        float interpolationTime = 0.5f;
        float timeStarted;
        float deltaTime = 0;
        float percentageDone = 0;
        Vector2 currentPosition = transform.position;

        timeStarted = Time.time;

        while (percentageDone < 1f)
        {
            deltaTime = Time.time - timeStarted;
            percentageDone = deltaTime / interpolationTime;

            transform.position = Vector2.Lerp(currentPosition, _initialPosition, movementCurve.Evaluate(percentageDone));

            yield return null;
        }

        _image.raycastTarget = true;
    }
}
