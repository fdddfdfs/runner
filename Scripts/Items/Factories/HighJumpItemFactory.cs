﻿using StarterAssets;
using UnityEngine;

public sealed class HighJumpItemFactory<T> : ItemFactory<T> where T : Item
{
    protected override string PrefabName => "Items/HighJump";

    private readonly float _baseJumpHeight;
    private readonly ActiveItemsUI _activeItemsUI;
    private readonly Run _run;
    private readonly ItemsActiveStates _itemsActiveStates;

    public HighJumpItemFactory(
        ThirdPersonController player,
        ActiveItemsUI activeItemsUI,
        Run run,
        ItemsActiveStates itemsActiveStates)
    {
        _baseJumpHeight = player.JumpHeight;
        _activeItemsUI = activeItemsUI;
        _run = run;
        _itemsActiveStates = itemsActiveStates;
    }

    public override T CreateItem()
    {
        GameObject highJumpObject = Object.Instantiate(_prefab);
        var highJump = highJumpObject.AddComponent<HighJump>();
        highJump.Init(_baseJumpHeight, _activeItemsUI, _run, _itemsActiveStates);

        return highJump as T;
    }
}