using System.Collections.Generic;
using UnityEngine;

public sealed class Effects
{
    private const int DefaultEffectTimeMilliseconds = 1000;
    
    private const string JumpEffectResourceName = "Effects/JumpEffect";
    private const string ExplosionEffectResourceName = "Effects/ExplosionEffect";
    private const string PickupItemEffectResourceName = "Effects/PickupItemEffect";
    
    private readonly Dictionary<EffectType, GameObjectPoolMono<Effect>> _effects;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;
    
    public Effects(ICancellationTokenProvider cancellationTokenProvider)
    {
        _cancellationTokenProvider = cancellationTokenProvider;
        
        Transform parent = new GameObject("EffectsParent").transform;
        
        _effects = new Dictionary<EffectType, GameObjectPoolMono<Effect>>
        {
            [EffectType.Jump] = new(
                ResourcesLoader.LoadObject(JumpEffectResourceName),
                parent,
                true,
                1),
            [EffectType.Explosion] = new(
                ResourcesLoader.LoadObject(ExplosionEffectResourceName),
                parent,
                true,
                1),
            [EffectType.PickupItem] = new(
                ResourcesLoader.LoadObject(PickupItemEffectResourceName),
                parent,
                true,
                1),
        };
    }

    public void ActivateEffect(
        EffectType effectType,
        Vector3 position,
        int timeMilliseconds = DefaultEffectTimeMilliseconds)
    {
        Effect effect = _effects[effectType].GetItem();
        effect.transform.position = position;
        
        effect.Activate(timeMilliseconds, _cancellationTokenProvider);
    }
}