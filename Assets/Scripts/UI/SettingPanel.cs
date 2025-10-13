using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 设置面板
/// </summary>
public class SettingPanel : MonoBehaviour
{
    private static SettingPanel instance;
    public static SettingPanel Instance => instance;
    private SettingPanel() { }

    //音量滑动条
    public Slider sliderMusic;
    //音效滑动条
    public Slider sliderSound;
    //音量关闭
    public Button btnChooseMusic;
    //音量开启
    public Button btnChooseMusicBK;
    //音效关闭
    public Button btnChooseSound;
    //音效开启
    public Button btnChooseSoundBK;
    //界面关闭
    public Button btnClose;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //音量滑动条
        sliderMusic.onValueChanged.AddListener((v) =>
        {
            //按钮点击音效
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            //改变背景音乐大小
            MusicMgr.Instance.audioSource.volume = v;
        });
        //音效滑动条
        sliderSound.onValueChanged.AddListener((v) =>
        {
            //按钮点击音效
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
        });
        //音量关闭
        btnChooseMusic.onClick.AddListener(() =>
        {
            //按钮点击音效
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            //隐藏图标
            btnChooseMusic.gameObject.SetActive(false);
            //背景音乐禁音
            MusicMgr.Instance.audioSource.mute = true;
        });
        //音效关闭
        btnChooseSound.onClick.AddListener(() =>
        {
            //按钮点击音效
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            //显示图标
            btnChooseSound.gameObject.SetActive(false);
        });
        //音量开启
        btnChooseMusicBK.onClick.AddListener(() =>
        {
            //按钮点击音效
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            //显示图标
            btnChooseMusic.gameObject.SetActive(true);
            //取消禁音
            MusicMgr.Instance.audioSource.mute = false;
        });
        //音量关闭
        btnChooseSoundBK.onClick.AddListener(() =>
        {
            //按钮点击音效
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            //隐藏图标
            btnChooseSound.gameObject.SetActive(true);
        });
        //界面关闭
        btnClose.onClick.AddListener(() =>
        {
            //按钮点击音效
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            //隐藏
            Hide();
            //保存音量数据
            DataMgr.Instance.SaveMusic(!MusicMgr.Instance.audioSource.mute, MusicMgr.Instance.audioSource.volume);
            //保存音效数据
            DataMgr.Instance.SaveSound(btnChooseSound.gameObject.activeSelf, sliderSound.value);
        });
        //初始化音量滑动条位置
        sliderMusic.SetValueWithoutNotify(MusicMgr.Instance.audioSource.volume);
        //初始化音量开关图标显隐
        btnChooseMusic.gameObject.SetActive(!MusicMgr.Instance.audioSource.mute);
        //初始化音效滑动条位置
        sliderSound.SetValueWithoutNotify(DataMgr.Instance.musicData.soundVolume);
        //初始化音效开关图标显隐
        btnChooseSound.gameObject.SetActive(DataMgr.Instance.musicData.soundOpen);
        //初始隐藏
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 显示面板
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// 隐藏面板
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
