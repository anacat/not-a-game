using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public AnimationCurve movementCurve;

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
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin,ray.direction, 1000);

        Debug.DrawRay(ray.origin, ray.direction, Color.red);

		if (hit.collider == null || !IsGhostAndWantsThisItem(hit))
		{
            StartCoroutine(GotoInitialPosition());
        }        
    }

    private bool IsGhostAndWantsThisItem(RaycastHit2D hit)
    {
        return hit.collider.CompareTag("Ghost") && hit.transform.gameObject.GetComponent<PuzzlePointController>().RecieveItem(_item, gameObject);
    }

    private IEnumerator GotoInitialPosition()
    {
        float interpolationTime = 0.5f;
		float timeStarted;
		float deltaTime = 0;
		float percentageDone = 0;
		Vector2 currentPosition = transform.position;
		
		timeStarted = Time.time;

		while(percentageDone < 1f)
		{
			deltaTime = Time.time - timeStarted;
			percentageDone = deltaTime / interpolationTime;

			transform.position = Vector2.Lerp(currentPosition, _initialPosition, movementCurve.Evaluate(percentageDone));

			yield return null;
		}
        
        _image.raycastTarget = true;
    }
}
