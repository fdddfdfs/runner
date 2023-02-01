using System.Collections.Generic;
using UnityEngine;

public sealed class Road : MapPart<RoadBlockInfo, RoadBlock>
{
    private readonly List<RoadBlockInfo> _roadBlockInfos;
    
    public Road(List<RoadBlockInfo> roadBlockInfos, Transform player)
        : base(roadBlockInfos, player, false, 0.2f)
    {
        _roadBlockInfos = roadBlockInfos;
    }

    protected override Dictionary<int, FactoryPoolMono<RoadBlock>> InitializeBlockPools()
    {
        Dictionary<int, FactoryPoolMono<RoadBlock>> blockPools = new();
        for (var i = 0; i < _roadBlockInfos.Count; i++)
        {
            AbstractFactory<RoadBlock> factory = new RoadBlockFactory(_roadBlockInfos[i]);
            blockPools.Add(i, new FactoryPoolMono<RoadBlock>(factory, null, true, 0));
        }

        return blockPools;
    }
}