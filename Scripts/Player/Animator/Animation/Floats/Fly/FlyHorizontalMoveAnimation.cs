public class FlyHorizontalMoveAnimation : FlyAnimation, IFloatAnimation
{
    public FlyHorizontalMoveAnimation(FlyVisual flyVisual) : base(flyVisual){}

    public void SetFloat(float value)
    {
        _flyVisual.HorizontalMove(value);
    }
}