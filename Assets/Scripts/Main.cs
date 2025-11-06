using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 主入口
/// </summary>
public class Main : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //过场景不销毁
        DontDestroyOnLoad(this.gameObject);
        //显示开始界面
        UIMgr.Instance.ShowPanel<BeginPanel>(E_UILayer.Bottom);
        //播放背景音乐
        MusicMgr.Instance.PlayBKMusic("Begin");
        //打开定时器管理器
        TimerMgr.Instance.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
