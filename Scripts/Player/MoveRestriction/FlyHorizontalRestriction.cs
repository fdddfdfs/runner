using UnityEngine;

public class FlyHorizontalRestriction : HorizontalMoveRestriction
{
    private readonly int _linesHalfCount;

    public FlyHorizontalRestriction()
    {
        _linesHalfCount = (int)Mathf.Floor(Map.LinesCount / 2f);
    }
    
    public override bool CheckHorizontalMoveRestriction(int moveDirection)
    {
        if (_linesHalfCount >= Mathf.Abs(_currentMovingLine + moveDirection))
        {
            ChangeCurrentLine(moveDirection);
            return true;
        }
        
        return false;
    }
}