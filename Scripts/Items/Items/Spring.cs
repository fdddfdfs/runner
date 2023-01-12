using StarterAssets;

public sealed class Spring : Item
{
    public static int Weight => 1;
    
    public void Init()
    {
        base.Init();
    }
    
    public override void PickupItem(ThirdPersonController player)
    {
        base.PickupItem(player);
        
        player.ChangeGravitable(player.Gravitables[typeof(SpringGravity)]);
    }
}