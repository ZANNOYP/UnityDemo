using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 血包
/// </summary>
public class Hp : MonoBehaviour
{
    //血包旋转速度
    public float roundSpeed = 100f;
    //血包信息
    public HpItem hpItem;
    //数量
    public int count = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //自转
        transform.Rotate(Vector3.up, roundSpeed * Time.deltaTime);
    }
    //拾取血包
    public void PickUp()
    {
        //隐藏交互面板
        UIMgr.Instance.HidePanel<InteractionPanel>();
        //显示物品拾取信息面板
        UIMgr.Instance.ShowPanel<ItemPanel>(E_UILayer.Bottom, (panel) =>
        {
            //设置图片
            panel.GetControl<Image>("imgItem").sprite = hpItem.sprite;
            //设置名字
            panel.GetControl<TextMeshProUGUI>("txtItemName").text = hpItem.itemName;
            //设置数量
            panel.GetControl<TextMeshProUGUI>("txtItemNum").text = "x" + count;
            //将物品添加至背包列表容器
            InventoryMgr.Instance.AddItem(hpItem, count);
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
