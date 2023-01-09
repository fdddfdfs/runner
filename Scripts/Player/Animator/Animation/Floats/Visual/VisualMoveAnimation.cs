public class VisualMoveAnimation : VisualAnimation, IFloatAnimation
{
    public VisualMoveAnimation(Visual visual) : base(visual){}

    public void SetFloat(float value)
    {
        _visual.Move(value);
    }
}