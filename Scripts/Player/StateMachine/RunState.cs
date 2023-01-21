using StarterAssets;

public class RunState : PlayerState, IState
{
    public RunState(ThirdPersonController player) : base(player) { }
    
    public void EnterState()
    {
        _player.ChangeGravitable(_player.Gravitables[typeof(DefaultGravity)]);
        _player.ChangeHittable(_player.Hittables[typeof(PlayerHittable)]);
        _player.ChangeHorizontalMoveRestriction(_player.HorizontalMoveRestrictions[typeof(HorizontalMoveRestriction)]);
        _player.PlayerAnimator.ChangeAnimator(typeof(PlayerDefaultAnimator));
    }

    public void ExitState()
    {
        
    }
}