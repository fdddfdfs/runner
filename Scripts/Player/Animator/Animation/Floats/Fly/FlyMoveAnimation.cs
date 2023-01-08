public class FlyMoveAnimation : FlyAnimation, IFloatAnimation
{
    public FlyMoveAnimation(FlyVisual flyVisual) : base(flyVisual){}

    public void SetFloat(float value)
    {
        _flyVisual.Move(value);
    }
}