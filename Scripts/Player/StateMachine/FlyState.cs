using StarterAssets;

public class FlyState : PlayerActivateState, IState
{
    public FlyState(ThirdPersonController player, ActiveItemsUI activeItemsUI) : base(player, activeItemsUI) { }

    public void EnterState()
    {
        _player.ChangeGravitable(_player.Gravitables[typeof(FlyGravity)]);
        _player.ChangeHorizontalMoveRestriction(_player.HorizontalMoveRestrictions[typeof(FlyHorizontalRestriction)]);
        _player.PlayerAnimator.ChangeAnimator(typeof(PlayerFlyAnimator));
        _activeItemsUI.ShowNewItemEffect(ItemType.Fly, 10); //TODO : make it affected by time in item
    }

    public void ExitState()
    {
        _player.ChangeGravitable(_player.Gravitables[typeof(DefaultGravity)]);
        _player.ChangeHorizontalMoveRestriction(_player.HorizontalMoveRestrictions[typeof(HorizontalMoveRestriction)]);
        _player.PlayerAnimator.ChangeAnimator(typeof(PlayerDefaultAnimator));
        _activeItemsUI.HideEffect(ItemType.Fly);
    }
}