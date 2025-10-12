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
    //��ʼ��Ϸ��ť
    public Button btnStart;
    //�˳���Ϸ��ť
    public Button btnQuit;

    private void Awake()
    {
        //ʱ�俪��
        Time.timeScale = 1;
        instance = this;
        //������
        Cursor.lockState = CursorLockMode.None;
    }
    // Start is called before the first frame update
    void Start()
    {
        //��ʼ��Ϸ�л�����
        btnStart.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GameScene");
        });
        //�˳���Ϸ�ر���Ϸ
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
