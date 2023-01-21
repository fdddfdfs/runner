using StarterAssets;

public class PlayerActivateState : PlayerState
{
    protected readonly ActiveItemsUI _activeItemsUI;
    
    public PlayerActivateState(ThirdPersonController player, ActiveItemsUI activeItemsUI) : base(player)
    {
        _activeItemsUI = activeItemsUI;
    }
}