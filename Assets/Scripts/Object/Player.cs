using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.Video;
/// <summary>
/// 玩家状态枚举
/// </summary>
public enum statePlayer
{
    Idle,//待机
    Move,//移动
    Jump,//跳跃
    Atk,//攻击
    Interaction,//交互
}
/// <summary>
/// 玩家攻击类型
/// </summary>
public enum AtkType
{
    Riot,//拳击
    ShortSword,//短剑
}


/// <summary>
/// 玩家类
/// </summary>
public class Player : MonoBehaviour
{
    //玩家移动速度
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    //玩家转动速度
    public float roundSpeed = 10f;
    //玩家动画
    public Animator animator;
    //玩家脚底位置
    public Transform foot;
    //玩家跳跃高度
    public float jumpHeight = 2f;
    //地面层级
    public LayerMask layerGround;
    //地面检测球体半径
    public float checkSphereRadius = 0.17f;
    //玩家控制器
    private CharacterController controller;
    //玩家移动方向
    private Vector3 move;
    //玩家旋转方向
    private Quaternion targetRotation;
    //玩家当前移动速度
    [SerializeField]
    private float nowSpeed; 
    //玩家目标移动速度
    [SerializeField]
    private float targetSpeed;
    //玩家加速度
    private float changeSpeed = 5f;
    //玩家是否在地面状态
    [SerializeField]
    private bool isGround;
    //玩家y轴方向速度
    [SerializeField]
    private float nowYspeed;
    //玩家状态
    public statePlayer state;
    //玩家血量
    [SerializeField]
    private int hp;
    //玩家最大血量
    private int maxHp = 5;
    //门
    public Door[] doors;

    //角色能否攻击、互动，防止关闭面板时角色自动攻击一下
    public bool canControl;
    //角色正在攻击
    public bool isAtk1;
    public bool isAtk2;
    //角色攻击类型
    public AtkType atkType;
    //角色上一次攻击类型
    //private AtkType frontAtkType;
    //脚步音效
    public AudioSource footstep;
    //能否连击
    public bool nextCombo;

    public int atk;

    public bool openDoor;

    public Door door;

