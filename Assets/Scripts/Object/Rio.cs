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
    /// 攻击球形检测
    /// </summary>
    public void AtkEvent1()
    {
        Collider[] colliders;
        switch (player.atkType)
        {
            case AtkType.Riot:
                //攻击检测怪物
                colliders = Physics.OverlapSphere(player.transform.position + Vector3.up * 1.1f + player.transform.forward * 0.7f + player.transform.right * 0.2f, 0.4f, 1 << LayerMask.NameToLayer("Monster"), QueryTriggerInteraction.Collide);
                foreach (Collider collider in colliders)
                {
                    Monster m = collider.gameObject.GetComponent<Monster>();
                    //打到怪物，怪物掉血
                    if (m != null)
                    {
                        MusicMgr.Instance.PlaySound("Hit");
                        m.Wound();
                    }

                }
                break;
            case AtkType.ShortSword:
                colliders = Physics.OverlapSphere(player.transform.position + Vector3.up * 1.25f + player.transform.forward * 1f, 0.5f, 1 << LayerMask.NameToLayer("Monster"), QueryTriggerInteraction.Collide);
                if (colliders.Length <= 0) 
                    MusicMgr.Instance.PlaySound("Sword");
                foreach (Collider collider in colliders)
                {
                    
                    //得到怪物脚本
                    Monster m = collider.gameObject.GetComponent<Monster>();
                    //打到怪物，怪物掉血
                    if (m != null)
                    {
                        MusicMgr.Instance.PlaySound("SwordBody");
                        m.Wound();
                    }
                }
                break;
            default:
                break;
        }
        
    }

    /// <summary>
    /// 攻击球形检测
    /// </summary>
    public void AtkEvent2()
    {
        Collider[] colliders;
        switch (player.atkType)
        {
            case AtkType.Riot:
                //攻击检测怪物
                colliders = Physics.OverlapSphere(player.transform.position + Vector3.up * 1.1f + player.transform.forward * 0.7f + player.transform.right * 0.2f, 0.4f, 1 << LayerMask.NameToLayer("Monster"), QueryTriggerInteraction.Collide);
                foreach (Collider collider in colliders)
                {
                    Monster m = collider.gameObject.GetComponent<Monster>();
                    //打到怪物，怪物掉血
                    if (m != null)
                    {
                        MusicMgr.Instance.PlaySound("Hit");
                        m.Wound();
                    }

                }
                break;
            case AtkType.ShortSword:
                colliders = Physics.OverlapSphere(player.transform.position + Vector3.up * 1.25f + player.transform.forward * 1f, 0.5f, 1 << LayerMask.NameToLayer("Monster"), QueryTriggerInteraction.Collide);
                if (colliders.Length <= 0)
                    MusicMgr.Instance.PlaySound("Shield");
                foreach (Collider collider in colliders)
                {
                    //得到怪物脚本
                    Monster m = collider.gameObject.GetComponent<Monster>();
                    //打到怪物，怪物掉血
                    if (m != null)
                    {
                        MusicMgr.Instance.PlaySound("Shield");
                        m.Wound();
                    }
                }
                break;
            default:
                break;
        }

    }

    /// <summary>
    /// 是否进行第二段连击判定
    /// </summary>
    public void ComboCheck()
    {
        //连击输入则进行第二段连击
        if (player.nextCombo)
        {
            player.StartCombo2();
        }

    }
    /// <summary>
    /// 第一段连击结束
    /// </summary>
    public void AtkOver1()
    {
        //恢复为未攻击状态
        player.isAtk1 = false;
        //恢复动画
        player.animator.SetBool("isAtk1", player.isAtk1);
        //玩家状态转为待机
        player.state = statePlayer.Idle;
        
    }

    /// <summary>
    /// 第二段连击结束
    /// </summary>
    public void AtkOver2()
    {
        //恢复为未攻击状态
        player.isAtk1 = false;
        player.isAtk2 = false;
        player.nextCombo = false;
        //恢复动画
        player.animator.SetBool("isAtk1", player.isAtk1);
        player.animator.SetBool("isAtk2", player.isAtk2);
        //玩家状态转为待机
        player.state = statePlayer.Idle;
        
    }
    /// <summary>
    /// 脚步音效
    /// </summary>
    public void FootStepEvent()
    {
        MusicMgr.Instance.PlaySound("FootStep");
    }
}
