using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �������
/// </summary>
public class InteractionPanel : MonoBehaviour
{
    private static InteractionPanel instance;
    public static InteractionPanel Instance => instance;
    private InteractionPanel() { }

    //�����ı�
    public Text txtInteraction;
    //���
    public Player player;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //ע����ʾ����
        player.actionInteraction += Show;
        //ע�����ػ���
        player.actionInteraction2 += Hide;
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// ���»����ı���Ϣ
    /// </summary>
    /// <param name="text"></param>
    public void UpdateTxt(string text)
    {
        this.txtInteraction.text = text;
        
    }
    /// <summary>
    /// ��ʾ���
    /// </summary>
    /// <param name="text"></param>
    public void Show(string text)
    {
        UpdateTxt(text);
        this.gameObject.SetActive(true);
    }
    /// <summary>
    /// �������
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
