using UnityEngine;

public sealed class MainMenuLocation : IRunnable
{
    private readonly GameObject _startLocation;
    private readonly Transform _player;
    
    public MainMenuLocation(BoxCollider startLocation, Transform player)
    {
        _startLocation = Object.Instantiate(startLocation.gameObject);
        float sizeZ = startLocation.size.z;
        _startLocation.transform.position = Vector3.back * (sizeZ / 2);

        _player = player;
    }

    private void ChangeActive(bool active)
    {
        _startLocation.SetActive(active);
    }

    public void StartRun()
    {
        
    }

    public void EndRun()
    {
        ChangeActive(true);
    }

    public void CheckToHideBlock()
    {
        if (!_startLocation.activeSelf) return;
        
        if (_player.transform.position.z > -_startLocation.transform.position.z)
        {
            ChangeActive(false);
        }
    }
}