using StarterAssets;

public sealed class HighJump : ActivatableItem
{
    public static int Weight => 1;
    
    private const float Multiplayer = 3;
    
    private static float _baseHeight = -1;

    private ThirdPersonController _player;

    public void Init(float baseJumpHeight, ActiveItemsUI activeItemUI, Run run)
    {
        base.Init(activeItemUI, run);

        _baseHeight = baseJumpHeight;
    }

    public override void PickupItem(ThirdPersonController player)
    {
        _player = player;
        
        base.PickupItem(player);
    }

    protected override float ActiveTime => 10;
    protected override ItemType ActiveItemType => ItemType.HighJump;
    protected override void Activate()
    {
        _player.JumpHeight = _baseHeight * Multiplayer;
    }

    protected override void Deactivate()
    {
        _player.JumpHeight = _baseHeight;
    }
}