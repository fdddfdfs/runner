public class ImmuneHittable : IHittable
{
    private readonly Level _level;
    
    public ImmuneHittable(Level level)
    {
        _level = level;
    }
    
    public bool Hit(HitType hitType)
    {
        if (hitType == HitType.Hard)
        {
            _level.HideCurrentBlock();
        }

        return false;
    }
}