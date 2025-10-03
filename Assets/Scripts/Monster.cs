using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
/// <summary>
/// 怪物状态枚举
/// </summary>
public enum State
{
    Idle, 
    Patrol, 
    Chase, 
    Return,
    Atk,
    Dead
}

/// <summary>
/// 怪物类
/// </summary>
public class Monster : MonoBehaviour
{
    //巡逻点1
    public Transform pos1;
    //巡逻点2
    public Transform pos2;
    //玩家
    public GameObject player;
    //怪物血量
    [SerializeField]
    private int hp;
    private int maxHp = 3;

    //怪物移动方向
    [SerializeField]
    private Vector3 dir;
    //怪物移动速度
    private float speed = 3f;
    //怪物控制器
    private CharacterController controller;
    //待机时间
    private float time;
    //目标位置
    [SerializeField]
    private Vector3 targetCurrent;
    //怪物状态
    [SerializeField]
    private State state;
    //攻击间隔时间
    private const float cdTime = 2f;
    //攻击间隔计时
    private float cTime;
    //攻击范围
    private float atkRange = 0.8f;
    //动画状态机
    private Animator animator;
    //目标移动速度
    private float targetSpeed;
    //当前移动速度
    private float nowSpeed;
    //移动加速度
    private float changeSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        controller = GetComponent<CharacterController>();
        targetCurrent = pos1.position;
        animator = GetComponent<Animator>();
        hp = maxHp;
    }

    // Update is called once per frame
    void Update()
    {
        controller.Move(Vector3.down * Time.deltaTime);
        switch (state)
        {
            //待机
            case State.Idle:
                Idle();
                break;
            //巡逻
            case State.Patrol:
                Patrol();
                break;
            //索敌
            case State.Chase:
                Chase();
                break;
            //返回
            case State.Return:
                Return();
                break;
            //攻击
            case State.Atk:
                Atk();
                break;
            //攻击
            case State.Dead:
                Dead();
                break;
        }
    }

    /// <summary>
    /// 受伤
    /// </summary>
    public void Wound()
    {
        //血量-1
        hp--;
        //游戏界面更新血条
        GamePanel.Instance.UpdateMonsterHp(hp,maxHp);
        if (hp <= 0)
        {
            //死亡
            state = State.Dead;

        }
    }

    /// <summary>
    /// 死亡播放动画
    /// </summary>
    public void Dead()
    {
        animator.SetTrigger("isDead");
    }

    /// <summary>
    /// 死亡动画播放完毕等待1秒销毁自己
    /// </summary>
    public void DeadEvent()
    {
        //销毁自己
        Destroy(gameObject, 1f);
        //游戏界面更新分数
        GamePanel.Instance.UpdateScore();
        //生成血包
        Instantiate(Resources.Load<GameObject>("Heart"), transform.position + Vector3.up * 0.5f - transform.forward * 0.5f + transform.right, Quaternion.identity);
        Instantiate(Resources.Load<GameObject>("Key"), transform.position + Vector3.up * 0.5f- Vector3.forward * 0.5f - transform.right, Quaternion.identity);
    }


    /// <summary>
    /// 待机
    /// </summary>
    public void Idle()
    {
        //玩家在怪物索敌范围切换索敌状态
        if (Vector3.Angle(transform.forward, player.transform.position - transform.position) < 60f
                && Vector3.Distance(transform.position, player.transform.position) < 5f)
        {
            //待机时间设为0
            time = 0;
            
            state = State.Chase;
            return;
        }
        //待机计时
        time += Time.deltaTime;

        //速度变化平滑
        targetSpeed = 0;
        nowSpeed = Mathf.Lerp(nowSpeed, targetSpeed, Time.deltaTime * changeSpeed);
        if (Mathf.Abs(targetSpeed - nowSpeed) < 0.1f) nowSpeed = targetSpeed;
        //播放移动动画
        animator.SetFloat("Speed", nowSpeed);

        if (time >= 2f)
        {
            //待机结束，切换巡逻状态
            if (targetCurrent == pos1.position)
            {
                targetCurrent = pos2.position;
                dir = (targetCurrent - transform.position).normalized;
                transform.LookAt(targetCurrent);
                time = 0;
                state = State.Patrol;
                return;
            }
            if (targetCurrent == pos2.position)
            {
                targetCurrent = pos1.position;
                dir = (targetCurrent - transform.position).normalized;
                transform.LookAt(targetCurrent);
                time = 0;
                state = State.Patrol;
                return;
            }

        }
    }

    /// <summary>
    /// 巡逻
    /// </summary>
    public void Patrol()
    {
        //玩家在怪物索敌范围切换索敌状态
        if (Vector3.Angle(transform.forward, player.transform.position - transform.position) < 60f
                && Vector3.Distance(transform.position, player.transform.position) < 5f)
        {
            state = State.Chase;
            return;
        }
        //到达巡逻点切换待机状态
        if (Vector3.Distance(transform.position, targetCurrent) < 0.2f)
        {
            state = State.Idle;
            return;
        }
        
        //速度变化平滑
        targetSpeed = speed;
        nowSpeed = Mathf.Lerp(nowSpeed, targetSpeed, Time.deltaTime * changeSpeed);
        if (Mathf.Abs(targetSpeed - nowSpeed) < 0.1f) nowSpeed = targetSpeed;
        //播放移动动画
        animator.SetFloat("Speed", nowSpeed);
        //巡逻
        controller.Move(dir * nowSpeed * Time.deltaTime);

    }

    /// <summary>
    /// 索敌
    /// </summary>
    public void Chase()
    {
        //看向玩家
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        //将目标设为玩家
        targetCurrent = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        //移动方向
        dir = (targetCurrent - transform.position).normalized;

        //速度变化平滑
        targetSpeed = speed;
        nowSpeed = Mathf.Lerp(nowSpeed, targetSpeed, Time.deltaTime * changeSpeed);
        if (Mathf.Abs(targetSpeed - nowSpeed) < 0.1f) nowSpeed = targetSpeed;
        //播放移动动画
        animator.SetFloat("Speed", nowSpeed);

        //向玩家移动
        controller.Move(dir * nowSpeed * Time.deltaTime);
        //玩家远离索敌范围返回巡逻点，切换返回状态
        if (Vector3.Distance(transform.position, targetCurrent) > 10f)
        {
            state = State.Return;
            return;
        }
        //玩家进入怪物攻击范围，切换攻击状态
        if (Vector3.Distance(transform.position, targetCurrent) < atkRange)
        {
            state = State.Atk;
        }
    }

    /// <summary>
    /// 返回
    /// </summary>
    public void Return()
    {
        //得到最近的巡逻点位置
        targetCurrent = Vector3.Distance(transform.position, pos1.position) > Vector3.Distance(transform.position, pos2.position) ? pos2.position : pos1.position;
        //移动方向
        dir = (targetCurrent - transform.position).normalized;
        //看向目标巡逻点
        transform.LookAt(targetCurrent);
        //移动
        controller.Move(dir * speed * Time.deltaTime);
        //到达巡逻点切换待机状态
        if (Vector3.Distance(transform.position, targetCurrent) < 0.2f)
        {
            state = State.Idle;
        }
            
    }

    /// <summary>
    /// 攻击
    /// </summary>
    public void Atk()
    {
        //玩家脱离怪物攻击范围，切换索敌状态，恢复攻击冷却时间
        if (Vector3.Distance(transform.position, targetCurrent) > atkRange)
        {
            state = State.Chase;
            cTime = 0;
            return;
        }

        //攻击不在冷却时间，进行攻击
        if (cTime == 0)
        {
            animator.SetFloat("Speed", 0);
            animator.SetTrigger("isAtk");
        }
            
        //攻击冷却计时，时间一到切换索敌状态
        cTime += Time.deltaTime;
        if (cTime >= cdTime) 
        {
            cTime = 0;
            state = State.Chase;
        }
    }


    /// <summary>
    /// 攻击球形检测
    /// </summary>
    public void AtkEvent()
    {
        //攻击检测玩家
        if (Physics.SphereCast(transform.position + Vector3.up * 1.3f ,
                                0.1f,
                                transform.forward,
                                out RaycastHit hit,
                                0.8f,
                                1 << LayerMask.NameToLayer("Player"),
                                QueryTriggerInteraction.Ignore))
        {
            Player p = hit.collider.gameObject.GetComponent<Player>();
            //打到玩家，玩家掉血
            if (p != null)
            {
                p.Wound();
            }

        }
    }

    /// <summary>
    /// 辅助绘制
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + Vector3.up * 1.3f + transform.forward * 0.8f , 0.1f);
        Gizmos.DrawSphere(transform.position + Vector3.up * 1.3f , 0.1f);
    }
}
