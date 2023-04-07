using System;
using Gaia;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class MainMenuLocation : IRunnable
{
    private readonly GameObject _startLocation;
    private readonly Transform _player;
    private readonly TerrainDetailOverwrite _terrainDetailOverwrite;

    private float _currentDetailsDensity;
    
    public MainMenuLocation(BoxCollider startLocation, Transform player)
    {
        _startLocation = Object.Instantiate(startLocation.gameObject);
        float sizeZ = startLocation.size.z;
        _startLocation.transform.position = Vector3.back * (sizeZ / 2);

        _player = player;

        _terrainDetailOverwrite = _startLocation.GetComponentInChildren<TerrainDetailOverwrite>();
        _currentDetailsDensity = _terrainDetailOverwrite.m_detailDensity;
    }

    public void StartRun()
    {
        
    }

    public void EndRun()
    {
        ChangeActive(true);
    }

    public void ChangeDetailsDensity(float value)
    {
        if (Math.Abs(_currentDetailsDensity - value) < 0.01f) return;
        
        _terrainDetailOverwrite.m_detailDensity = value;
        _currentDetailsDensity = value;
        
        _terrainDetailOverwrite.ApplySettings(false);
    }

    public void CheckToHideBlock()
    {
        if (!_startLocation.activeSelf) return;
        
        if (_player.transform.position.z > -_startLocation.transform.position.z)
        {
            ChangeActive(false);
        }
    }
    
    private void ChangeActive(bool active)
    {
        _startLocation.SetActive(active);
    }
}