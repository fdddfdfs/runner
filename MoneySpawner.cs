using System;
using UnityEngine;

public class MoneySpawner
{
    private const float MoneyDistance = 1f;
    private const int MaxIterationsCount = 1000;
    
    private readonly float _height;
    private readonly float _gravity;
    private readonly float _speed;
    private readonly float _playerHalfHeight;
    private readonly FactoryPool<Item> _moneyPool;
    private readonly float[] _spawnLines;
    private readonly RunProgress _runProgress;

    public MoneySpawner(
        MoneyFactory<Item> moneyFactory,
        float height,
        float gravity,
        float speed,
        float playerHeight,
        RunProgress runProgress,
        float[] spawnLines = null)
    {
        _height = height;
        _gravity = gravity;
        _speed = speed;
        _playerHalfHeight = playerHeight / 2;
        _moneyPool = new FactoryPool<Item>(moneyFactory, null, true);
        _spawnLines = spawnLines;
        _runProgress = runProgress;
    }
    
    public float SpawnMoneys(Vector3 startPosition, float activeTime)
    {
        float currentPosition = 0;
        float distance = Time.fixedDeltaTime * _speed * _runProgress.SpeedMultiplayer;
        float gravity = Mathf.Sqrt(_height * -2f * _gravity);
        float currentGravity = 0;
        float currentTime = 0;

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
                startPosition = SpawnMoneyItem(startPosition, currentGravity);
                currentPosition -= MoneyDistance;
            }
        }

        return startPosition.z;
    }
    
    public float SpawnMoneys(float gravityRestriction, Vector3 startPosition)
    {
        float currentPosition = 0;
        float distance = Time.fixedDeltaTime * _speed * _runProgress.SpeedMultiplayer;
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
        for (int i = 0; i < (_spawnLines?.Length ?? 1); i++)
        {
            Vector3 tempPosition = new Vector3(
                _spawnLines == null ? Map.GetClosestColumn(position.x) : _spawnLines[i],
                currentGravity + _playerHalfHeight,
                position.z + MoneyDistance);
            _moneyPool.GetItem().transform.position = tempPosition;
        }

        return position + MoneyDistance * Vector3.forward;
    }
}