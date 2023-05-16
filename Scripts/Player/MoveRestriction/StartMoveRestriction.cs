public class StartMoveRestriction : HorizontalMoveRestriction
{
    public override bool CheckHorizontalMoveRestriction(int moveDirection)
    {
        return false;
    }
}