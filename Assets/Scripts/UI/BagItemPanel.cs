using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 背包物品面板
/// </summary>
public class BagItemPanel : BasePanel
{
    //玩家
    private Player player;
    //武器
    private Weapon weapon;
    //面板绑定的物品信息
    private ItemInstance currentItem;
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    protected override void ClickBtn(string btnName)
    {
        //使用、装备按钮
        if (btnName == "btnUse")
        {
            //回血
            if (currentItem.item.itemType == ItemType.Hp)
            {
                AddHp();
            }
            //装备武器
            else if (currentItem.item.itemType == ItemType.Weapon)
            {
                WearWeapon();
            }
            //开门
            else if (currentItem.item.itemType == ItemType.Key)
            {
                UnLock();
            }
            //隐藏背包物品面板
            UIMgr.Instance.HidePanel<BagItemPanel>();
            //隐藏背包面板
            UIMgr.Instance.HidePanel<BagPanel>();
            player.canControl = true;
            UIMgr.Instance.GetPanel<GamePanel>((panel) =>
            {
                panel.PauseStart(false);
            });
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }
        //返回按钮
        else if (btnName == "btnReturn")
        {
            UIMgr.Instance.HidePanel<BagItemPanel>();
        }
    }
    //将物品信息保存下来
    public void SetItem(ItemInstance item)
    {
        currentItem = item;

        string btnText = item.item.itemType == ItemType.Hp ? "使用" :
                         item.item.itemType == ItemType.Key ? "使用" :
                         item.item.itemType == ItemType.Weapon ? "装备" : "未知";
        GetControl<TextMeshProUGUI>("txtUse").text = btnText;
    }
    /// <summary>
    /// 回血、将物品移除背包列表容器
    /// </summary>
    private void AddHp()
    {
        player.AddHp((currentItem.item as HpItem).addHp);
        InventoryMgr.Instance.RemoveItem(currentItem.item);
    }
    /// <summary>
    /// 装备武器、将物品移除背包列表容器
    /// </summary>
    private void WearWeapon()
    {
        player.AddAtk((currentItem.item as WeaponItem).atk);
        GameObject weaponObj = Instantiate(Resources.Load<GameObject>("Weapon"));
        weapon = weaponObj.GetComponent<Weapon>();
        weapon.weaponLeftPos = GameObject.Find("shield").GetComponent<Transform>();
        weapon.weaponRightPos = GameObject.Find("weapon").GetComponent<Transform>();
        if (weapon.weaponLeftPos != null)
        {
            weapon.weaponLeft.SetParent(weapon.weaponLeftPos);
            weapon.weaponLeft.localPosition = Vector3.zero;
            weapon.weaponLeft.localEulerAngles = Vector3.zero;
        }
        weapon.weaponRight.SetParent(weapon.weaponRightPos);
        weapon.weaponRight.localPosition = Vector3.zero;
        weapon.weaponRight.localEulerAngles = Vector3.zero;
        Destroy(weaponObj);
        //改变玩家攻击类型
        player.atkType = AtkType.ShortSword;
        player.animator.SetLayerWeight(1, 1);

        InventoryMgr.Instance.RemoveItem(currentItem.item);
    }
    /// <summary>
    /// 解锁大门、将物品移除背包列表容器
    /// </summary>
    private void UnLock()
    {
        //如果玩家接触的门与存储的钥匙上的门id相同则打开门
        if (player.door.doorID == (currentItem.item as KeyItem).doorID)
        {
            player.door.OpenDoor();
            InventoryMgr.Instance.RemoveItem(currentItem.item);
        }
    }

    public override void HideMe()
    {
        
    }

    public override void ShowMe()
    {
        
    }

    
}
