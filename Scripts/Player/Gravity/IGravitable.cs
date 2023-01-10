public interface IGravitable
{
    public void EnterGravity(){}

    public void LeaveGravity(){}

    public float VerticalVelocity(bool isGrounded);
}