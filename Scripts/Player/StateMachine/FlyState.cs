using StarterAssets;

public class FlyState : PlayerActivateState, IState
{
    private readonly Follower _follower;
    
    public FlyState(ThirdPersonController player, ActiveItemsUI activeItemsUI, Follower follower)
        : base(player, activeItemsUI)
    {
        _follower = follower;
    }

    public void EnterState()
    {
        _player.StopRecover();

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