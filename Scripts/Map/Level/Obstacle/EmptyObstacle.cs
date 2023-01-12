using System.Collections.Generic;

public class EmptyObstacle : Obstacle
{
    public void InitItems(List<ItemParent> items)
    {
        _items = items;
    }
}