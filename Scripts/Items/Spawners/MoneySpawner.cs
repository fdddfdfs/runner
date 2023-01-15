using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using UnityEngine;

public sealed class MoneySpawner
{
    private const float MoneyDistance = 1f;
    private const int MaxIterationsCount = 1000;
    private const int MoneyDeactivateTimeMilliseconds = 10 * 1000;

    private readonly float _height;
    private readonly float _gravity;
    private readonly float _speed;
    private readonly float _playerHalfHeight;
    private readonly FactoryPoolMono<RuntimeItemParent> _moneyPool;
    private readonly RunProgress _runProgress;
    private readonly RandomSpawnLine _randomSpawnLine;

    private float[] _spawnLines;

    public MoneySpawner(
        float height,
        float gravity,
        float speed,
        float playerHeight,
        RunProgress runProgress,
        Run run,
        float[] spawnLines = null)
    {
        MoneyItemFactory<Item> moneyItemFactory = new(runProgress, run, false, true);
        _height = height;
        _gravity = gravity;
        _speed = speed;
        _playerHalfHeight = playerHeight / 2;
        _moneyPool = new FactoryPoolMono<RuntimeItemParent>(
            new RuntimeItemParentFactory(moneyItemFactory),
            null,
            true);
        _spawnLines = spawnLines;
        _runProgress = runProgress;
        _randomSpawnLine = new RandomSpawnLine();
    }
    
    public float SpawnMoneys(Vector3 startPosition, float activeTime)
    {
        float currentPosition = 0;
        float distance = Time.fixedDeltaTime * _speed * _runProgress.SpeedMultiplier;
        float gravity = Mathf.Sqrt(_height * -2f * _gravity);
        float currentGravity = 0;
        float currentTime = 0;
        
        _randomSpawnLine.Init(startPosition.x);

        while (currentTime < activeTime)
        {
            currentTime += Time.fixedDeltaTime;
            currentGravity += gravity * Time.fixedDeltaTime;
            if (gravity > 0)
            {
                gravity += _gravity * Time.fixedDeltaTime;

                if (gravity < 0)
                {
                    gravity = 0;
                }
            }

            currentPosition += distance;
            if (currentPosition > MoneyDistance)
            {
                if (gravity == 0)
                {
                    _spawnLines = _randomSpawnLine.GetLines();
                }

                startPosition = SpawnMoneyItem(startPosition, currentGravity);
                currentPosition -= MoneyDistance;
            }
        }

        return startPosition.z;
    }
    
    public float SpawnMoneys(float gravityRestriction, Vector3 startPosition)
    {
        float currentPosition = 0;
        float distance = Time.fixedDeltaTime * _speed * _runProgress.SpeedMultiplier;
        float gravity = Mathf.Sqrt(_height * -2f * _gravity);
        float currentGravity = 0;
        int counter = 0;

        while (true)
        {
            currentGravity += gravity * Time.fixedDeltaTime;
            if (gravity > 0 ||  currentGravity > gravityRestriction)
            {
                gravity += _gravity * Time.fixedDeltaTime;
            }
            else
            {
                return startPosition.z;
            }

            currentPosition += distance;
            if (currentPosition > MoneyDistance)
            {
                startPosition = SpawnMoneyItem(startPosition, currentGravity);
                currentPosition -= MoneyDistance;
                counter++;
            }

            if (counter > MaxIterationsCount)
            {
                throw new Exception("Too many money spawned, might be infinity loop");
            }
        }
    }

    private Vector3 SpawnMoneyItem(Vector3 position, float currentGravity)
    {
        for (var i = 0; i < (_spawnLines?.Length ?? 1); i++)
        {
            Vector3 tempPosition = new Vector3(
                _spawnLines == null ? Map.GetClosestColumn(position.x) : _spawnLines[i],
                 position.y + currentGravity + _playerHalfHeight,
                position.z + MoneyDistance);
            RuntimeItemParent itemParent = _moneyPool.GetItem();
            itemParent.transform.position = tempPosition;
            itemParent.ItemObject.DeactivateInTime();
        }

        return position + MoneyDistance * Vector3.forward;
    }
}