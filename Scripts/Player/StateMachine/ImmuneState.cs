using StarterAssets;
using UnityEngine;

public sealed class ImmuneState : PlayerActivateState, IState
{
    private const int ChangeVisualEffectTimeMilliseconds = 5000;
    private const float EffectPositionYOffset = 2;
    
    private readonly Effects _effects;

    public ImmuneState(ThirdPersonController player, ActiveItemsUI activeItemsUI, Effects effects)
        : base(player, activeItemsUI)
    {
        _effects = effects;
    }
    
    public void EnterState()
    {
        _player.ChangeHittable(_player.Hittables[typeof(ImmuneHittable)]);
        _player.PlayerAnimator.ChangeAnimator(typeof(PlayerImmuneAnimator));
        _effects.ActivateEffect(
            EffectType.ChangeVisual,
            _player.transform.position + EffectPositionYOffset * Vector3.up,
            ChangeVisualEffectTimeMilliseconds);
    }

    public void ExitState()
    {
        _player.ChangeHittable(_player.Hittables[typeof(PlayerHittable)]);
        _player.PlayerAnimator.ChangeAnimator(typeof(PlayerDefaultAnimator));
        _activeItemsUI.HideEffect(ItemType.Immune);
        _effects.ActivateEffect(
            EffectType.ChangeVisual,
            _player.transform.position + EffectPositionYOffset * Vector3.up,
            ChangeVisualEffectTimeMilliseconds);
    }
}