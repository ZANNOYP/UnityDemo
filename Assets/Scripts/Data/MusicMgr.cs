using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��������
/// </summary>
public class MusicMgr : MonoBehaviour
{
    private static MusicMgr instance;
    public static MusicMgr Instance => instance;
    private MusicMgr() { }

    public AudioSource audioSource;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //��ʼ������
        audioSource.mute = !DataMgr.Instance.musicData.musicOpen;
        audioSource.volume = DataMgr.Instance.musicData.musicVolume;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
