using UnityEngine;

public sealed class ImmuneVisual : ColliderVisual
{
    protected override Quaternion RotatorAngle => Quaternion.Euler(-1,0,0);
}