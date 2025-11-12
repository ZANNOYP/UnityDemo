using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/// <summary>
/// 游戏界面
/// </summary>
public class GamePanel : BasePanel
{
    //血条位置偏移
    public Vector3 offset;
    //当前屏幕上怪物血条应处位置
    [SerializeField]
    private Vector3 localPos;

    //玩家
    public Player player;
    //怪物
    public Monster monster;
    //通关点
    public CheckPoint checkPoint;
    //是否暂停
    private bool isPause;
    //隐藏怪物血条协程
    private Coroutine hideMonsterHp;

    protected override void Awake()
    {
        base.Awake();
        monster = GameObject.Find("Monster").GetComponent<Monster>();
        //添加玩家血条变化委托
        EventCenter.Instance.AddEventListener<float>(E_EventType.E_Player_HpChange, ChangePlayerHp);
        //添加怪物血条变化委托
        //EventCenter.Instance.AddEventListener<float>(E_EventType.E_Monster_HpChange, ChangeMonsterHp);
        //添加开启暂停菜单委托
        EventCenter.Instance.AddEventListener(E_EventType.E_Pause_Menu, PauseMenu);
        //打开输入管理器
        InputMgr.Instance.StartOrCloseInputMgr(true);
        //添加打开暂停界面按键信息
        InputMgr.Instance.ChangeKeyboardInfo(E_EventType.E_Pause_Menu, KeyCode.Escape, InputInfo.E_InputType.Down);
        //默认不暂停
        isPause = false;

    }

    // Start is called before the first frame update
    void Start()
    {
        //鼠标锁定
        Cursor.lockState = CursorLockMode.Locked;
        
    }
    /// <summary>
    /// 改变玩家血条委托
    /// </summary>
    /// <param name="hp"></param>
    private void ChangePlayerHp(float hp)
    {
        GetControl<Slider>("sliderHp").value = hp;
    }
    /// <summary>
    /// 改变怪物血条委托
    /// </summary>
    /// <param name="hp"></param>
    private void ChangeMonsterHp(float hp)
    {
        Slider sliderMonsterHp = GetControl<Slider>("sliderMonsterHp");
        sliderMonsterHp.value = hp;
        sliderMonsterHp.gameObject.SetActive(true);
        //开启怪物血条延迟隐藏协程
        if (hideMonsterHp != null) 
            StopCoroutine(hideMonsterHp);
        hideMonsterHp = StartCoroutine(HideMonsterHp(2f));

    }
    /// <summary>
    /// 打开暂停界面委托
    /// </summary>
    private void PauseMenu()
    {
        if (!isPause)
        {
            UIMgr.Instance.ShowPanel<TipPanel>(E_UILayer.Middle, (panel) =>
            {
                panel.SetInfo("游戏暂停", "游戏继续");
                PauseStart(true);
            });
        }
    }
    /// <summary>
    /// 暂停开始游戏
    /// </summary>
    /// <param name="isPause"></param>
    public void PauseStart(bool isPause)
    {
        this.isPause = isPause;
    }

    private void OnDestroy()
    {
        //删除玩家血条变化委托
        EventCenter.Instance.RemoveEventListener<float>(E_EventType.E_Player_HpChange, ChangePlayerHp);
        //删除怪物血条变化委托
        //EventCenter.Instance.RemoveEventListener<float>(E_EventType.E_Monster_HpChange, ChangeMonsterHp);
        //删除开启暂停菜单委托
        EventCenter.Instance.RemoveEventListener(E_EventType.E_Pause_Menu, PauseMenu);
    }

    // Update is called once per frame
    void Update()
    {

        if (monster == null)
            return;
        //怪物血条跟随
        Vector3 screenPos = Camera.main.WorldToScreenPoint(monster.transform.position + offset);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            UIMgr.Instance.uiCanvas.transform as RectTransform,
            screenPos,
            UIMgr.Instance.uiCanvas.worldCamera,
            out Vector2 localPos);
        (GetControl<Slider>("sliderMonsterHp").transform as RectTransform).localPosition = localPos;

        //}
        //按F12清空存档
        //if (Input.GetKeyDown(KeyCode.T)) 
        //{
        //    PlayerPrefs.DeleteAll();
        //    PlayerPrefs.Save();
        //    Debug.Log("PlayerPrefs 已清空");
        //}
    }

    /// <summary>
    /// 怪物血条延迟隐藏协程
    /// </summary>
    /// <param name="sec"></param>
    /// <returns></returns>
    private IEnumerator HideMonsterHp(float sec)
    {
        yield return new WaitForSeconds(sec);
        GetControl<Slider>("sliderMonsterHp").gameObject.SetActive(false);
    }

    public override void ShowMe()
    {
        //隐藏怪物血条
        GetControl<Slider>("sliderMonsterHp").gameObject.SetActive(false);
        EventCenter.Instance.EventTrigger<float>(E_EventType.E_Player_HpChange, 1);
    }

    public override void HideMe()
    {
        
    }
}
