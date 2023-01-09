using UnityEngine;

public class ImmuneVisual : Visual
{
    protected override Quaternion RotatorAngle => Quaternion.Euler(1,0,0);
}