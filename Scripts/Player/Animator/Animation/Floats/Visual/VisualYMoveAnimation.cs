public sealed class VisualYMoveAnimation : VisualAnimation, IFloatAnimation
{
    public VisualYMoveAnimation(Visual visual) : base(visual) {}

    public void SetFloat(float value)
    {
        _visual.MoveY(-value);
    }
}