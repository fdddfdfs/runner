public class FlyTakeoffAnimation : FlyAnimation, IBoolAnimation
{
    public FlyTakeoffAnimation(FlyVisual flyVisual) : base(flyVisual){}

    public void SetBool(bool value)
    {
        _flyVisual.Takeoff(value);
    }
}