using UnityEngine;
using UnityEngine.PlayerLoop;

public class RandomSpawnLine
{
    private const int MinSpawnedItemsInLine = 15;
    private const int ChanceToChangeLine = 10; // actual chance is 1 / ChangeToChangeLine
    private const int DoubleChanceToChangeLine = ChanceToChangeLine * 2;
    
    private readonly float[] _allSpawnLines;
    
    private float[] _spawnLines;
    private int _spawnedItemsCount;
    private int _currentLine;

    public RandomSpawnLine()
    {
        _allSpawnLines = Map.AllLinesCoordsX;
    }

    public void Init(float startPositionX)
    {
        _spawnedItemsCount = 0;
        _currentLine = Map.GetClosestColumnIndex(startPositionX);
        _spawnLines = new[] { _allSpawnLines[_currentLine] };
    }

    public float[] GetLines()
    {
        _spawnedItemsCount += 1;
        if (_spawnedItemsCount > MinSpawnedItemsInLine)
        {
            int r = Random.Range(0, DoubleChanceToChangeLine);
            if (r is 0 or 1)
            {
                if (r is 0 && _currentLine != 0 || _currentLine == _allSpawnLines.Length - 1)
                {
                    _currentLine -= 1;
                }
                else if (r is 1 || _currentLine is 0)
                {
                    _currentLine += 1;
                }

                _spawnLines[0] = _allSpawnLines[_currentLine];
                _spawnedItemsCount = 0;
            }
        }

        return _spawnLines;
    }
}