using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckPoint : MonoBehaviour
{
    //通关委托
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
        //角色触发通关点，结束游戏
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Player p = other.gameObject.GetComponent<Player>();
            if (p != null)
            {
                //通关
                actionPass?.Invoke();
                //时间暂停
                Time.timeScale = 0;
                //鼠标解锁
                Cursor.lockState = CursorLockMode.None;
                
            }
        }
    }
}
