using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CheckPoint : MonoBehaviour
{
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
                UIMgr.Instance.ShowPanel<TipPanel>(E_UILayer.Middle, (panel) =>
                {
                    panel.SetInfo("恭喜过关", "返回主菜单", false, false);
                });
                
            }
        }
    }
}
