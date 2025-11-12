using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 武器类
/// </summary>
public class Weapon : MonoBehaviour
{
    //旋转速度
    public float roundSpeed = 100f;
    //玩家左手武器位置
    public Transform weaponLeftPos;
    //玩家右手武器位置
    public Transform weaponRightPos;
    //左手武器
    public Transform weaponLeft;
    //右手武器
    public Transform weaponRight;
    //武器信息
    public WeaponItem weaponItem;
    //武器数量
    public int count;

    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    // Update is called once per frame
    void Update()
    {
        //自转
        this.transform.Rotate(Vector3.up, roundSpeed * Time.deltaTime);
    }
    //拾取武器
    public void PickUp()
    {
        //隐藏交互面板
        UIMgr.Instance.HidePanel<InteractionPanel>();
        //显示物品拾取信息面板
        UIMgr.Instance.ShowPanel<ItemPanel>(E_UILayer.Bottom, (panel) =>
        {
            //改变面板信息
            panel.SetInfo(weaponItem.sprite, weaponItem.itemName, count);
            //将物品添加至背包列表容器
            InventoryMgr.Instance.AddItem(weaponItem, count);
            //销毁自己
            Destroy(this.gameObject);
        });
    }

    public void WearWeapon()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        weaponLeftPos = GameObject.Find("shield").GetComponent<Transform>();
        weaponRightPos = GameObject.Find("weapon").GetComponent<Transform>();
        if (weaponLeftPos != null)
        {
            weaponLeft.SetParent(weaponLeftPos);
            weaponLeft.localPosition = Vector3.zero;
            weaponLeft.localEulerAngles = Vector3.zero;
        }
        weaponRight.SetParent(weaponRightPos);
        weaponRight.localPosition = Vector3.zero;
        weaponRight.localEulerAngles = Vector3.zero;
        Destroy(this.gameObject);
        //改变玩家攻击类型
        player.atkType = AtkType.ShortSword;
        player.animator.SetLayerWeight(1, 1);
    }
}
