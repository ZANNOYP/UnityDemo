using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    Weapon,
    Key,
    Hp,
}

public class BaseItem : ScriptableObject
{
    public int id;
    public string itemName;
    public Sprite sprite;
    public ItemType itemType;

    //public BaseItem(int id, string name, Sprite sprite, ItemType itemType, int count)
    //{
    //    this.id = id;
    //    this.itemName = name;
    //    this.sprite = sprite;
    //    this.itemType = itemType;
    //    this.count = count;
    //}
}
