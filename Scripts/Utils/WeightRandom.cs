using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public sealed class WeightRandom
{
    private readonly int[] _weights;
    private readonly bool _canRepeatValues;
    
    private int _lastValue = -1;
    
    public WeightRandom(List<IWeightable> weigtables, bool canRepeatValues = false)
    {
        _canRepeatValues = canRepeatValues;
        
        _weights = new int[weigtables.Count];
        int currentTotalWeight = 0;
        for (int i = 0; i < weigtables.Count; i++)
        {
            currentTotalWeight += weigtables[i].Weight;
            _weights[i] = currentTotalWeight;
        }
    }
    
    public int GetRandom()
    {
        int blockID;
        do
        {
            int r = Random.Range(0, _weights[^1]);
            blockID = Array.BinarySearch(_weights, r);
            blockID = blockID < 0 ? ~blockID : blockID;
        } while (blockID == _lastValue && !_canRepeatValues);

        _lastValue = blockID;
        return blockID;
    }
}