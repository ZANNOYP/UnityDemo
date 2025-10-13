using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ��Ч����
/// </summary>
public class SoundMgr : MonoBehaviour
{
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        //��ʼ����Ч
        audioSource.mute = !SettingPanel.Instance.btnChooseSound.gameObject.activeSelf;
        audioSource.volume = SettingPanel.Instance.sliderSound.value;
        audioSource.Play();
        Destroy(gameObject, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
