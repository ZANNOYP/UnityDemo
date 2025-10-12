using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 提示面板
/// </summary>
public class TipPanel : MonoBehaviour
{
    private static TipPanel instance;
    public static TipPanel Instance => instance;
    private TipPanel() { }

    //提示信息
    public Text txtTip;
    //按钮文字信息
    public Text txtBtn;
    //按钮
    public Button btnReturn;

    //玩家
    public Player player;
    //通关点
    public CheckPoint checkPoint;
    //交互的门
    private Door door;
    //交互的钥匙
    private Key k;

    private void Awake()
    {
        instance = this;
        //注册角色死亡
        player.actionDead += PlayerDead;
        //注册游戏通关
        checkPoint.actionPass += Pass;
        //注册玩家开门
        player.actionOpendoorTip += OpenTip;
        //注册暂停
        GamePanel.Instance.actionPause += Pause;
        //注册拾取物品
        player.actionPick += Pick;
    }
    // Start is called before the first frame update
    void Start()
    {
        btnReturn.onClick.AddListener(() =>
        {

            if (txtBtn.text == "返回主菜单")
            {
                SceneManager.LoadScene("BeginScene");
            }
            else if (txtBtn.text == "确定")
            {
                if (txtTip.text == "需要钥匙")
                {
                    player.interaction = "E开门";
                    player.actionInteraction?.Invoke(player.interaction);
                }
                if (txtTip.text == "大门开启")
                {
                    door.OpenDoor();
                }
                if (txtTip.text == "获得钥匙")
                {
                    k.door.UnLock();
                    Destroy(k.gameObject);
                }
            }
            else if (txtBtn.text == "游戏继续")
            {

            }
            
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            this.gameObject.SetActive(false);
            player.canControl = true;

        });
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 更新提示信息
    /// </summary>
    /// <param name="text"></param>
    /// <param name="txtBtn"></param>
    public void UpdateTip(string text, string txtBtn)
    {
        txtTip.text = text ;
        this.txtBtn.text = txtBtn;
    }
    /// <summary>
    /// 玩家死亡委托
    /// </summary>
    public void PlayerDead()
    {
        UpdateTip("玩家死亡 游戏结束", "返回主菜单");
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 通关委托
    /// </summary>
    public void Pass()
    {
        UpdateTip("恭喜过关 游戏结束", "返回主菜单");
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 开门提示委托
    /// </summary>
    /// <param name="door">要开的门</param>
    public void OpenTip(Door door)
    {
        //记录要开的门
        this.door = door;
        //判断门是否上锁
        if (door.isLock)
        {
            UpdateTip("需要钥匙", "确定");
        }
        else
        {
            UpdateTip("大门开启", "确定");
        }
        //隐藏交互面板
        player.actionInteraction2?.Invoke();
        gameObject.SetActive(true);
        //打开面板时无法攻击与互动
        player.canControl = false;
    }
    /// <summary>
    /// 暂停提示委托
    /// </summary>
    public void Pause()
    {
        UpdateTip("游戏暂停", "游戏继续");
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 拾取提示委托
    /// </summary>
    /// <param name="k"></param>
    public void Pick(Key k)
    {
        this.k = k;
        UpdateTip("获得钥匙", "确定");
        //隐藏交互面板
        player.actionInteraction2?.Invoke();
        gameObject.SetActive(true);
        //打开面板时无法攻击与互动
        player.canControl = false;
    }
}
