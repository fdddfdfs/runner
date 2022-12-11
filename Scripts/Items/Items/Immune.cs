using System.Collections;
using StarterAssets;
using UnityEngine;

public class Immune : ActivatableItem<Immune>
{
    private static ThirdPersonController _player;
    
    protected override float ActiveTime => 10;
    
    protected override ItemType ActiveItemType => ItemType.Immune;
    
    public override void PickupItem(ThirdPersonController player)
    {
        _player = player;
        
        base.PickupItem(player);
    }
    
    protected override void Activate()
    {
        _player.ChangeHittable(_player.Hittables[typeof(ImmuneHittable)]);
    }

    protected override void Deactivate()
    {
        _player.ChangeHittable(_player.Hittables[typeof(PlayerHittable)]);
    }
}