    // Start is called before the first frame update
    void Start()
    {
        //初始化攻击类型为拳击
        atkType = AtkType.Riot;
        //frontAtkType = atkType;
        //默认不在攻击
        isAtk1 = false;
        isAtk2 = false;
        //角色默认可攻击与互动
        canControl = true;
        //角色控制器
        controller = GetComponent<CharacterController>();
        //默认状态为待机
        state = statePlayer.Idle;
        //出生满血
        hp = maxHp;

        openDoor = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            //待机
            case statePlayer.Idle:
                Idle();
                break;
            //移动
            case statePlayer.Move:
                Move();
                break;
            //跳跃
            case statePlayer.Jump:
                Jump();
                break;
            //攻击
            case statePlayer.Atk:
                Atk();
                break;
            //交互
            case statePlayer.Interaction:
                Interaction(); 
                break;
        }

    }
    /// <summary>
    /// 待机
    /// </summary>
    public void Idle()
    {
        //得到移动方向
        move = Quaternion.Euler(0, CameraController.yaw, 0) * new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        move = move.magnitude > 1 ? move.normalized : move;
        //目标移动速度设为0，让速度平滑变化
        targetSpeed = 0;
        nowSpeed = Mathf.Lerp(nowSpeed, targetSpeed, Time.deltaTime * changeSpeed);
        if (Mathf.Abs(targetSpeed - nowSpeed) < 0.1f) nowSpeed = targetSpeed;
        //y轴速度改为-1贴紧地面
        nowYspeed = -1;
        controller.Move(Vector3.up * nowYspeed * Time.deltaTime);
        //E键切换交互状态
        if (canControl && Input.GetKeyDown(KeyCode.E)) 
        {
            state = statePlayer.Interaction;
            return;
        }
        //左键切换攻击状态
        if (canControl && Input.GetMouseButtonDown(0)) 
        {
            state = statePlayer.Atk;
            return;
        }
        //空格键切换跳跃状态
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = statePlayer.Jump;
        }
        //没有按下WASD播放待机动画
        if (move == Vector3.zero)
        {
            animator.SetFloat("Speed", nowSpeed);
        }
        //按下WASD切换移动状态
        else
        {
            state = statePlayer.Move;
        }
            
    }
    /// <summary>
    /// 移动
    /// </summary>
    public void Move()
    {
        //得到移动方向
        move = Quaternion.Euler(0, CameraController.yaw, 0) * new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        move = move.magnitude > 1 ? move.normalized : move;
        //E键切换交互状态
        if (canControl && Input.GetKeyDown(KeyCode.E))
        {
            state = statePlayer.Interaction;
            return;
        }
        //左键切换攻击状态
        if (canControl && Input.GetMouseButtonDown(0)) 
        {
            state = statePlayer.Atk;
            return;
        }
        //空格切换跳跃状态
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = statePlayer.Jump;
            return;
        }
        //没有按下WASD切换待机状态
        if (move == Vector3.zero)
        {
            state = statePlayer.Idle;
            return;
        }
        //玩家转向移动方向
        targetRotation = Quaternion.LookRotation(move);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, roundSpeed * Time.deltaTime);
        //shift键改变移动速度
        targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        //改变当前移动速度
        nowSpeed = Mathf.Lerp(nowSpeed, targetSpeed, Time.deltaTime * changeSpeed);
        if (Mathf.Abs(targetSpeed - nowSpeed) < 0.1f) nowSpeed = targetSpeed;
        //保证移动时角色贴地
        nowYspeed = -1;
        //角色移动
        controller.Move((Vector3.up * nowYspeed + move * nowSpeed) * Time.deltaTime);
        //播放移动动画
        animator.SetFloat("Speed", nowSpeed);
        
    }

    /// <summary>
    /// 跳跃
    /// </summary>
    public void Jump()
    {
        //得到移动方向
        move = Quaternion.Euler(0, CameraController.yaw, 0) * new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        move = move.magnitude > 1 ? move.normalized : move;
        //得到人物触地状态
        isGround = CheckGround();
        //人物触地且y轴速度为-1，将y轴速度设为设定跳跃高度得到的起跳速度
        if (isGround && nowYspeed == -1)  
        {
            nowYspeed = Mathf.Sqrt(2 * 10 * jumpHeight);
        }
        //在空中时人物受重力影响，慢慢减速
        if (!isGround)
        {
            nowYspeed -= 10 * Time.deltaTime;
        }
        //在地面且y轴速度小于0，将y轴速度设为-1紧贴地面
        else if (nowYspeed < 0)
        {
            nowYspeed = -1f;
            //跳跃完成，没有WASD输入进入待机状态
            if (move == Vector3.zero)
            {
                state = statePlayer.Idle;
            }
            //有WASD输入进入移动状态
            else
            {
                state = statePlayer.Move;
            }
        }
        //空中有WASD输入可以朝移动方向移动
        if (move != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, roundSpeed * Time.deltaTime);
            targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

            nowSpeed = Mathf.Lerp(nowSpeed, targetSpeed, Time.deltaTime * changeSpeed);
            if (Mathf.Abs(targetSpeed - nowSpeed) < 0.1f) nowSpeed = targetSpeed;
        }
        
        //人物跳跃、移动叠加
        controller.Move((Vector3.up * nowYspeed + move * nowSpeed) * Time.deltaTime);
        //播放跳跃动画
        animator.SetFloat("ySpeed", nowYspeed);
        animator.SetBool("IsGround", isGround);
    }

    /// <summary>
    /// 检查人物触地状态
    /// </summary>
    /// <returns></returns>
    public bool CheckGround()
    {
        return Physics.CheckSphere(foot.position, checkSphereRadius, layerGround, QueryTriggerInteraction.Ignore);
    }

    /// <summary>
    /// 攻击
    /// </summary>
    public void Atk()
    {
        //未处于攻击状态时进行攻击
        if (!isAtk1)
        {
            StartCombo1();
            return;
        }
        //攻击状态时检测攻击输入
        if (isAtk1 && !isAtk2 && Input.GetMouseButtonDown(0)) 
        {
            //记录连击输入
            nextCombo = true;
        }
    }

    /// <summary>
    /// 第一连击
    /// </summary>
    public void StartCombo1()
    {
        //变为正在第一段连击状态
        isAtk1 = true;
        //玩家切换武器改变攻击类型则改变攻击动画
        //if (frontAtkType != atkType)
        //{
        //    switch (atkType)
        //    {
        //        case AtkType.Riot:
        //            //拳击层
        //            animator.SetLayerWeight(1, 0);
        //            break;
        //        case AtkType.ShortSword:
        //            //短剑层
        //            animator.SetLayerWeight(1, 1);
        //            break;
        //        default:
        //            break;
        //    }
        //    frontAtkType = atkType;
        //}
        //切换动画，水平速度设为0
        animator.SetBool("isAtk1", isAtk1);
        animator.SetFloat("Speed", 0);
        
    }

    /// <summary>
    /// 第二连击
    /// </summary>
    public void StartCombo2()
    {
        //变为正在第二段连击状态
        isAtk2 = true;
        //if (frontAtkType != atkType)
        //{
        //    switch (atkType)
        //    {
        //        case AtkType.Riot:
        //            //拳击层
        //            animator.SetLayerWeight(1, 0);
        //            break;
        //        case AtkType.ShortSword:
        //            //短剑层
        //            animator.SetLayerWeight(1, 1);
        //            break;
        //        default:
        //            break;
        //    }
        //    frontAtkType = atkType;
        //}
        //切换动画，水平速度设为0
        animator.SetBool("isAtk2", isAtk2);
        animator.SetFloat("Speed", 0);

    }

    /// <summary>
    /// 交互
    /// </summary>
    public void Interaction()
    {
        //开门相关
        Vector3 position = transform.position + Vector3.up * 0.5f + transform.forward * 0.5f;
        float sphereRadium = 0.4f;
        LayerMask mask = (1 << LayerMask.NameToLayer("Door")) |
                         (1 << LayerMask.NameToLayer("Key")) |
                         (1 << LayerMask.NameToLayer("Weapon")) |
                         (1 << LayerMask.NameToLayer("Hp"));
        //得到门对象
        Collider[] colliders = Physics.OverlapSphere(position, sphereRadium, mask, QueryTriggerInteraction.Collide);
        foreach (Collider collider in colliders)
        {
            BaseItem item;
            //得到门脚本
            Door d = collider.gameObject.GetComponent<Door>();
            if (d != null) 
            {
                UIMgr.Instance.GetPanel<GamePanel>((panel) =>
                {
                    panel.PauseStart(true);
                });
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                canControl = false;
                UIMgr.Instance.ShowPanel<BagPanel>();
            }

            //得到钥匙脚本
            Key k = collider.gameObject.GetComponent<Key>();
            if (k != null)
            {
                k.PickUp();
            }

            //得到武器脚本
            Weapon w = collider.gameObject.GetComponent<Weapon>();
            if (w != null)
            {
                w.PickUp();
            }

            //得到血包脚本
            Hp h = collider.gameObject.GetComponent<Hp>();
            if (h != null)
            {
                h.PickUp();
            }
        }
        
        //检测一次人物返回待机状态
        state = statePlayer.Idle;
    }

    /// <summary>
    /// 受伤
    /// </summary>
    public void Wound()
    {
        //血量-1
        hp--;
        //游戏界面更新血条
        EventCenter.Instance.EventTrigger<float>(E_EventType.E_Player_HpChange, (float)hp / maxHp);
        //actionWound?.Invoke(hp, maxHp);
        if (hp <= 0)
        {
            Dead();
        }
    }

    /// <summary>
    /// 死亡
    /// </summary>
    public void Dead()
    {
        InputMgr.Instance.StartOrCloseInputMgr(false);
        UIMgr.Instance.ShowPanel<TipPanel>(E_UILayer.Middle, (panel) =>
        {
            panel.GetControl<TextMeshProUGUI>("txtTip").text = "玩家死亡 游戏结束";
            panel.GetControl<TextMeshProUGUI>("txtBtn").text = "返回主菜单";
            panel.GetControl<Button>("btnSetting").gameObject.SetActive(false);
            panel.GetControl<Button>("btnBag").gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// 回血
    /// </summary>
    public void AddHp(int addHp)
    {
        hp += addHp;
        if (hp > maxHp)
        {
            hp = maxHp;
        }
        //血条更新
        EventCenter.Instance.EventTrigger<float>(E_EventType.E_Player_HpChange, (float)hp / maxHp);
    }

    /// <summary>
    /// 加攻击力
    /// </summary>
    /// <param name="atk"></param>
    public void AddAtk(int atk)
    {
        this.atk += atk;
    }

    /// <summary>
    /// 互动提示开启
    /// </summary>
    /// <param name="other">互动物体触发器</param>
    private void OnTriggerEnter(Collider other)
    {
        Vector3 position = transform.position + Vector3.up * 0.5f + transform.forward * 0.5f;
        float sphereRadium = 0.4f;
        LayerMask mask = (1 << LayerMask.NameToLayer("Door")) |
                         (1 << LayerMask.NameToLayer("Key")) |
                         (1 << LayerMask.NameToLayer("Weapon")) |
                         (1 << LayerMask.NameToLayer("Hp"));
        Collider[] colliders = Physics.OverlapSphere(position, sphereRadium, mask, QueryTriggerInteraction.Collide);
        foreach (Collider collider in colliders)
        {
            //得到门脚本
            Door d = collider.gameObject.GetComponent<Door>();
            if (d != null && !d.isOpen) 
            {
                UIMgr.Instance.ShowPanel<InteractionPanel>(E_UILayer.Bottom, (panel) =>
                {
                    panel.GetControl<TextMeshProUGUI>("txtInteraction").text = "E开门";
                });
                openDoor = true;
                door = d;
            }

            //得到钥匙脚本
            Key k = collider.gameObject.GetComponent<Key>();
            //得到武器脚本
            Weapon w = collider.gameObject.GetComponent<Weapon>();
            //得到血包脚本
            Hp h = collider.gameObject.GetComponent<Hp>();
            if (k != null || w != null || h != null) 
            {
                UIMgr.Instance.ShowPanel<InteractionPanel>(E_UILayer.Bottom, (panel) =>
                {
                    panel.GetControl<TextMeshProUGUI>("txtInteraction").text = "E拾取";
                });
            }

            //VideoPlayer v = collider.gameObject.GetComponent<VideoPlayer>();
            //if (v != null) 
            //    v.Play();
        }
    }

    /// <summary>
    /// 互动提示关闭
    /// </summary>
    /// <param name="other">互动物体触发器</param>
    private void OnTriggerExit(Collider other)
    {
        //触发器离开检测
        if (other.gameObject.layer == LayerMask.NameToLayer("Door") ||
            other.gameObject.layer == LayerMask.NameToLayer("Key") ||
            other.gameObject.layer == LayerMask.NameToLayer("Weapon") ||
            other.gameObject.layer == LayerMask.NameToLayer("Hp")) 
        {
            UIMgr.Instance.HidePanel<InteractionPanel>();
            openDoor = false;
            door = null;
        }
    }

    /// <summary>
    /// 辅助绘制
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawSphere(foot.position, checkSphereRadius);
        //Gizmos.DrawSphere(transform.position + Vector3.up * 1.1f + transform.forward * 0.7f + transform.right * 0.2f, 0.4f);
        Gizmos.DrawLine(transform.position + Vector3.up * 0.5f, transform.position + Vector3.up * 0.5f + transform.forward * 0.5f);
        //Gizmos.DrawSphere(transform.position + Vector3.up * 1.25f + transform.forward * 1f, 0.5f);
        Gizmos.DrawSphere(transform.position + Vector3.up * 0.5f + transform.forward * 0.5f, 0.4f);

    }
}
