using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TipPanel : MonoBehaviour
{
    private static TipPanel instance;
    public static TipPanel Instance => instance;
    private TipPanel() { }

    public Text txtTip;
    public Button btnReturn;

    public Player player;
    public CheckPoint checkPoint;

    private void Awake()
    {
        instance = this;
        player.actionDead += PlayerDead;
        checkPoint.actionPass += Pass;
    }
    // Start is called before the first frame update
    void Start()
    {
        btnReturn.onClick.AddListener(() =>
        {
            this.gameObject.SetActive(false);
            SceneManager.LoadScene("BeginScene");
        });
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTip(string text)
    {
        txtTip.text = text + " ��Ϸ����";
    }

    public void PlayerDead()
    {
        UpdateTip("�������");
        gameObject.SetActive(true);
    }

    public void Pass()
    {
        UpdateTip("��ϲ����");
        gameObject.SetActive(true);
    }
}
