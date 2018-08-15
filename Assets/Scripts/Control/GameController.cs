using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController player;
    public DoorController doorController;

    [Header("Variables")]
    public Inventory playerInventory;
    public BoolVariable hasShowInitialStory;
    public BoolVariable canPlayerMove;
    public BoolVariable canPlayerInteract;
    public IntVariable numberOfFinalItems;

    //Resets player stats when game ends
    private void OnApplicationQuit()
    {
        playerInventory.ClearInventory();
        hasShowInitialStory.SetValue(false);
        canPlayerMove.SetValue(true);
        canPlayerInteract.SetValue(true);
        numberOfFinalItems.SetValue(0);
    }
}
