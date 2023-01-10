using UnityEngine;

public class EnvironmentBlockFactory : AbstractFactory<EnvironmentBlock>
{
    private readonly EnvironmentBlockInfo _environmentBlockInfo;
    private float _count;
    
    public EnvironmentBlockFactory(EnvironmentBlockInfo environmentBlockInfo)
    {
        _environmentBlockInfo = environmentBlockInfo;
    }
    
    public override EnvironmentBlock CreateItem()
    {
        GameObject parent = new($"MapPartParent_{_environmentBlockInfo.name}{_count.ToString()}");
        GameObject environmentBlockObject = Object.Instantiate(_environmentBlockInfo.Prefab, parent.transform);
        var environmentBlock = environmentBlockObject.GetComponent<EnvironmentBlock>();
        environmentBlock.Init(
            _environmentBlockInfo.Terrain.terrainData.size.z *
            _environmentBlockInfo.Prefab.transform.localScale.z);
        return environmentBlock;
    }
}