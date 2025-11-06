using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstance
{
    public BaseItem item;
    public int count;

    public ItemInstance(BaseItem item, int count)
    {
        this.item = item;
        this.count = count;
    }

    public bool CanUse(Player player)
    {
        if (item.itemType == ItemType.Key)
        {
            if (player.openDoor) 
                return true;
            else
                return false;
        }
        return true;
    }

}
