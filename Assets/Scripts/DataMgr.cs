using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr
{
    private static DataMgr instance = new DataMgr();
    public static DataMgr Instance => instance;
    private DataMgr() { }

    private int score;

    public int LoadScore()
    {
        return score = PlayerPrefs.GetInt("score");
    }

    public void SaveScore(int score)
    {
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.Save();
    }
}
