using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    //记录点动画协程
    private Coroutine dotCoroutine;

    public override void HideMe()
    {
        //停止点动画协程
        if (dotCoroutine != null)
        {
            StopCoroutine(dotCoroutine);
            dotCoroutine = null;
        }
    }

    public override void ShowMe()
    {
        //过场景清空音效、以及对象池
        MusicMgr.Instance.ClearSound();
        PoolMgr.Instance.ClearPool();
        //停止协程 然后开启点动画协程
        if (dotCoroutine != null)
        {
            StopCoroutine(dotCoroutine);
        }
        dotCoroutine = StartCoroutine(LoadingDots());
    }

    protected override void Awake()
    {
        base.Awake();
        //注册进度条变化委托
        EventCenter.Instance.AddEventListener<float>(E_EventType.E_SceneLoadChange, ChangeProgress);
    }

    /// <summary>
    /// 改变进度条
    /// </summary>
    /// <param name="progress"></param>
    private void ChangeProgress(float progress)
    {
        GetControl<Slider>("sliderProgress").value = progress;
    }

    private void OnDestroy()
    {
        //注销进度条变化委托
        EventCenter.Instance.RemoveEventListener<float>(E_EventType.E_SceneLoadChange, ChangeProgress);
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
    /// 加载中...点动画协程
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadingDots()
    {
        //点的数量
        int dotCount = 0;
        while (true)
        {
            //改变点的数量
            dotCount = (dotCount % 3) + 1;
            //改变文本
            GetControl<TextMeshProUGUI>("txtProgress").text = "加载中" + new string('.', dotCount);
            //等待0.25s
            yield return new WaitForSeconds(0.25f);
        }
    }
}
