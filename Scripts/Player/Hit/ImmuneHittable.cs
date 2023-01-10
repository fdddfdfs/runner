public sealed class ImmuneHittable : IHittable
{
    private readonly Map _map;
    
    public ImmuneHittable(Map map)
    {
        _map = map;
    }
    
    public bool Hit(HitType hitType)
    {
        if (hitType == HitType.Hard)
        {
            _map.Level.HideCurrentEnteredBlock();
        }

        return false;
    }
}