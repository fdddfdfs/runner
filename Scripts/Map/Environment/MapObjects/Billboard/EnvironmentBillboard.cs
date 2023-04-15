using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class EnvironmentBillboard : MonoBehaviour, ITriggerable
{
    private const int ImageMaterialNumber = 1;
    
    [SerializeField] private List<EnvironmentBillboardInfo> _environmentBillboardInfos;
    [SerializeField] private MeshRenderer _billboard;

    private int _currentBillboard;
    
    public void Trigger()
    {
        Achievements.Instance.GetAchievement(_environmentBillboardInfos[_currentBillboard].BillboardAchievement.Name);
    }

    private void OnEnable()
    {
        int r = Random.Range(0, _environmentBillboardInfos.Count);

        _billboard.materials[ImageMaterialNumber].mainTexture = _environmentBillboardInfos[r].Sprite.texture;

        _currentBillboard = r;
    }
}