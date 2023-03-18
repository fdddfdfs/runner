using StarterAssets;

public sealed class BoardState : PlayerActivateState, IState
{
    public BoardState(ThirdPersonController player, ActiveItemsUI activeItemsUI) : base(player, activeItemsUI) { }

    public void EnterState()
    {
        IHittable boardHittable = _player.Hittables[typeof(Board)];
        (boardHittable as Board)?.Activate();
        _player.ChangeHittable(boardHittable);
        
        _player.PlayerAnimator.ChangeAnimator(typeof(PlayerBoardAnimator));
        _activeItemsUI.ShowNewItemEffect(ItemType.Board, Board.BoardDuration);
    }

    public void ExitState()
    {
        _player.ChangeHittable(_player.Hittables[typeof(PlayerHittable)]);
        _player.PlayerAnimator.ChangeAnimator(typeof(PlayerDefaultAnimator));
        _activeItemsUI.HideEffect(ItemType.Board);
    }
}