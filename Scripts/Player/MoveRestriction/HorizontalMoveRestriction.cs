public class HorizontalMoveRestriction
{
    protected int _currentMovingLine;

    public virtual bool CheckHorizontalMoveRestriction(int moveDirection)
    {
        ChangeCurrentLine(moveDirection);
        
        return true;
    }
    
    public HorizontalMoveRestriction ChangeRestriction(HorizontalMoveRestriction newRestriction)
    {
        newRestriction.Init(_currentMovingLine);
        return newRestriction;
    }

    protected void ChangeCurrentLine(int moveDirection)
    {
        _currentMovingLine += moveDirection;
    }
    
    private void Init(int currentMovingLine)
    {
        _currentMovingLine = currentMovingLine;
    }
}