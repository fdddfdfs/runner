using UnityEngine;

public class EnvironmentBillboard : MonoBehaviour, ITriggerable
{
    [SerializeField] private EnvironmentBillboardInfo _environmentBillboardInfo;
    [SerializeField] private MeshRenderer _billboard;
    
    public void Trigger()
    {
        int r = Random.Range(0, _environmentBillboardInfo.SpriteVarients.Count);

        _billboard.material.mainTexture = _environmentBillboardInfo.SpriteVarients[r].texture;
    }
}