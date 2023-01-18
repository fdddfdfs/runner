using UnityEngine;

public class HorizontalMoveRestriction
{
    protected int _currentMovingLine;

    public int CurrentMovingLine => _currentMovingLine;

    public virtual bool CheckHorizontalMoveRestriction(int moveDirection)
    {
        ChangeCurrentLine(moveDirection);

        return true;
    }

    public void Init(int currentMovingLine)
    {
        _currentMovingLine = currentMovingLine;
    }

    protected void ChangeCurrentLine(int moveDirection)
    {
        _currentMovingLine += moveDirection;
    }
}
 