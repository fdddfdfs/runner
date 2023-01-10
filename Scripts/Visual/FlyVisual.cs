using UnityEngine;

public sealed class FlyVisual : Visual
{
    protected override Quaternion RotatorAngle { get; } = Quaternion.Euler(0, 0, 1);
}