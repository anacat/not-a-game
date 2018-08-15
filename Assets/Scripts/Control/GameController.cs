using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController player;
    public Inventory playerInventory;
    public BoolVariable hasShowInitialStory;
    public BoolVariable canPlayerMove;
    public BoolVariable canPlayerInteract;

    //Resets player stats when game begins
    private void Awake()
    {
        playerInventory.ClearInventory();
        hasShowInitialStory.SetValue(false);
        canPlayerMove.SetValue(true);
        canPlayerInteract.SetValue(true);
    }
}
