using System;
using System.Globalization;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class RoadBlockFactory : AbstractFactory<RoadBlock>
{
    private readonly RoadBlockInfo _roadBlockInfo;

    private int _count;
    
    public RoadBlockFactory(RoadBlockInfo roadBlockInfo)
    {
        _roadBlockInfo = roadBlockInfo;
    }
    
    public override RoadBlock CreateItem()
    {
        GameObject parent = new(
            $"RoadBlock_{_roadBlockInfo.name}_{_count.ToString(CultureInfo.InvariantCulture)}");
        GameObject leftBorder = Object.Instantiate(_roadBlockInfo.RoadLine.LeftBorder, parent.transform);
        GameObject road = Object.Instantiate(_roadBlockInfo.RoadLine.Road, parent.transform);
        GameObject rightBorder = Object.Instantiate(_roadBlockInfo.RoadLine.RightBorder, parent.transform);

        road.layer = LayerMask.NameToLayer("Ground");
        leftBorder.layer = LayerMask.NameToLayer("Obstacle");
        rightBorder.layer = LayerMask.NameToLayer("Obstacle");
        
        var roadBlock = parent.AddComponent<RoadBlock>();
        
        var roadCollider = road.GetComponent<BoxCollider>();
        if (!roadCollider)
        {
            throw new Exception($"Road prefab {road} must have BoxCollider");
        }
        
        SetBoarderPosition(leftBorder, roadCollider, -1);
        SetBoarderPosition(rightBorder, roadCollider, 1);
        Vector3 size = roadCollider.size;
        road.transform.localPosition = new Vector3(0, -size.y / 2, 0);
        
        roadBlock.Init(size.z);
        
        _count++;
        
        StaticBatchingUtility.Combine(parent);
        
        return roadBlock;
    }

    private static void SetBoarderPosition(GameObject border, BoxCollider road, int dir)
    {
        var borderCollider = border.GetComponent<BoxCollider>();
        if (!borderCollider)
        {
            throw new Exception($"Boarder {border.name} prefab must have BoxCollider");
        }

        Vector3 borderSize = borderCollider.size;
        border.transform.localPosition = new Vector3(
            dir * (road.size.x / 2 + borderSize.x / 2),
            borderSize.y / 2,
            0);
    }
}