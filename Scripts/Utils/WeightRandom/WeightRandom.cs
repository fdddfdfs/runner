using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public sealed class WeightRandom
{
    private readonly int[] _weights;
    private readonly bool _canRepeatValues;
    private readonly int _totalWeights;
    
    private int _lastValue = -1;

    public WeightRandom(IReadOnlyList<IWeightable> weigtables, bool canRepeatValues = false)
    {
        _canRepeatValues = canRepeatValues;
        
        _weights = new int[weigtables.Count];
        var currentTotalWeight = 0;
        for (var i = 0; i < weigtables.Count; i++)
        {
            currentTotalWeight += weigtables[i].Weight;
            _weights[i] = currentTotalWeight;
        }

        _totalWeights = currentTotalWeight;
    }

    public WeightRandom(IReadOnlyList<int> weights, bool canRepeatValues = false)
    {
        _canRepeatValues = canRepeatValues;
        
        _weights = new int[weights.Count];
        var currentTotalWeight = 0;
        for (var i = 0; i < weights.Count; i++)
        {
            currentTotalWeight += weights[i];
            _weights[i] = currentTotalWeight;
        }

        _totalWeights = currentTotalWeight;
    }
    
    public int GetRandom()
    {
        int blockID;
        do
        {
            int r = Random.Range(0, _totalWeights);
            blockID = Array.BinarySearch(_weights, r);
            blockID = blockID < 0 ? ~blockID : blockID;
        } while (blockID == _lastValue && !_canRepeatValues);

        _lastValue = blockID;
        return blockID;
    }
}