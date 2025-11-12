using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 背包列表管理器
/// </summary>
public class InventoryMgr : BaseManager<InventoryMgr>
{
    private List<ItemInstance> itemsList = new List<ItemInstance>();
    private List<ItemInstance> delItemsList = new List<ItemInstance>();
    public int Count => itemsList.Count;

    private InventoryMgr() { }
    /// <summary>
    /// 添加物品信息
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    public void AddItem(BaseItem item, int count = 1)
    {
        //背包遍历 有一样的且不是武器 则叠加
        for (int i = 0; i < itemsList.Count; i++)
        {
            if (itemsList[i].item.id == item.id && itemsList[i].item.itemType != ItemType.Weapon)  
            {
                itemsList[i].count += count;
                return;
            }
        }
        //背包没找到相同物品直接添加
        ItemInstance itemInstance = new ItemInstance(item, count);
        itemsList.Add(itemInstance);
    }
    /// <summary>
    /// 移除物品信息
    /// </summary>
    /// <param name="item"></param>
    /// <param name="count"></param>
    public void RemoveItem(BaseItem item, int count = 1)
    {
        //遍历列表
        for (int i = 0; i < itemsList.Count; i++)
        {
            //id相同数量-1
            if (itemsList[i].item.id == item.id)
            {
                itemsList[i].count -= count;
                //数量小于等于0时放入待删除列表
                if (itemsList[i].count <= 0)
                {
                    delItemsList.Add(itemsList[i]);
                }
            }
        }
        //遍历待删除列表
        for (int i = 0; i < delItemsList.Count; i++)
        {
            //删除需要删除的物品信息
            itemsList.Remove(delItemsList[i]);
        }
        //清空待删除列表
        delItemsList.Clear();
    }
    /// <summary>
    /// 获取背包列表容器
    /// </summary>
    /// <returns></returns>
    public List<ItemInstance> GetAllItems()
    {
        return itemsList;
    }

    public void Clear()
    {
        itemsList.Clear();
        delItemsList.Clear();
    }
  
}
