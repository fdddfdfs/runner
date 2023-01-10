public sealed class VisualTakeoffAnimation : VisualAnimation, IBoolAnimation
{
    public VisualTakeoffAnimation(Visual visual) : base(visual){}

    public void SetBool(bool value)
    {
        _visual.MoveX(value? 1 : 0);
    }
}