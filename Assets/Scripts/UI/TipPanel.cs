using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// ��ʾ���
/// </summary>
public class TipPanel : MonoBehaviour
{
    private static TipPanel instance;
    public static TipPanel Instance => instance;
    private TipPanel() { }

    //��ʾ��Ϣ
    public Text txtTip;
    //��ť������Ϣ
    public Text txtBtn;
    //��ť
    public Button btnReturn;
    //���ð�ť
    public Button btnSetting;

    //���
    public Player player;
    //ͨ�ص�
    public CheckPoint checkPoint;
    //��������
    private Door door;
    //������Կ��
    private Key k;
    //����������
    private Weapon w;

    private void Awake()
    {
        instance = this;
        //ע���ɫ����
        player.actionDead += PlayerDead;
        //ע����Ϸͨ��
        checkPoint.actionPass += Pass;
        //ע����ҿ���
        player.actionOpendoorTip += OpenTip;
        //ע����ͣ
        GamePanel.Instance.actionPause += Pause;
        //ע��ʰȡ��Ʒ
        player.actionPickKey += PickKey;
        //ע��ʰȡ����
        player.actionPickWeapon += PickWeapon;
    }
    // Start is called before the first frame update
    void Start()
    {
        btnSetting.onClick.AddListener(() =>
        {
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            SettingPanel.Instance.Show();
        });
        btnReturn.onClick.AddListener(() =>
        {
            Instantiate(Resources.Load<GameObject>("Sound/btnSound"));
            if (txtBtn.text == "�������˵�")
            {
                SceneManager.LoadScene("BeginScene");
            }
            else if (txtBtn.text == "ȷ��")
            {
                if (txtTip.text == "��ҪԿ��")
                {
                    player.interaction = "E����";
                    player.actionInteraction?.Invoke(player.interaction);
                }
                if (txtTip.text == "���ſ���")
                {
                    door.OpenDoor();
                }
                if (txtTip.text == "���Կ��")
                {
                    k.door.UnLock();
                    Destroy(k.gameObject);
                }
                if (txtTip.text == "��ö̵�")
                {
                    w.WearWeapon();
                }
            }
            else if (txtBtn.text == "��Ϸ����")
            {
                
            }
            
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.Locked;
            this.gameObject.SetActive(false);
            player.canControl = true;

        });
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// ������ʾ��Ϣ
    /// </summary>
    /// <param name="text"></param>
    /// <param name="txtBtn"></param>
    public void UpdateTip(string text, string txtBtn)
    {
        txtTip.text = text ;
        this.txtBtn.text = txtBtn;
    }
    /// <summary>
    /// �������ί��
    /// </summary>
    public void PlayerDead()
    {
        UpdateTip("������� ��Ϸ����", "�������˵�");
        btnSetting.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
    /// <summary>
    /// ͨ��ί��
    /// </summary>
    public void Pass()
    {
        UpdateTip("��ϲ���� ��Ϸ����", "�������˵�");
        btnSetting.gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
    /// <summary>
    /// ������ʾί��
    /// </summary>
    /// <param name="door">Ҫ������</param>
    public void OpenTip(Door door)
    {
        //��¼Ҫ������
        this.door = door;
        //�ж����Ƿ�����
        if (door.isLock)
        {
            UpdateTip("��ҪԿ��", "ȷ��");
        }
        else
        {
            UpdateTip("���ſ���", "ȷ��");
        }
        btnSetting.gameObject.SetActive(false);
        //���ؽ������
        player.actionInteraction2?.Invoke();
        gameObject.SetActive(true);
        //�����ʱ�޷������뻥��
        player.canControl = false;
    }
    /// <summary>
    /// ��ͣ��ʾί��
    /// </summary>
    public void Pause()
    {
        UpdateTip("��Ϸ��ͣ", "��Ϸ����");
        btnSetting.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }
    /// <summary>
    /// ʰȡԿ����ʾί��
    /// </summary>
    /// <param name="k"></param>
    public void PickKey(Key k)
    {
        this.k = k;
        UpdateTip("���Կ��", "ȷ��");
        btnSetting.gameObject.SetActive(false);
        //���ؽ������
        player.actionInteraction2?.Invoke();
        gameObject.SetActive(true);
        //�����ʱ�޷������뻥��
        player.canControl = false;
    }
    /// <summary>
    /// ʰȡ������ʾί��
    /// </summary>
    /// <param name="w"></param>
    public void PickWeapon(Weapon w)
    {
        this.w = w;
        UpdateTip("��ö̵�", "ȷ��");
        btnSetting.gameObject.SetActive(false);
        //���ؽ������
        player.actionInteraction2?.Invoke();
        gameObject.SetActive(true);
        //�����ʱ�޷������뻥��
        player.canControl = false;
    }
}
