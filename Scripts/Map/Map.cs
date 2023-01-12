using System.Collections.Generic;
using UnityEngine;

public sealed class Map : MonoBehaviour, IPauseable , IRunnable
{
    public const int ColumnOffset = 3;
    public const int LinesCount = 3;

    [SerializeField] private List<LevelBlockInfo> _levelBlocks;
    [SerializeField] private LevelBlockInfo _startLevelBlock;
    [SerializeField] private List<EnvironmentBlockInfo> _environmentBlockInfos;
    [SerializeField] private Transform _player;
    [SerializeField] private Factories _factories;
    [SerializeField] private RunProgress _runProgress;
    [SerializeField] private bool _needSpawnLevel;
    [SerializeField] private bool _needSpawnEnvironment;

    private Level _level;
    private Environment _environment;
    private List<IRunnable> _runnables;
    
    private bool _isPause;

    public Level Level => _level;

    public static float[] AllLinesCoordsX { get; } = { -ColumnOffset, 0, ColumnOffset };

    public static float GetClosestColumn(float positionX)
    {
        return positionX < -ColumnOffset / 2f ? -ColumnOffset : positionX < ColumnOffset / 2f ? 0 : ColumnOffset;
    }

    public static int GetClosestColumnIndex(float positionX)
    {
        return positionX < -ColumnOffset / 2f ? 0 : positionX < ColumnOffset / 2f ? 1 : 2;
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
        foreach (IRunnable runnable in _runnables)
        {
            runnable?.StartRun();
        }
        
        _isPause = false;
    }

    public void EndRun()
    {
        foreach (IRunnable runnable in _runnables)
        {
            runnable?.EndRun();
        }

        _isPause = true;
    }
    
    private void Awake()
    {
        _isPause = true;
    }

    private void Start()
    {
        _level = _needSpawnLevel? new Level(_levelBlocks, _startLevelBlock, _factories, _player, _runProgress) : null;
        _environment = _needSpawnEnvironment? new Environment(_environmentBlockInfos, _player) : null;
        _runnables = new List<IRunnable> { _level, _environment };
    }

    private void Update()
    {
        if (_isPause)
            return;
        
        _level?.CheckToHideBlock();
        _environment?.CheckToHideBlock();
    }
}
