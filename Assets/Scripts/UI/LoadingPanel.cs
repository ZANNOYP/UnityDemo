using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : BasePanel
{
    public override void HideMe()
    {
        
    }

    public override void ShowMe()
    {
        //过场景清空音效、以及对象池
        MusicMgr.Instance.ClearSound();
        PoolMgr.Instance.ClearPool();
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
}
