using System.Collections.Generic;

public class ItemManager : Singleton<ItemManager>
{

    public List<Item> CreateItems(List<ItemType> itemTypes)
    {
        List<Item> items = new();
        for (int i = 0; i < itemTypes.Count; i++)
        {
            items.Add(CreateItem(itemTypes[i]));
        }
        return items;
    }

    public Item CreateItem(ItemType type)
    {
        ItemScriptable itemScriptable = ResourceSystem.instance.GetItem(type);
        Item newItem = itemScriptable.CreateItemInstance();
        return newItem;
    }



}