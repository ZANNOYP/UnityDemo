using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckPoint : MonoBehaviour
{
    //ͨ��ί��
    public UnityAction actionPass;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //��ɫ����ͨ�ص㣬������Ϸ
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player p = other.gameObject.GetComponent<Player>();
            if (p != null)
            {
                //ͨ��
                actionPass?.Invoke();
                //ʱ����ͣ
                Time.timeScale = 0;
                //������
                Cursor.lockState = CursorLockMode.None;
                
            }
        }
    }
}
