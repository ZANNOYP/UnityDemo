using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BeginPanel : MonoBehaviour
{
    private static BeginPanel instance;
    public static BeginPanel Instance => instance;
    private BeginPanel() { }
    //开始游戏按钮
    public Button btnStart;
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
            SceneManager.LoadScene("GameScene");
        });
        //退出游戏关闭游戏
        btnQuit.onClick.AddListener(() =>
        {
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
