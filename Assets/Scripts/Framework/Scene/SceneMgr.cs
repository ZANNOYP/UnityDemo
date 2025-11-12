using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/// <summary>
/// 场景切换管理器 主要用于切换场景
/// </summary>
public class SceneMgr : BaseManager<SceneMgr>
{
    private SceneMgr() { }

    //同步切换场景的方法
    public void LoadScene(string name, UnityAction callBack = null)
    {
        //切换场景
        SceneManager.LoadScene(name);
        //调用回调
        callBack?.Invoke();
        callBack = null;
    }

    //异步切换场景的方法
    public void LoadSceneAsyn(string name, UnityAction callBack = null)
    {
        MonoMgr.Instance.StartCoroutine(ReallyLoadSceneAsyn(name, callBack));
    }

    private IEnumerator ReallyLoadSceneAsyn(string name, UnityAction callBack)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(name);
        //场景加载完毕不直接进入
        ao.allowSceneActivation = false;
        //进度条
        float displayedProgress = 0f;
        //不停的在协同程序中每帧检测是否加载结束 如果加载结束并且进度条走完 就不会进这个循环每帧执行了
        while (!ao.isDone) 
        {
            // 真实进度最大 0.9
            float targetProgress = Mathf.Clamp01(ao.progress / 0.9f);

            // 平滑过渡显示
            displayedProgress = Mathf.MoveTowards(displayedProgress, targetProgress, Time.deltaTime);
            //可以在这里利用事件中心 每一帧将进度发送给想要得到的地方
            EventCenter.Instance.EventTrigger<float>(E_EventType.E_SceneLoadChange, displayedProgress);

            //加载一秒且进度为0.9 则将切换场景
            if (displayedProgress >= 1f && ao.progress >= 0.9f) 
            {
                ao.allowSceneActivation = true;
            }
            yield return 0;
        }
        //避免最后一帧直接结束了 没有同步1出去
        EventCenter.Instance.EventTrigger<float>(E_EventType.E_SceneLoadChange, 1);

        callBack?.Invoke();
        callBack = null;
    }
}
