using System.Globalization;
using UnityEngine;

public sealed class EnvironmentBlockFactory : AbstractFactory<EnvironmentBlock>
{
    private readonly EnvironmentBlockInfo _environmentBlockInfo;
    private float _count;
    
    public EnvironmentBlockFactory(EnvironmentBlockInfo environmentBlockInfo)
    {
        _environmentBlockInfo = environmentBlockInfo;
    }
    
    public override EnvironmentBlock CreateItem()
    {
        GameObject parent = new(
            $"MapPartParent_{_environmentBlockInfo.name}{_count.ToString(CultureInfo.InvariantCulture)}");
        GameObject environmentBlockObject = Object.Instantiate(_environmentBlockInfo.Prefab, parent.transform);
        var environmentBlock = environmentBlockObject.GetComponent<EnvironmentBlock>();
        environmentBlock.Init(
            _environmentBlockInfo.Terrain.terrainData.size.z *
            _environmentBlockInfo.Prefab.transform.localScale.z);
        return environmentBlock;
    }
}