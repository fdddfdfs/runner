using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnvironmentBillboard", menuName = "Environments/EnvironmentBillboardInfo")]
public class EnvironmentBillboardInfo : ScriptableObject
{
    [SerializeField] private List<Sprite> _spriteVarients;

    public IReadOnlyList<Sprite> SpriteVarients => _spriteVarients;
}