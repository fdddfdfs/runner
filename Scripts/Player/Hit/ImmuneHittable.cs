using UnityEngine;

public sealed class ImmuneHittable : IHittable
{
    private const float EffectYOffset = 1;
    private const int EffectTimeMilliseconds = 5000;
    
    private readonly Map _map;
    private readonly Effects _effects;
    private readonly Transform _player;
    
    public ImmuneHittable(Map map, Effects effects, Transform player)
    {
        _map = map;
        _effects = effects;
        _player = player;
    }
    
    public bool Hit(HitType hitType)
    {
        if (hitType == HitType.Hard)
        {
            _effects.ActivateEffect(
                EffectType.Explosion,
                _player.position + EffectYOffset * Vector3.up,
                EffectTimeMilliseconds);
            _map.Level.HideCurrentEnteredBlock();
        }

        return false;
    }
}