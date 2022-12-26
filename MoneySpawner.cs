using UnityEngine;

public class MoneySpawner
{
    private const float MoneyDistance = 1f;
    
    private readonly float _height;
    private readonly float _gravity;
    private readonly float _speed;
    private readonly float _playerHalfHeight;
    private readonly FactoryPool<Money> _moneyPool;
    private readonly float[] _spawnLines;

    public MoneySpawner(
        MoneyFactory<Money> moneyFactory,
        float height,
        float gravity,
        float speed,
        float playerHeight,
        float[] spawnLines = null)
    {
        _height = height;
        _gravity = gravity;
        _speed = speed;
        _playerHalfHeight = playerHeight / 2;
        _moneyPool = new FactoryPool<Money>(moneyFactory, null, true);
        _spawnLines = spawnLines;
    }
    
    public void SpawnMoneys(Vector3 startPosition, float activeTime)
    {
        float currentPosition = 0;
        Vector3 position = startPosition;
        float timeStep = Time.fixedDeltaTime;
        float distance = Time.fixedDeltaTime * _speed;
        float gravity = Mathf.Sqrt(_height * -2f * _gravity);
        int counter = 0;
        int maxCounter = (int)(activeTime * _speed / MoneyDistance);
        float currentGravity = 0;

        while (counter < maxCounter)
        {
            currentGravity += gravity * timeStep;
            if (gravity > 0)
            {
                gravity += _gravity * timeStep;

                if (gravity < 0)
                {
                    gravity = 0;
                }
            }
            currentPosition += distance;
            if (currentPosition > MoneyDistance)
            {
                if (_spawnLines != null)
                {
                    for (int i = 0; i < _spawnLines.Length; i++)
                    {
                        position = new Vector3(
                            _spawnLines[i],
                            currentGravity + _playerHalfHeight,
                            position.z + MoneyDistance);
                        _moneyPool.GetItem().transform.position = position;
                    }
                }
                else
                {
                    position = new Vector3(
                        Level.GetClosestColumn(position.x),
                        currentGravity + _playerHalfHeight,
                        position.z + MoneyDistance);
                    _moneyPool.GetItem().transform.position = position;
                }

                currentPosition = 0;
                counter++;
            }
        }
    }
}