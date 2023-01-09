public class VisualFallAnimation : VisualAnimation, IBoolAnimation
{
    public VisualFallAnimation(Visual visual) : base(visual) {}

    public void SetBool(bool value)
    {
        _visual.MoveX(value? -1 : 0);
    }
}