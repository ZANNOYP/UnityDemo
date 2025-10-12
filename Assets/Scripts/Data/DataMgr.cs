using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 数据管理类
/// </summary>
public class DataMgr
{
    private static DataMgr instance = new DataMgr();
    public static DataMgr Instance => instance;
    private DataMgr() { }

    //分数
    private int score;

    /// <summary>
    /// 分数读档
    /// </summary>
    /// <returns></returns>
    public int LoadScore()
    {
        return score = PlayerPrefs.GetInt("score");
    }

    /// <summary>
    /// 分数存档
    /// </summary>
    /// <param name="score"></param>
    public void SaveScore(int score)
    {
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.Save();
    }
}
