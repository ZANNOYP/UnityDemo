using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 主菜单开始面板
/// </summary>
public class BeginPanel : MonoBehaviour
{
    private static BeginPanel instance;
    public static BeginPanel Instance => instance;
    private BeginPanel() { }
    //开始游戏按钮
    public Button btnStart;
    //设置按钮
    public Button btnSetting;
    //退出游戏按钮
    public Button btnQuit;

    private void Awake()
    {
        //时间开启
        Time.timeScale = 1;
        instance = this;
        //鼠标解锁
        Cursor.lockState = CursorLockMode.None;
    }
    // Start is called before the first frame update
    void Start()
    {
        //开始游戏切换场景
        btnStart.onClick.AddListener(() =>
        {
            //按钮点击音效
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            SceneManager.LoadScene("GameScene");
        });
        //打开设置面板
        btnSetting.onClick.AddListener(() =>
        {
            //按钮点击音效
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            SettingPanel.Instance.Show();
        });
        //退出游戏关闭游戏
        btnQuit.onClick.AddListener(() =>
        {
            //按钮点击音效
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif

        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
