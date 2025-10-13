using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// ���˵���ʼ���
/// </summary>
public class BeginPanel : MonoBehaviour
{
    private static BeginPanel instance;
    public static BeginPanel Instance => instance;
    private BeginPanel() { }
    //��ʼ��Ϸ��ť
    public Button btnStart;
    //���ð�ť
    public Button btnSetting;
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
            //��ť�����Ч
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            SceneManager.LoadScene("GameScene");
        });
        //���������
        btnSetting.onClick.AddListener(() =>
        {
            //��ť�����Ч
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            SettingPanel.Instance.Show();
        });
        //�˳���Ϸ�ر���Ϸ
        btnQuit.onClick.AddListener(() =>
        {
            //��ť�����Ч
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
