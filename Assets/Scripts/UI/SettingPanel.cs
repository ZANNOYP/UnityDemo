using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 设置面板
/// </summary>
public class SettingPanel : BasePanel
{
    protected override void Awake()
    {
        base.Awake();

    }
    /// <summary>
    /// 按钮点击委托
    /// </summary>
    /// <param name="btnName"></param>
    protected override void ClickBtn(string btnName)
    {
        //按钮音效
        MusicMgr.Instance.PlaySound("Button");
        switch (btnName)
        {
            //关闭按钮 隐藏自己 显示开始界面
            case "btnClose":
                UIMgr.Instance.HidePanel<SettingPanel>();
                if (SceneManager.GetActiveScene().name == "GameScene") 
                    UIMgr.Instance.ShowPanel<TipPanel>();
                break;
        }
    }
    /// <summary>
    /// 滑动条值变化委托
    /// </summary>
    /// <param name="sliderName"></param>
    /// <param name="value"></param>
    protected override void SliderValueChange(string sliderName, float value)
    {
        //按钮音效
        MusicMgr.Instance.PlaySound("Button");
        //如果是背景音乐的滑动条 改变滑动条时 也同时改变音乐管理器里存储的音量大小
        if (sliderName == "sliderMusic")
        {
            MusicMgr.Instance.ChangeBKMusicValue(value);
        }
        //如果是音效的滑动条 则改变音乐管理器里 音效大小
        else if (sliderName == "sliderSound")
        {
            MusicMgr.Instance.ChangeSoundValue(value);
        }

    }
    /// <summary>
    /// 多选框值改变委托
    /// </summary>
    /// <param name="toggleName"></param>
    /// <param name="value"></param>
    protected override void ToggleValueChange(string toggleName, bool value)
    {
        //按钮音效
        MusicMgr.Instance.PlaySound("Button");
        //如果是音乐开关 改变音乐管理器里 存储的背景音乐是否开启
        if (toggleName == "togMusic")
        {
            MusicMgr.Instance.ChangeBKMusic(value);
            //根据多选框的值 决定播放或暂停音乐
            if (value)
            {
                if (SceneManager.GetActiveScene().name == "BeginScene")
                    MusicMgr.Instance.PlayBKMusic("Begin");
                else
                    MusicMgr.Instance.PlayBKMusic("Game");
            }
            else
                MusicMgr.Instance.PauseBKMusic();
        }
        //音效开关 则播放或暂停音效
        else if (toggleName == "togSound")
        {
            MusicMgr.Instance.PlayOrPauseSound(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    

    public override void ShowMe()
    {
        //显示设置面板时 初始化滑动条、多选框状态
        MusicData musicData = MusicMgr.Instance.GetData();
        GetControl<Toggle>("togMusic").SetIsOnWithoutNotify(musicData.musicOpen);
        GetControl<Toggle>("togSound").SetIsOnWithoutNotify(musicData.soundOpen);
        GetControl<Slider>("sliderMusic").SetValueWithoutNotify(musicData.musicVolume);
        GetControl<Slider>("sliderSound").SetValueWithoutNotify(musicData.soundVolume);
    }

    public override void HideMe()
    {
        //隐藏面板时 将音乐管理器中 的数据存储到本地
        MusicData musicData = MusicMgr.Instance.GetData();
        DataMgr.Instance.SaveMusic(musicData);
    }
}
