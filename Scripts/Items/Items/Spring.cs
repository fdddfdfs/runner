﻿using System;
using StarterAssets;

public class Spring : Item
{
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