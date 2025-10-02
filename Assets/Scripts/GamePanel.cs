using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 游戏界面
/// </summary>
public class GamePanel : MonoBehaviour
{
    private static GamePanel instance;
    public static GamePanel Instance => instance;
    private GamePanel() { }

    //分数
    public Text txtScore;

    public Image imgPlayerHp;
    private float nowPlayerHpWidth;
    private float maxPlayerHpWidth = 500f;

    //怪物血条
    public Slider sliderMonsterHp;
    //怪物血条2秒隐藏协程
    private Coroutine hideCoroutine;
    //怪物位置
    public Transform monsterPos;
    //血条位置偏移
    public Vector3 offset;
    //当前屏幕上怪物血条应处位置
    [SerializeField]
    private Vector3 localPos;

    

    private void Awake()
    {
        instance = this;
        txtScore.text = DataMgr.Instance.LoadScore().ToString();
        sliderMonsterHp.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //怪物未死亡，血条跟随
        if (monsterPos != null)
        {
            localPos = Camera.main.WorldToScreenPoint(monsterPos.position+ offset);
            localPos.x = localPos.x * 1920 / Screen.width;
            localPos.y = localPos.y * 1080 / Screen.height;

            (sliderMonsterHp.transform as RectTransform).localPosition = localPos;
        }
        
    }

    /// <summary>
    /// 更新分数
    /// </summary>
    public void UpdateScore()
    {
        txtScore.text = (int.Parse(txtScore.text) + 1).ToString();
        DataMgr.Instance.SaveScore(int.Parse(txtScore.text));
    }

    /// <summary>
    /// 更新玩家血条
    /// </summary>
    /// <param name="hp">玩家当前血量</param>
    /// <param name="maxHp">玩家最大血量</param>
    public void UpdatePlayerHp(int hp, int maxHp)
    {
        nowPlayerHpWidth = hp * maxPlayerHpWidth / maxHp;
        imgPlayerHp.rectTransform.sizeDelta = new Vector2(nowPlayerHpWidth, imgPlayerHp.rectTransform.sizeDelta.y);
    }

    /// <summary>
    /// 更新怪物血条
    /// </summary>
    /// <param name="hp">怪物当前血量</param>
    /// <param name="maxHp">怪物最大血量</param>
    public void UpdateMonsterHp(int hp, int maxHp)
    {
        ShowMonsterHp();
        sliderMonsterHp.value = (float)hp / maxHp;
    }

    /// <summary>
    /// 协程计时，攻击怪物血条显示2秒
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
