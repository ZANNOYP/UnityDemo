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
    private DataMgr() 
    {
        score = PlayerPrefs.GetInt("score", 0);
        musicData = new MusicData();
        musicData.musicOpen = PlayerPrefs.GetInt("musicOpen", 1) == 1 ? true : false;
        musicData.musicVolume = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        musicData.soundOpen = PlayerPrefs.GetInt("soundOpen", 1) == 1 ? true : false;
        musicData.soundVolume = PlayerPrefs.GetFloat("soundVolume", 0.5f);
    }

    //����
    public int score;
    //����
    public MusicData musicData;


    /// <summary>
    /// �����浵
    /// </summary>
    /// <param name="score"></param>
    public void SaveScore(int score)
    {
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.Save();
    }

    public void SaveMusic(bool open, float volume)
    {
        PlayerPrefs.SetInt("musicOpen", open ? 1 : 0);
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }
    public void SaveSound(bool open, float volume)
    {
        PlayerPrefs.SetInt("soundOpen", open ? 1 : 0);
        PlayerPrefs.SetFloat("soundVolume", volume);
        PlayerPrefs.Save();
    }
}
