using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ���ݹ�����
/// </summary>
public class DataMgr
{
    private static DataMgr instance = new DataMgr();
    public static DataMgr Instance => instance;
    private DataMgr() { }

    //����
    private int score;

    /// <summary>
    /// ��������
    /// </summary>
    /// <returns></returns>
    public int LoadScore()
    {
        return score = PlayerPrefs.GetInt("score");
    }

    /// <summary>
    /// �����浵
    /// </summary>
    /// <param name="score"></param>
    public void SaveScore(int score)
    {
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.Save();
    }
}
