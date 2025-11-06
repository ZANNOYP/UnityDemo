using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 背包面板
/// </summary>
public class BagPanel : BasePanel
{
    //玩家
    private Player player;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    protected override void ClickBtn(string btnName)
    {
        //关闭按钮
        if (btnName == "btnClose")
        {
            UIMgr.Instance.HidePanel<BagPanel>();
            //如果玩家靠近门 取消暂停
            if (player.door != null)
            {
                player.canControl = true;
                UIMgr.Instance.GetPanel<GamePanel>((panel) =>
                {
                    panel.PauseStart(false);
                });
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
            }
            //如果玩家原理门 打开暂停面板
            else
            {
                UIMgr.Instance.ShowPanel<TipPanel>(E_UILayer.Middle, (panel) =>
                {
                    panel.GetControl<TextMeshProUGUI>("txtTip").text = "游戏暂停";
                    panel.GetControl<TextMeshProUGUI>("txtBtn").text = "游戏继续";
                    panel.GetControl<Button>("btnSetting").gameObject.SetActive(true);
                    panel.GetControl<Button>("btnBag").gameObject.SetActive(true);
                });
            }
            
        }
        //物品图标按钮
        else if (btnName == "Item")
            //打开背包物品面板
            UIMgr.Instance.ShowPanel<BagItemPanel>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //更新背包
    public void UpdateBag()
    {
        //获取背包ScrollView的content
        RectTransform content = GetControl<ScrollRect>("ScrollViewBag").content;
        //遍历content 清空所有物品图片和按钮事件，之后回收所有对象
        for (int i = content.childCount - 1; i >= 0; i--)
        {
            Transform child = content.GetChild(i);
            GameObject item = child.gameObject;
            Image image = item.GetComponent<Image>();
            image.sprite = null;
            Button btn = item.GetComponent<Button>();
            btn.onClick.RemoveAllListeners();
            PoolMgr.Instance.PushObj(child.gameObject);
        }

        //得到背包列表容器
        List<ItemInstance> items = InventoryMgr.Instance.GetAllItems();
        //遍历背包列表容器
        for (int i = 0; i < items.Count; i++) 
        {
            //取物品UI
            GameObject item = PoolMgr.Instance.GetObj("UI/Item");
            Image image = item.GetComponent<Image>();
            //设置图片
            image.sprite = items[i].item.sprite;
            Button btn = item.GetComponent<Button>();
            //得到当前物品信息
            ItemInstance currentItem = items[i];
            //添加事件
            btn.onClick.AddListener(() =>
            {
                //可以使用按钮时打开背包物品面板
                if (currentItem.CanUse(player))
                {
                    UIMgr.Instance.ShowPanel<BagItemPanel>(E_UILayer.Top, (panel) =>
                    {
                        //设置背包物品面板信息
                        panel.SetItem(currentItem);
                        //将按钮放到鼠标位置
                        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                            UIMgr.Instance.uiCanvas.transform as RectTransform,
                            Input.mousePosition,
                            UIMgr.Instance.uiCanvas.worldCamera,
                            out Vector2 localPos);
                        RectTransform rs = (panel.GetControl<Button>("btnUse").transform as RectTransform);
                        rs.localPosition = localPos - new Vector2(0, rs.sizeDelta.y);
                    });
                }
                
            });
            //修改物品右下角数量信息
            TextMeshProUGUI txtNum = item.GetComponentInChildren<TextMeshProUGUI>();
            txtNum.text = items[i].count.ToString();
            //设置物品位置
            RectTransform rt = item.GetComponent<RectTransform>();
            rt.SetParent(content, false);
            rt.anchoredPosition = new Vector2(-285 + ((i % 4) * 190), 490 - ((i / 4) * 190));
            rt.localScale = Vector3.one;
            rt.localRotation = Quaternion.identity;
        }
    }

    public override void ShowMe()
    {
        UpdateBag();
    }

    public override void HideMe()
    {
        
    }
}
