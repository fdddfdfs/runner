using UnityEngine;

public class MoneySpawner
{
    private const float MoneyDistance = 1f;
    private const float MoneySpawnDistance = 200;
    
    private readonly MoneyFactory<Money> _moneyFactory;
    private readonly float _height;
    private readonly float _gravity;
    private readonly float _speed;
    private readonly float _playerHalfHeight;

    public MoneySpawner(
        MoneyFactory<Money> moneyFactory,
        float height,
        float gravity,
        float speed,
        float playerHeight)
    {
        _moneyFactory = moneyFactory;
        _height = height;
        _gravity = gravity;
        _speed = speed;
        _playerHalfHeight = playerHeight / 2;
    }
    //TODO: fix money returning to start location
    public void SpawnMoneys(Vector3 startPosition)
    {
        float currentPosition = 0;
        Vector3 position = startPosition;
        float timeStep = Time.fixedDeltaTime;
        float distance = Time.fixedDeltaTime * _speed;
        float gravity = Mathf.Sqrt(_height * -2f * _gravity);
        int counter = 0;
        int maxCounter = (int)(MoneySpawnDistance/ MoneyDistance);
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
                position = new Vector3(
                    Level.GetClosestColumn(position.x),
                    currentGravity + _playerHalfHeight,
                    position.z + MoneyDistance);
                _moneyFactory.CreateItem().transform.position = position;
                currentPosition = 0;
                counter++;
            }
        }
    }
}