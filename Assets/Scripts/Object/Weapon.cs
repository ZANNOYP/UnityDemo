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
            //设置图片
            panel.GetControl<Image>("imgItem").sprite = weaponItem.sprite;
            //设置名字
            panel.GetControl<TextMeshProUGUI>("txtItemName").text = weaponItem.itemName;
            //设置数量
            panel.GetControl<TextMeshProUGUI>("txtItemNum").text = "x" + count;
            //将物品添加至背包列表容器
            InventoryMgr.Instance.AddItem(weaponItem, count);
            //销毁自己
            Destroy(this.gameObject);

            //2s后隐藏
            TimerMgr.Instance.CreateTimer(false, 2000, () =>
            {
                UIMgr.Instance.HidePanel<ItemPanel>();
            });
        });
    }
}
