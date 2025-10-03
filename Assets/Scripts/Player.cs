using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
/// <summary>
/// 玩家状态枚举
/// </summary>
public enum statePlayer
{
    Idle,//待机
    Move,//移动
    Jump,//跳跃
    Atk,//攻击
    OpenDoor,//开门
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
    public float roundSpeed = 5f;
    //玩家动画
    public Animator animator;
    //玩家脚底位置
    public Transform foot;
    //玩家跳跃高度
    public float jumpHeight = 0.5f;
    //地面层级
    public LayerMask layerGround;
    //地面检测球体半径
    public float checkSphereRadius = 0.1f;
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

    private int maxHp = 5;

    public Door[] doors;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        state = statePlayer.Idle;
        hp = maxHp;
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
            //开门
            case statePlayer.OpenDoor:
                OpenDoor(); 
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
        //E键切换开门状态
        if (Input.GetKeyDown(KeyCode.E))
        {
            state = statePlayer.OpenDoor;
            return;
        }
        //左键切换攻击状态
        if (Input.GetMouseButtonDown(0))
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
        //E键切换开门状态
        if (Input.GetKeyDown(KeyCode.E))
        {
            state = statePlayer.OpenDoor;
            return;
        }
        //左键切换攻击状态
        if (Input.GetMouseButtonDown(0))
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
        //角色移动
        controller.Move(move * nowSpeed * Time.deltaTime);
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
        //攻击状态下攻击层级权重为0时，将权重改为1，播放攻击动画，停止播放移动动画
        if (animator.GetLayerWeight(1) == 0)
        {
            animator.SetLayerWeight(1, 1);
            animator.SetTrigger("isAtk");
            animator.SetFloat("Speed", 0);
        }
    }

    /// <summary>
    /// 开门
    /// </summary>
    public void OpenDoor()
    {
        //人物面前射线检测
        Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, 1 << LayerMask.NameToLayer("Door"), QueryTriggerInteraction.Ignore))
        {
            Door d = hit.collider.gameObject.GetComponent<Door>();
            print(d.gameObject.name);
            //如果得到门脚本且门没开则打开门
            if (d != null && !d.isOpen && !d.isLock) 
                d.OpenDoor();
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
        GamePanel.Instance.UpdatePlayerHp(hp, maxHp);
        if (hp <= 0)
        {
            //Application.Quit();
            //UnityEditor.EditorApplication.isPlaying = false;
            TipPanel.Instance.UpdateTip("玩家死亡");
            TipPanel.Instance.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// 回血
    /// </summary>
    public void AddHp()
    {
        hp = maxHp;
        GamePanel.Instance.UpdatePlayerHp(hp, maxHp);
        print("玩家回血");
    }

    /// <summary>
    /// 辅助绘制
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(foot.position, checkSphereRadius);
        Gizmos.DrawSphere(transform.position + Vector3.up * 1.3f + transform.forward * 0.8f + transform.right * 0.5f, 0.1f);
        Gizmos.DrawSphere(transform.position + Vector3.up * 1.3f + transform.right * 0.5f, 0.1f);
    }
}
