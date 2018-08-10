using System.Collections;
using UnityEngine;

public class AngryCoupleController : MonoBehaviour
{
    public BoolVariable hasShownInitialScene;
    public Inventory playerInventory;
    public InventorySystem inventorySystem;
    public BoolVariable canPlayerMove;
    public GameController controller;
    public Animator storyTimeAnimator;

    [Header("Items")]
    public InventoryItem neededItem;
    public InventoryItem returnedItem;

    private PlayerController _playerController;
    private Animator _ghostAnimator;

    private void Awake()
    {
        _playerController = controller.player;
        _ghostAnimator = GetComponent<Animator>();
    }

    private IEnumerator Move()
    {
        yield return _playerController.MovePlayer(transform.localPosition, 0.8f);

        if (!hasShownInitialScene.GetValue())
        {
            yield return RunStoryTime();
        }

        if (returnedItem != null)
        {
            _ghostAnimator.SetTrigger("interact");

            yield return new WaitForSeconds(0.5f);

            GivePlayerItem();
        }
    }

    public void StartStoryTime()
    {
        StartCoroutine(Move());
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

    private void GivePlayerItem()
    {
        playerInventory.AddItem(returnedItem);
        inventorySystem.PopulateGrid();
        returnedItem = null;
    }

    public bool RecieveItem(InventoryItem item, GameObject itemGameObject)
    {
        if (neededItem == item)
        {
            StartCoroutine(RecieveItemAnimation(item, itemGameObject));

            return true;
        }

        return false;
    }

    private IEnumerator RecieveItemAnimation(InventoryItem item, GameObject itemGameObject)
    {
        _ghostAnimator.SetTrigger("interact");

        yield return new WaitForSeconds(0.5f);

        playerInventory.RemoveItem(item);
        Destroy(itemGameObject);
        inventorySystem.PopulateGrid();

        Debug.Log("Congrations");

        //TODO: game end
    }
}
