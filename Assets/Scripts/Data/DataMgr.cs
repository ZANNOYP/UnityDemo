using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 数据管理类
/// </summary>
public class DataMgr : BaseManager<DataMgr>
{
    
    private DataMgr() 
    {
        LoadMusic();

    }

    //分数
    public int score;
    //音乐
    public MusicData musicData;


    /// <summary>
    /// 分数存档
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

    public void LoadMusic()
    {
        musicData = PlayerPrefsDataMgr.Instance.LoadData(typeof(MusicData), "musicData") as MusicData;
        if (!musicData.musicOpen && !musicData.soundOpen && musicData.musicVolume == 0 && musicData.soundVolume == 0) 
        {
            musicData.Init(true, true, 0.3f, 0.3f);
        }
    }

    public void SaveMusic(MusicData data)
    {
        PlayerPrefsDataMgr.Instance.SaveData(data, "musicData");
    }

}
