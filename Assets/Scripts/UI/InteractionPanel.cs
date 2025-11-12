using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 交互面板
/// </summary>
public class InteractionPanel : BasePanel
{
    protected override void Awake()
    {
        base.Awake();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// 设置面板信息
    /// </summary>
    /// <param name="txtInteraction"></param>
    public void SetInfo(string txtInteraction)
    {
        GetControl<TextMeshProUGUI>("txtInteraction").text = txtInteraction;
    }

    public override void ShowMe()
    {
        
    }

    public override void HideMe()
    {
        
    }
}
