using StarterAssets;

public sealed class Spring : Item
{
    public static int Weight => 100;
    
    public void Init(Run run)
    {
        base.Init(run);
    }
    
    public override void PickupItem(ThirdPersonController player)
    {
        base.PickupItem(player);
        
        player.ChangeGravitable(player.Gravitables[typeof(SpringGravity)]);
    }
}