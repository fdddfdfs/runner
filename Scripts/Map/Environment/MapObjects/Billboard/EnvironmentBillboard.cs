using System.Collections.Generic;
using UnityEngine;

public class EnvironmentBillboard : MonoBehaviour, ITriggerable
{
    [SerializeField] private List<EnvironmentBillboardInfo> _environmentBillboardInfos;
    [SerializeField] private MeshRenderer _billboard;
    
    public void Trigger()
    {
        int r = Random.Range(0, _environmentBillboardInfos.Count);

        _billboard.material.mainTexture = _environmentBillboardInfos[r].Sprite.texture;
        Achievements.Instance.GetAchievement(_environmentBillboardInfos[r].BillboardAchievement.Name);
    }
}