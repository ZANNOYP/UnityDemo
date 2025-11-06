using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR;
/// <summary>
/// 怪物状态枚举
/// </summary>
public enum State
{
    Idle,//待机 
    Patrol, //巡逻
    Chase, //索敌
    Atk,//攻击
    Dead,//死亡
}

/// <summary>
/// 怪物类
/// </summary>
public class Monster : MonoBehaviour
{
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
    //受伤委托
    public UnityAction<int, int> actionWound;
    //死亡委托
    public UnityAction actionDead;
    //绑定的门
    public Door door;
    //玩家相对怪物的方向
    public Vector3 toPlayer;
    //巡逻点
    public Transform[] patrolPoints;
    public int patrolIndex = 0;
    //待机时间
    public float idleDuration = 2f;
    //转动速度
    public float roundSpeed = 100f;
    //索敌范围
    public float detectionRange = 5f;
    //追击范围
    public float chaseRange = 7f;
    //玩家相对怪物距离的平方
    public float sqrtDist;
    //巡逻点切换状态的范围
    public float patrolPointRange = 0.3f;
    //去往玩家消失点
    public bool goingToLastPos = false;

    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        state = State.Idle;
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        hp = maxHp;

        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = true; // 必须为 true
        agent.updateRotation = true; // 自动朝向移动方向
        
    }

    // Update is called once per frame
    void Update()
    {
        //controller.Move(Vector3.down * Time.deltaTime);
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
            //追击
            case State.Chase:
                Chase();
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
        EventCenter.Instance.EventTrigger<float>(E_EventType.E_Monster_HpChange, (float)hp / maxHp);
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
        //生成血包
        Instantiate(Resources.Load<GameObject>("Hp"), transform.position + Vector3.up * 0.5f - transform.forward * 0.5f + transform.right, Quaternion.identity);
        //生成钥匙
        Key k = Instantiate(Resources.Load<GameObject>("Key"), transform.position + Vector3.up * 0.5f - Vector3.forward * 0.5f - transform.right, Quaternion.identity).GetComponent<Key>();
        //将钥匙与门绑定
        //k.door = this.door;
    }


    /// <summary>
    /// 待机
    /// </summary>
    public void Idle()
    {
        toPlayer = (player.transform.position - transform.position).normalized;
        sqrtDist = (player.transform.position - transform.position).sqrMagnitude;
        //玩家在怪物索敌范围切换索敌状态
        if (Vector3.Dot(transform.forward, toPlayer) > Mathf.Cos(90f * Mathf.Deg2Rad)
                && sqrtDist < detectionRange * detectionRange) 
        {
            //待机时间设为0
            time = 0;
            
            state = State.Chase;
            return;
        }
        //待机计时
        time += Time.deltaTime;

        //播放移动动画
        animator.SetFloat("Speed", 0f);

        if (time >= 2f)
        {
            if (goingToLastPos)
            {
                targetCurrent = patrolPoints[patrolIndex].position;
            }
            else
            {
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
                targetCurrent = patrolPoints[patrolIndex].position;
            }
            agent.isStopped = false;
            agent.SetDestination(targetCurrent);
            //待机结束，切换巡逻状态
            //dir = (targetCurrent - transform.position).normalized;
            // 检测前方是否有障碍
            //if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, out RaycastHit hit, 1f))
            //{
            //    // 两种可能的滑动方向
            //    Vector3 slideA = Vector3.Cross(hit.normal, Vector3.up);
            //    Vector3 slideB = Vector3.Cross(Vector3.up, hit.normal);

            //    // 选择更接近玩家方向的那个
            //    dir = (Vector3.Dot(slideA, dir) > Vector3.Dot(slideB, dir)) ? slideA : slideB;
            //}
            //Quaternion targetRot = Quaternion.LookRotation(dir);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * roundSpeed);
            time = 0;
            state = State.Patrol;
            return;
        }
    }

    /// <summary>
    /// 巡逻
    /// </summary>
    public void Patrol()
    {
        toPlayer = (player.transform.position - transform.position).normalized;
        sqrtDist = (player.transform.position - transform.position).sqrMagnitude;
        //玩家在怪物索敌范围切换索敌状态
        if (Vector3.Dot(transform.forward, toPlayer) > Mathf.Cos(90f * Mathf.Deg2Rad)
                && sqrtDist < detectionRange * detectionRange)
        {
            state = State.Chase;
            return;
        }
        //到达巡逻点切换待机状态
        if ((targetCurrent - transform.position).sqrMagnitude < patrolPointRange * patrolPointRange) 
        {
            agent.isStopped = true;
            if (goingToLastPos)
            {
                for (int i = 0; i < patrolPoints.Length; i++)
                {
                    if (targetCurrent == patrolPoints[i].position)
                    {
                        goingToLastPos = false;
                    }
                }
            }
            state = State.Idle;
            return;
        }

        //速度变化平滑
        //targetSpeed = speed;
        //agent.speed = Mathf.Lerp(agent.speed, targetSpeed, Time.deltaTime * changeSpeed);
        //if (Mathf.Abs(targetSpeed - agent.speed) < 0.1f) agent.speed = targetSpeed;
        ////播放移动动画
        animator.SetFloat("Speed", agent.velocity.magnitude);
        //Quaternion targetRot = Quaternion.LookRotation(dir);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * roundSpeed);

        //巡逻
        //controller.Move(dir * nowSpeed * Time.deltaTime);

    }

    /// <summary>
    /// 追击
    /// </summary>
    public void Chase()
    {
        Vector3 playerPos = player.transform.position;
        //将目标设为玩家
        targetCurrent = new Vector3(playerPos.x, transform.position.y, playerPos.z);
        //移动方向
        dir = (targetCurrent - transform.position).normalized;
        agent.isStopped = false;
        agent.SetDestination(targetCurrent);
        // 检测前方是否有障碍
        //if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, out RaycastHit hit, 1f))
        //{
        //    // 两种可能的滑动方向
        //    Vector3 slideA = Vector3.Cross(hit.normal, Vector3.up);
        //    Vector3 slideB = Vector3.Cross(Vector3.up, hit.normal);

        //    // 选择更接近玩家方向的那个
        //    dir = (Vector3.Dot(slideA, dir) > Vector3.Dot(slideB, dir)) ? slideA : slideB;
        //}
            
        //看向玩家
        //Quaternion targetRot = Quaternion.LookRotation(dir);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * roundSpeed);
        //速度变化平滑
        //targetSpeed = speed;
        //agent.speed = Mathf.Lerp(agent.speed, targetSpeed, Time.deltaTime * changeSpeed);
        //if (Mathf.Abs(targetSpeed - agent.speed) < 0.1f) agent.speed = targetSpeed;
        ////播放移动动画
        animator.SetFloat("Speed", agent.velocity.magnitude);

        //向玩家移动
        //controller.Move(transform.forward * nowSpeed * Time.deltaTime);
        //玩家远离追击范围返回巡逻点，切换返回状态
        if ((targetCurrent - transform.position).sqrMagnitude > chaseRange * chaseRange)
        {
            //targetCurrent = playerPos;
            goingToLastPos = true;
            //dir = (targetCurrent - transform.position).normalized;
            //Quaternion target = Quaternion.LookRotation(dir);
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * roundSpeed);
            state = State.Patrol;
            return;
        }
        //玩家进入怪物攻击范围，切换攻击状态
        if ((targetCurrent - transform.position).sqrMagnitude < atkRange * atkRange)
        {
            state = State.Atk;
        }
    }

    /// <summary>
    /// 攻击
    /// </summary>
    public void Atk()
    {
        agent.isStopped = true;
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
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.up * 1.1f + transform.forward * 0.7f + transform.right * 0.2f, 0.4f, 1 << LayerMask.NameToLayer("Player"), QueryTriggerInteraction.Collide);
        foreach (Collider collider in colliders)
        {
            Player p = collider.gameObject.GetComponent<Player>();
            //打到玩家，玩家掉血
            if (p != null)
            {
                MusicMgr.Instance.PlaySound("Hit");
                p.Wound();
            }
        }
    }
    /// <summary>
    /// 脚步音效
    /// </summary>
    public void FootStepEvent()
    {
        //MusicMgr.Instance.PlaySound("FootStep");
    }
    /// <summary>
    /// 辅助绘制
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + Vector3.up * 1.1f + transform.forward * 0.7f + transform.right * 0.2f, 0.4f);
    }
}
