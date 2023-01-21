using StarterAssets;

public abstract class PlayerState
{
    protected readonly ThirdPersonController _player;

    protected PlayerState(ThirdPersonController player)
    {
        _player = player;
    }
}