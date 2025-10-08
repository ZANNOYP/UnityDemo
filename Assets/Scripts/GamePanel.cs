using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// ��Ϸ����
/// </summary>
public class GamePanel : MonoBehaviour
{
    private static GamePanel instance;
    public static GamePanel Instance => instance;
    private GamePanel() { }

    //����
    public Text txtScore;

    public Image imgPlayerHp;
    private float nowPlayerHpWidth;
    private float maxPlayerHpWidth = 500f;

    //����Ѫ��
    public Slider sliderMonsterHp;
    //����Ѫ��2������Э��
    private Coroutine hideCoroutine;
    //����λ��
    public Transform monsterPos;
    //Ѫ��λ��ƫ��
    public Vector3 offset;
    //��ǰ��Ļ�Ϲ���Ѫ��Ӧ��λ��
    [SerializeField]
    private Vector3 localPos;

    //��Ϸ�Ƿ���ͣ
    public static bool isPause = false;
    //��Ϸ�Ƿ����
    public static bool isOver;

    public Player player;
    public Monster monster;
    public CheckPoint checkPoint;

    private void Awake()
    {
        instance = this;
        txtScore.text = DataMgr.Instance.LoadScore().ToString();
        sliderMonsterHp.gameObject.SetActive(false);
        isOver = false;
        player.actionAddHp += UpdatePlayerHp;
        player.actionWound += UpdatePlayerHp;
        player.actionDead += Pass;
        monster.actionWound += UpdateMonsterHp;
        monster.actionDead += UpdateScore;
        checkPoint.actionPass += Pass;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        //����δ������Ѫ������
        if (monsterPos != null)
        {
            localPos = Camera.main.WorldToScreenPoint(monsterPos.position+ offset);
            localPos.x = localPos.x * 1920 / Screen.width;
            localPos.y = localPos.y * 1080 / Screen.height;

            (sliderMonsterHp.transform as RectTransform).localPosition = localPos;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !isPause && !isOver) 
        {
            isPause = true;
            PausePanel.Instance.gameObject.SetActive(true);
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;

        }
        
    }

    /// <summary>
    /// ���·���
    /// </summary>
    public void UpdateScore()
    {
        txtScore.text = (int.Parse(txtScore.text) + 1).ToString();
        DataMgr.Instance.SaveScore(int.Parse(txtScore.text));
    }

    /// <summary>
    /// �������Ѫ��
    /// </summary>
    /// <param name="hp">��ҵ�ǰѪ��</param>
    /// <param name="maxHp">������Ѫ��</param>
    public void UpdatePlayerHp(int hp, int maxHp)
    {
        nowPlayerHpWidth = hp * maxPlayerHpWidth / maxHp;
        imgPlayerHp.rectTransform.sizeDelta = new Vector2(nowPlayerHpWidth, imgPlayerHp.rectTransform.sizeDelta.y);
    }

    /// <summary>
    /// ���¹���Ѫ��
    /// </summary>
    /// <param name="hp">���ﵱǰѪ��</param>
    /// <param name="maxHp">�������Ѫ��</param>
    public void UpdateMonsterHp(int hp, int maxHp)
    {
        ShowMonsterHp();
        sliderMonsterHp.value = (float)hp / maxHp;
    }

    /// <summary>
    /// ͨ�ػ�ʧ��
    /// </summary>
    public void Pass()
    {
        isOver = true;
    }

    /// <summary>
    /// Э�̼�ʱ����������Ѫ����ʾ2��
    /// </summary>
    public void ShowMonsterHp()
    {
        sliderMonsterHp.gameObject.SetActive(true);
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideAfterDelay(2f));
    }

    IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        sliderMonsterHp.gameObject.SetActive(false);
    }
}
