using StarterAssets;

public class ImmuneState : PlayerActivateState, IState
{
    public ImmuneState(ThirdPersonController player, ActiveItemsUI activeItemsUI) : base(player, activeItemsUI) { }
    
    public void EnterState()
    {
        _player.ChangeHittable(_player.Hittables[typeof(ImmuneHittable)]);
        _player.PlayerAnimator.ChangeAnimator(typeof(PlayerImmuneAnimator));
        _activeItemsUI.ShowNewItemEffect(ItemType.Immune, 10); //TODO : make it affected by time in item
    }

    public void ExitState()
    {
        _player.ChangeHittable(_player.Hittables[typeof(PlayerHittable)]);
        _player.PlayerAnimator.ChangeAnimator(typeof(PlayerDefaultAnimator));
        _activeItemsUI.HideEffect(ItemType.Immune);
    }
}