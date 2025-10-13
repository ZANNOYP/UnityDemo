using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �������
/// </summary>
public class SettingPanel : MonoBehaviour
{
    private static SettingPanel instance;
    public static SettingPanel Instance => instance;
    private SettingPanel() { }

    //����������
    public Slider sliderMusic;
    //��Ч������
    public Slider sliderSound;
    //�����ر�
    public Button btnChooseMusic;
    //��������
    public Button btnChooseMusicBK;
    //��Ч�ر�
    public Button btnChooseSound;
    //��Ч����
    public Button btnChooseSoundBK;
    //����ر�
    public Button btnClose;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //����������
        sliderMusic.onValueChanged.AddListener((v) =>
        {
            //��ť�����Ч
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            //�ı䱳�����ִ�С
            MusicMgr.Instance.audioSource.volume = v;
        });
        //��Ч������
        sliderSound.onValueChanged.AddListener((v) =>
        {
            //��ť�����Ч
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
        });
        //�����ر�
        btnChooseMusic.onClick.AddListener(() =>
        {
            //��ť�����Ч
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            //����ͼ��
            btnChooseMusic.gameObject.SetActive(false);
            //�������ֽ���
            MusicMgr.Instance.audioSource.mute = true;
        });
        //��Ч�ر�
        btnChooseSound.onClick.AddListener(() =>
        {
            //��ť�����Ч
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            //��ʾͼ��
            btnChooseSound.gameObject.SetActive(false);
        });
        //��������
        btnChooseMusicBK.onClick.AddListener(() =>
        {
            //��ť�����Ч
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            //��ʾͼ��
            btnChooseMusic.gameObject.SetActive(true);
            //ȡ������
            MusicMgr.Instance.audioSource.mute = false;
        });
        //�����ر�
        btnChooseSoundBK.onClick.AddListener(() =>
        {
            //��ť�����Ч
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            //����ͼ��
            btnChooseSound.gameObject.SetActive(true);
        });
        //����ر�
        btnClose.onClick.AddListener(() =>
        {
            //��ť�����Ч
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            //����
            Hide();
            //������������
            DataMgr.Instance.SaveMusic(!MusicMgr.Instance.audioSource.mute, MusicMgr.Instance.audioSource.volume);
            //������Ч����
            DataMgr.Instance.SaveSound(btnChooseSound.gameObject.activeSelf, sliderSound.value);
        });
        //��ʼ������������λ��
        sliderMusic.SetValueWithoutNotify(MusicMgr.Instance.audioSource.volume);
        //��ʼ����������ͼ������
        btnChooseMusic.gameObject.SetActive(!MusicMgr.Instance.audioSource.mute);
        //��ʼ����Ч������λ��
        sliderSound.SetValueWithoutNotify(DataMgr.Instance.musicData.soundVolume);
        //��ʼ����Ч����ͼ������
        btnChooseSound.gameObject.SetActive(DataMgr.Instance.musicData.soundOpen);
        //��ʼ����
        Hide();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// ��ʾ���
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }
    /// <summary>
    /// �������
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
