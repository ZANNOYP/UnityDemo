using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 音量管理
/// </summary>
public class MusicMgr1 : MonoBehaviour
{
    private static MusicMgr1 instance;
    public static MusicMgr1 Instance => instance;
    private MusicMgr1() { }

    public AudioSource audioSource;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            // 已经有实例，销毁自己
            Destroy(gameObject); 
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        //初始化音量
        audioSource.mute = !DataMgr.Instance.musicData.musicOpen;
        audioSource.volume = DataMgr.Instance.musicData.musicVolume;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
