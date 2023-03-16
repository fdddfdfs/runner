using System.Collections.Generic;

public sealed class EmptyObstacle : Obstacle
{
    public void InitItems(List<ItemParent> items)
    {
        _items = items;
    }
}