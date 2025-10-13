using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

    //游戏是否结束
    public static bool isOver;
    //玩家
    public Player player;
    //怪物
    public Monster monster;
    //通关点
    public CheckPoint checkPoint;
    //暂停委托
    public UnityAction actionPause;

    private void Awake()
    {
        instance = this;
        //加载分数
        txtScore.text = DataMgr.Instance.score.ToString();
        //隐藏怪物血条
        sliderMonsterHp.gameObject.SetActive(false);
        //默认未通关
        isOver = false;
        //注册玩家回血
        player.actionAddHp += UpdatePlayerHp;
        //注册玩家受伤
        player.actionWound += UpdatePlayerHp;
        //注册玩家死亡
        player.actionDead += Pass;
        //注册怪物受伤
        monster.actionWound += UpdateMonsterHp;
        //注册怪物死亡
        monster.actionDead += UpdateScore;
        //注册通关
        checkPoint.actionPass += Pass;
    }

    // Start is called before the first frame update
    void Start()
    {
        //鼠标锁定
        Cursor.lockState = CursorLockMode.Locked;
        //改变背景音乐
        MusicMgr.Instance.audioSource.clip = Resources.Load<AudioClip>("Music/celestial-wanderer-i-391268");
        MusicMgr.Instance.audioSource.Play();
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
        //暂停面板
        if (Input.GetKeyDown(KeyCode.Escape) && player.canControl && !isOver) 
        {
            player.canControl = false;
            actionPause?.Invoke();
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;

        }
        //按F12清空存档
        //if (Input.GetKeyDown(KeyCode.T)) 
        //{
        //    PlayerPrefs.DeleteAll();
        //    PlayerPrefs.Save();
        //    Debug.Log("PlayerPrefs 已清空");
        //}
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
    /// 通关或失败
    /// </summary>
    public void Pass()
    {
        isOver = true;
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
