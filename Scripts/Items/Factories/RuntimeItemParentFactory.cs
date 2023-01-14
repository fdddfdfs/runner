using Unity.VisualScripting;

public class RuntimeItemParentFactory : AbstractFactory<RuntimeItemParent>
{
    private readonly AbstractFactory<Item> _factory;
    
    public RuntimeItemParentFactory(AbstractFactory<Item> factory)
    {
        _factory = factory;
    }
    
    public override RuntimeItemParent CreateItem()
    {
        Item item = _factory.CreateItem();
        var itemParent = item.AddComponent<RuntimeItemParent>();
        itemParent.EnterObstacle();
        itemParent.ItemObject = item;

        return itemParent;
    }
}