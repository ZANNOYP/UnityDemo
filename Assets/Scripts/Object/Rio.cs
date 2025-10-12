using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rio : MonoBehaviour
{
    //玩家控制脚本
    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 攻击结束
    /// </summary>
    public void AtkOver()
    {
        //玩家状态转为待机，层级权重改为0
        player.state = statePlayer.Idle;
        player.animator.SetLayerWeight(1, 0);
    }

    /// <summary>
    /// 攻击球形检测
    /// </summary>
    public void AtkEvent()
    {
        //攻击检测怪物
        if (Physics.SphereCast(player.gameObject.transform.position + Vector3.up * 1.3f + player.gameObject.transform.right * 0.5f, 
                                0.1f, 
                                player.gameObject.transform.forward, 
                                out RaycastHit hit, 
                                0.8f, 
                                1 << LayerMask.NameToLayer("Monster"), 
                                QueryTriggerInteraction.Ignore)) 
        {
            Monster m = hit.collider.gameObject.GetComponent<Monster>();
            //打到怪物，怪物掉血
            if (m != null)
            {
                m.Wound();
                print("攻击成功");
            }
                
        }
    }
}
