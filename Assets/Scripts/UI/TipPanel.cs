using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 提示面板
/// </summary>
public class TipPanel : BasePanel
{
    //玩家
    private Player player;
    ////物品拾取信息位置
    //private Vector3 itemPos;
    ////物品拾取信息x
    //private float x;

    protected override void Awake()
    {
        base.Awake();
        player = GameObject.Find("Player").GetComponent<Player>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //x = Mathf.Lerp(x, 260, Time.deltaTime * 5);
        //itemPos.x = x;
        //GetControl<Image>("Item").rectTransform.localPosition = itemPos;
    }

    protected override void ClickBtn(string btnName)
    {
        MusicMgr.Instance.PlaySound("Button");
        switch (btnName)
        {
            //返回按钮
            case "btnReturn":
                UIMgr.Instance.HidePanel<TipPanel>();
                //切回开始场景
                if (GetControl<TextMeshProUGUI>("txtBtn").text == "返回主菜单")
                {
                    MusicMgr.Instance.ClearSound();
                    PoolMgr.Instance.ClearPool();
                    InventoryMgr.Instance.Clear();
                    SceneManager.LoadScene("BeginScene");
                    UIMgr.Instance.HidePanel<GamePanel>(true);
                    UIMgr.Instance.HidePanel<InteractionPanel>(true);
                    UIMgr.Instance.HidePanel<TipPanel>(true);
                    UIMgr.Instance.HidePanel<BagItemPanel>(true);
                    UIMgr.Instance.HidePanel<BagPanel>(true);
                    UIMgr.Instance.HidePanel<ItemPanel>(true);
                    Main.Instance.InitBeginScene();
                }
                else if (GetControl<TextMeshProUGUI>("txtBtn").text == "确定")
                {
                    if (GetControl<TextMeshProUGUI>("txtTip").text == "需要钥匙")
                    {
                        
                    }
                    else if (GetControl<TextMeshProUGUI>("txtTip").text == "大门开启")
                    {
                        //door.OpenDoor();
                    }
                    else if (GetControl<TextMeshProUGUI>("txtTip").text == "获得钥匙")
                    {
                        //BagPanel.Instance.inventory.AddItem(k.KeyItem);
                        //key.door.UnLock();
                        //Destroy(key.gameObject);
                    }
                    else if (GetControl<TextMeshProUGUI>("txtTip").text == "获得长剑")
                    {
                        //BagPanel.Instance.inventory.AddItem(w.weaponItem);
                        //weapon.WearWeapon();
                        //Destroy(weapon.gameObject);
                    }
                    else if (GetControl<TextMeshProUGUI>("txtTip").text == "获得血包")
                    {
                        
                        //player.AddHp();
                        //Destroy(hp.gameObject);
                    }
                    //BagPanel.Instance.UpdateBag();
                }
                else if (GetControl<TextMeshProUGUI>("txtBtn").text == "游戏继续")
                {
                    Time.timeScale = 1f;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                player.canControl = true;
                UIMgr.Instance.GetPanel<GamePanel>((panel) =>
                {
                    panel.PauseStart(false);
                });
                
                break;
            //设置按钮
            case "btnSetting":
                UIMgr.Instance.HidePanel<TipPanel>();
                UIMgr.Instance.ShowPanel<SettingPanel>();
                break;
            //背包按钮
            case "btnBag":
                UIMgr.Instance.HidePanel<TipPanel>();
                UIMgr.Instance.ShowPanel<BagPanel>();
                break;
        }
    }
    /// <summary>
    /// 设置面板信息
    /// </summary>
    /// <param name="txtTip"></param>
    /// <param name="txtBtn"></param>
    /// <param name="settingActive"></param>
    /// <param name="bagActive"></param>
    public void SetInfo(string txtTip, string txtBtn, bool settingActive = true, bool bagActive = true)
    {
        GetControl<TextMeshProUGUI>("txtTip").text = txtTip;
        GetControl<TextMeshProUGUI>("txtBtn").text = txtBtn;
        GetControl<Button>("btnSetting").gameObject.SetActive(settingActive);
        GetControl<Button>("btnBag").gameObject.SetActive(bagActive);
    }

    public override void ShowMe()
    {
        //游戏暂停
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        player.canControl = false;

        ////初始化x
        //x = 780;
        ////初始化ui位置
        //itemPos = new Vector3(x, GetControl<Image>("Item").rectTransform.localPosition.y, GetControl<Image>("Item").rectTransform.localPosition.z);
        //GetControl<Image>("Item").rectTransform.localPosition = itemPos;
    }

    public override void HideMe()
    {
        
    }
}
