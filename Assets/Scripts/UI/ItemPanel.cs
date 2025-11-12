using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 物品面板
/// </summary>
public class ItemPanel : BasePanel
{
    //物品拾取信息位置
    private Vector3 itemPos;
    //物品拾取信息位置的x坐标
    private float x;
    //面板透明度
    private CanvasGroup canvasGroup;
    //显示面板标识
    private bool isShow;
    //4s后隐藏协程
    private Coroutine hideCoroutine;
    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //每帧改变物品信息位置 形成滑入效果
        x = Mathf.Lerp(x, 260, Time.deltaTime * 5);
        //x = Mathf.MoveTowards(x, 260, Time.deltaTime * 700);
        itemPos.x = x;
        GetControl<Image>("Item").rectTransform.localPosition = itemPos;

        //显示面板和隐藏面板时 慢慢改变面板的透明度
        if (isShow)
        {
            canvasGroup.alpha += Time.deltaTime;
            if (canvasGroup.alpha >= 1) 
                canvasGroup.alpha = 1;
        }
        else
        {
            canvasGroup.alpha -= Time.deltaTime;
            if (canvasGroup.alpha <= 0)
                canvasGroup.alpha = 0;
        }
    }
    /// <summary>
    /// 设置面板信息
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="itemName"></param>
    /// <param name="count"></param>
    public void SetInfo(Sprite sprite, string itemName, int count)
    {
        //设置图片
        GetControl<Image>("imgItem").sprite = sprite;
        //设置名字
        GetControl<TextMeshProUGUI>("txtItemName").text = itemName;
        //设置数量
        GetControl<TextMeshProUGUI>("txtItemNum").text = "x" + count;
        //每次设置面板信息即为拾取物品 此时清空隐藏协程
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);
        //开启隐藏协程
        hideCoroutine = StartCoroutine(HideDelay(4f));
    }

    private IEnumerator HideDelay(float sec)
    {
        yield return new WaitForSeconds(sec);
        UIMgr.Instance.HidePanel<ItemPanel>();
    }

    public override void HideMe()
    {
        //改变透明度 显示标识
        canvasGroup.alpha = 1;
        isShow = false;
    }

    public override void ShowMe()
    {
        //初始化x
        x = 780;
        //初始化ui位置
        itemPos = new Vector3(x, GetControl<Image>("Item").rectTransform.localPosition.y, GetControl<Image>("Item").rectTransform.localPosition.z);
        GetControl<Image>("Item").rectTransform.localPosition = itemPos;
        //改变透明度 显示标识
        canvasGroup.alpha = 0;
        isShow = true;
    }
}
