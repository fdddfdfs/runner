using UnityEngine;

public class FlyVisual : Visual
{
    protected override Quaternion RotatorAngle { get; } = Quaternion.Euler(0, 0, 1);
}