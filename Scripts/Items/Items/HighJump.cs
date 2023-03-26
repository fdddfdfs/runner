using StarterAssets;

public sealed class HighJump : ActivatableItem
{
    private const float AddedSecondsPerLevel = 2;
    private const float BaseDuration = 10;
    
    public static int Weight => 1;
    
    private const float Multiplayer = 3;
    
    private static float _baseHeight = -1;

    private ThirdPersonController _player;
    
    protected override float ActiveTime => BaseDuration + AddedSecondsPerLevel * Stats.DoubleMoneyLevel.Value;
    
    protected override ItemType ActiveItemType => ItemType.HighJump;

    public void Init(
        float baseJumpHeight,
        ActiveItemsUI activeItemUI,
        Run run,
        ItemsActiveStates itemsActiveStates,
        Effects effects)
    {
        base.Init(activeItemUI, run, itemsActiveStates, effects);

        _baseHeight = baseJumpHeight;
    }

    public override void PickupItem(ThirdPersonController player)
    {
        _player = player;
        
        Sounds.Instance.PlaySound(2, "HighJump");
        Achievements.Instance.GetAchievement("Item_4");
        
        base.PickupItem(player);
    }
    
    protected override void Activate()
    {
        _player.JumpHeight = _baseHeight * Multiplayer;
    }

    protected override void Deactivate()
    {
        _player.JumpHeight = _baseHeight;
    }
}