public sealed class VisualJumpAnimation : VisualAnimation, IBoolAnimation
{
    public VisualJumpAnimation(Visual visual) : base(visual) {}

    public void SetBool(bool value)
    {
        _visual.MoveX(value ? 1 : -1);
    }
}