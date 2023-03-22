using StarterAssets;
using UnityEngine;

public sealed class FlyState : PlayerActivateState, IState
{
    private const int ChangeVisualEffectTimeMilliseconds = 5000;
    private const float EffectPositionYOffset = 2;

    private readonly Follower _follower;
    private readonly Effects _effects;
    
    public FlyState(ThirdPersonController player, ActiveItemsUI activeItemsUI, Follower follower, Effects effects)
        : base(player, activeItemsUI)
    {
        _follower = follower;
        _effects = effects;
    }

    public void EnterState()
    {
        _player.StopRecover();

        _player.ChangeGravitable(_player.Gravitables[typeof(FlyGravity)]);
        _player.ChangeHorizontalMoveRestriction(_player.HorizontalMoveRestrictions[typeof(FlyHorizontalRestriction)]);
        _player.PlayerAnimator.ChangeAnimator(typeof(PlayerFlyAnimator));
        _player.ChangeHittable(_player.Hittables[typeof(ImmuneHittable)]);
        _effects.ActivateEffect(
            EffectType.ChangeVisual,
            _player.transform.position + EffectPositionYOffset * Vector3.up,
            ChangeVisualEffectTimeMilliseconds);
    }

    public void ExitState()
    {
        _player.ChangeGravitable(_player.Gravitables[typeof(DefaultGravity)]);
        _player.ChangeHorizontalMoveRestriction(_player.HorizontalMoveRestrictions[typeof(HorizontalMoveRestriction)]);
        _player.PlayerAnimator.ChangeAnimator(typeof(PlayerDefaultAnimator));
        _player.ChangeHittable(_player.Hittables[typeof(PlayerHittable)]);
        _activeItemsUI.HideEffect(ItemType.Fly);
        _effects.ActivateEffect(
            EffectType.ChangeVisual,
            _player.transform.position + EffectPositionYOffset * Vector3.up,
            ChangeVisualEffectTimeMilliseconds);
    }
}