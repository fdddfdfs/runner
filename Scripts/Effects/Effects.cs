using System.Collections.Generic;
using UnityEngine;

public sealed class Effects
{
    private const int DefaultEffectTimeMilliseconds = 1000;
    
    private const string JumpEffectResourceName = "Effects/JumpEffect";
    private const string ExplosionEffectResourceName = "Effects/ExplosionEffect";
    private const string PickupItemEffectResourceName = "Effects/PickupItemEffect";
    private const string ChangeVisualEffectResourceName = "Effects/ChangeVisualEffect";
    
    private readonly Dictionary<EffectType, GameObject> _effects;
    private readonly ICancellationTokenProvider _cancellationTokenProvider;
    private readonly Transform _parent;
    
    public Effects(ICancellationTokenProvider cancellationTokenProvider)
    {
        _cancellationTokenProvider = cancellationTokenProvider;
        
        _parent = new GameObject("EffectsParent").transform;
        
        _effects = new Dictionary<EffectType, GameObject>
        {
            [EffectType.Jump] = ResourcesLoader.LoadObject(JumpEffectResourceName),
            [EffectType.Explosion] = ResourcesLoader.LoadObject(ExplosionEffectResourceName),
            [EffectType.PickupItem] = ResourcesLoader.LoadObject(PickupItemEffectResourceName),
            [EffectType.ChangeVisual] = ResourcesLoader.LoadObject(ChangeVisualEffectResourceName),
        };
    }

    public void ActivateEffect(
        EffectType effectType,
        Vector3 position,
        int timeMilliseconds = DefaultEffectTimeMilliseconds)
    {
        var effect = ResourcesLoader.InstantiateLoadedComponent<Effect>(_effects[effectType]);
        Transform transform = effect.transform;
        transform.parent = _parent;
        transform.position = position;
        
        effect.Activate(timeMilliseconds, _cancellationTokenProvider);
    }
}