using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class Map : MonoBehaviour, IPauseable
{
    public const int ColumnOffset = 3;
    public const int LinesCount = 3;

    [SerializeField] private List<LevelBlockInfo> _levelBlocks;
    [SerializeField] private List<EnvironmentBlockInfo> _environmentBlockInfos;
    [SerializeField] private Transform _player;
    [SerializeField] private Factories _factories;
    [SerializeField] private RunProgress _runProgress;
    [SerializeField] private bool _needSpawnLevel;
    [SerializeField] private bool _needSpawnEnvironment;

    private Level _level;
    private Environment _environment;

    private bool _isPause;

    public Level Level => _level;

    public static float GetClosestColumn(float positionX)
    {
        return positionX < -ColumnOffset / 2f ? -ColumnOffset : positionX < ColumnOffset / 2f ? 0 : ColumnOffset;
    }

    public void Pause()
    {
        _isPause = true;
    }

    public void UnPause()
    {
        _isPause = false;
    }

    public void StartRun()
    {
        _isPause = false;

        _level?.StartRun();
        _environment?.StartRun();
    }

    public void EndRun()
    {
        _level?.EndRun();
        _environment?.EndRun();
    }
    
    private void Awake()
    {
        _isPause = true;
    }

    private void Start()
    {
        _level = _needSpawnLevel? new Level(_levelBlocks, _factories, _player, ColumnOffset, _runProgress) : null;
        _environment = _needSpawnEnvironment? new Environment(_environmentBlockInfos, _player) : null;
    }

    private void Update()
    {
        if (_isPause)
            return;
        
        _level?.CheckToHideBlock();
        _environment?.CheckToHideBlock();
    }
}
