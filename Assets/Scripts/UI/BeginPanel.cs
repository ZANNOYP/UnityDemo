using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 主菜单开始面板
/// </summary>
public class BeginPanel : BasePanel
{
    private CanvasGroup canvasGroup;
    private bool isShow;

    //重写按钮点击逻辑
    protected override void ClickBtn(string btnName)
    {
        //按钮音效
        MusicMgr.Instance.PlaySound("Button");
        switch (btnName)
        {
            //开始游戏按钮
            case "btnStart":
                //隐藏开始界面(销毁)
                UIMgr.Instance.HidePanel<BeginPanel>(true);
                //显示加载界面
                UIMgr.Instance.ShowPanel<LoadingPanel>();
                //异步加载场景
                SceneMgr.Instance.LoadSceneAsyn("GameScene",()=>
                {
                    //加载场景结束后 销毁加载界面 显示游戏界面 切换游戏背景音乐
                    UIMgr.Instance.HidePanel<LoadingPanel>(true);
                    UIMgr.Instance.ShowPanel<GamePanel>(E_UILayer.Bottom);
                    MusicMgr.Instance.PlayBKMusic("Game");
                });
                break;
            //设置按钮
            case "btnSetting":
                //显示设置界面
                UIMgr.Instance.ShowPanel<SettingPanel>(E_UILayer.Top);
                break;
            //退出按钮
            case "btnQuit":
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
                break;
        }
    }


    //重写awake
    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponent<CanvasGroup>();
        isShow = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isShow)
        {
            canvasGroup.alpha += Time.deltaTime;
            if (canvasGroup.alpha > 1) 
                canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.alpha -= Time.deltaTime;
            if (canvasGroup.alpha < 0)
                canvasGroup.alpha = 0;
        }
    }

    public override void ShowMe()
    {
        //时间开启
        Time.timeScale = 1;
        //鼠标解锁
        Cursor.lockState = CursorLockMode.None;

        canvasGroup.alpha = 0;
        isShow = true;
    }

    public override void HideMe()
    {
        canvasGroup.alpha = 1;
        isShow = false;
    }
}
