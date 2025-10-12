using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 互动面板
/// </summary>
public class InteractionPanel : MonoBehaviour
{
    private static InteractionPanel instance;
    public static InteractionPanel Instance => instance;
    private InteractionPanel() { }

    //互动文本
    public Text txtInteraction;
    //玩家
    public Player player;
    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //注册显示互动
        player.actionInteraction += Show;
        //注册隐藏互动
        player.actionInteraction2 += Hide;
        this.gameObject.SetActive(false);
    }
    /// <summary>
    /// 更新互动文本信息
    /// </summary>
    /// <param name="text"></param>
    public void UpdateTxt(string text)
    {
        this.txtInteraction.text = text;
        
    }
    /// <summary>
    /// 显示面板
    /// </summary>
    /// <param name="text"></param>
    public void Show(string text)
    {
        UpdateTxt(text);
        this.gameObject.SetActive(true);
    }
    /// <summary>
    /// 隐藏面板
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
