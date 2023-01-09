using UnityEngine;

public class BoardVisual : Visual
{
    protected override Quaternion RotatorAngle { get; } = Quaternion.Euler(1,0,0);
}