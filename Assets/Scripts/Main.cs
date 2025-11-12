using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 主入口
/// </summary>
public class Main : SingletonMono<Main>
{
    /// <summary>
    /// 初始化开始场景
    /// </summary>
    public void InitBeginScene()
    {
        //显示开始界面
        UIMgr.Instance.ShowPanel<BeginPanel>(E_UILayer.Bottom, (panel) =>
        {
            //panel.GetControl<Image>("imgBK").gameObject.SetActive(true);
            //SceneManager.LoadScene("GameScene2", LoadSceneMode.Additive);
            //打开定时器管理器
            TimerMgr.Instance.Start();
            //TimerMgr.Instance.CreateTimer(false, 1000, () =>
            //{
            //    panel.GetControl<Image>("imgBK").gameObject.SetActive(false);
            //});
            //播放背景音乐
            MusicMgr.Instance.PlayBKMusic("Begin");
        });
    }
    // Start is called before the first frame update
    void Start()
    {
        InitBeginScene();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
