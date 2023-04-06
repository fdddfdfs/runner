using System.Collections.Generic;
using UnityEngine;

public sealed class Road : MapPart<RoadBlockInfo, RoadBlock>
{
    private readonly List<RoadBlockInfo> _roadBlockInfo;
    private readonly List<RoadBlockDamageInfo> _roadBlockDamageInfo;

    public Road(List<RoadBlockInfo> roadBlockInfo, Transform player, List<RoadBlockDamageInfo> roadBlockDamageInfo)
        : base(roadBlockInfo, player, false, 0.2f)
    {
        _roadBlockInfo = roadBlockInfo;
        _roadBlockDamageInfo = roadBlockDamageInfo;
    }

    protected override Dictionary<int, FactoryPoolMono<RoadBlock>> InitializeBlockPools()
    {
        WeightRandom roadBlockDamageRandom = new(new List<IWeightable>(_roadBlockDamageInfo));
        
        Dictionary<int, FactoryPoolMono<RoadBlock>> blockPools = new();
        for (var i = 0; i < _roadBlockInfo.Count; i++)
        {
            AbstractFactory<RoadBlock> factory = new RoadBlockFactory(
                _roadBlockInfo[i],
                _roadBlockDamageInfo,
                roadBlockDamageRandom);
            blockPools.Add(i, new FactoryPoolMono<RoadBlock>(factory, null, true, 0));
        }

        return blockPools;
    }
}