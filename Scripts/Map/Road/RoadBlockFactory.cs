using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public sealed class RoadBlockFactory : AbstractFactory<RoadBlock>
{
    private const int RoadBlockDamageOffset = 5;
    private const int RoadBlockDamageSpawnChance = 3;
    private const float RoadBlockDamageYOffset = 0.01f;
    
    private readonly RoadBlockInfo _roadBlockInfo;
    private readonly List<RoadBlockDamageInfo> _roadBlockDamageInfo;
    private readonly WeightRandom _roadBlockDamageRandom;

    private int _count;
    
    public RoadBlockFactory(
        RoadBlockInfo roadBlockInfo,
        List<RoadBlockDamageInfo> roadBlockDamageInfo,
        WeightRandom roadBlockDamageRandom)
    {
        _roadBlockInfo = roadBlockInfo;
        _roadBlockDamageInfo = roadBlockDamageInfo;
        _roadBlockDamageRandom = roadBlockDamageRandom;
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
        
        SetDamage(road, roadCollider);
        
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

    private void SetDamage(GameObject road, BoxCollider roadCollider)
    {
        float positionZ = RoadBlockDamageOffset;

        while (positionZ < roadCollider.size.z)
        {
            int r = Random.Range(0, RoadBlockDamageSpawnChance);
            if (r != 0)
            {
                positionZ += RoadBlockDamageOffset;
                continue;
            }
            
            RoadBlockDamageInfo damageInfo = _roadBlockDamageInfo[_roadBlockDamageRandom.GetRandom()];
            float size = Random.Range(damageInfo.MinScale, damageInfo.MaxScale);
            float positionX = Random.Range(-Map.ColumnOffset * 2 + size / 2, Map.ColumnOffset * 2 - size / 2);
            GameObject damage = Object.Instantiate(damageInfo.Prefab, road.transform);
            damage.transform.localPosition = new Vector3(positionX, RoadBlockDamageYOffset, positionZ);
            damage.transform.localRotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
            damage.transform.localScale = new Vector3(size, 1, size);

            positionZ += RoadBlockDamageOffset + size;
        }
    }
}