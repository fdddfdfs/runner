public class IdleState : IState
{
    private readonly PlayerAnimator _playerAnimator;

    public IdleState(PlayerAnimator playerAnimator)
    {
        _playerAnimator = playerAnimator;
    }

    public void EnterState()
    {
        _playerAnimator.ChangeAnimator(typeof(PlayerIdleAnimator));
    }

    public void ExitState()
    {
        _playerAnimator.ChangeAnimator(typeof(PlayerDefaultAnimator));
    }
}