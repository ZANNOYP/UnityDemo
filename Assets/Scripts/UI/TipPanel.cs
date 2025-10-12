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

    //���
    public Player player;
    //ͨ�ص�
    public CheckPoint checkPoint;
    //��������
    private Door door;
    //������Կ��
    private Key k;

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
        player.actionPick += Pick;
    }
    // Start is called before the first frame update
    void Start()
    {
        btnReturn.onClick.AddListener(() =>
        {

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
        gameObject.SetActive(true);
    }
    /// <summary>
    /// ͨ��ί��
    /// </summary>
    public void Pass()
    {
        UpdateTip("��ϲ���� ��Ϸ����", "�������˵�");
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
        gameObject.SetActive(true);
    }
    /// <summary>
    /// ʰȡ��ʾί��
    /// </summary>
    /// <param name="k"></param>
    public void Pick(Key k)
    {
        this.k = k;
        UpdateTip("���Կ��", "ȷ��");
        //���ؽ������
        player.actionInteraction2?.Invoke();
        gameObject.SetActive(true);
        //�����ʱ�޷������뻥��
        player.canControl = false;
    }
}
