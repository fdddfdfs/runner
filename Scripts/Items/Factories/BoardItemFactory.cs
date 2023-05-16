using UnityEngine;

public sealed class BoardItemFactory<T> : ItemFactory<T> where T : Item
{
    private readonly Run _run;
    private readonly Effects _effects;
    private readonly PickupCar _pickupCar;
    
    protected override string PrefabName => "Items/Board";

    public BoardItemFactory(Run run, Effects effects, PickupCar pickupCar)
    {
        _run = run;
        _effects = effects;
        _pickupCar = pickupCar;
    }

    public override T CreateItem()
    {
        GameObject boardItemObject = Object.Instantiate(_prefab);
        var boardItem = boardItemObject.AddComponent<BoardItem>();
        boardItem.Init(_run, _effects, _pickupCar);

        return boardItem as T;
    }
}