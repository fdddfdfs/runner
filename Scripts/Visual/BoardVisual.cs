using UnityEngine;

public sealed class BoardVisual : ColliderVisual
{
    protected override Quaternion RotatorAngle { get; } = Quaternion.Euler(-1,0,0);
}