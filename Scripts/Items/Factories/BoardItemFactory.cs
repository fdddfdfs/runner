using UnityEngine;

public sealed class BoardItemFactory<T> : ItemFactory<T> where T : Item
{
    private readonly Run _run;
    private readonly Effects _effects;
    
    protected override string PrefabName => "Items/Board";

    public BoardItemFactory(Run run, Effects effects)
    {
        _run = run;
        _effects = effects;
    }

    public override T CreateItem()
    {
        GameObject boardItemObject = Object.Instantiate(_prefab);
        var boardItem = boardItemObject.AddComponent<BoardItem>();
        boardItem.Init(_run, _effects);

        return boardItem as T;
    }
}