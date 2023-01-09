public class VisualZMoveAnimation : VisualAnimation, IFloatAnimation
{
    public VisualZMoveAnimation(Visual visual) : base(visual){}

    public void SetFloat(float value)
    {
        _visual.MoveZ(value);
    }
}