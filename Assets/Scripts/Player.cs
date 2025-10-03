using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
/// <summary>
/// ���״̬ö��
/// </summary>
public enum statePlayer
{
    Idle,//����
    Move,//�ƶ�
    Jump,//��Ծ
    Atk,//����
    OpenDoor,//����
}

/// <summary>
/// �����
/// </summary>
public class Player : MonoBehaviour
{
    //����ƶ��ٶ�
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    //���ת���ٶ�
    public float roundSpeed = 5f;
    //��Ҷ���
    public Animator animator;
    //��ҽŵ�λ��
    public Transform foot;
    //�����Ծ�߶�
    public float jumpHeight = 0.5f;
    //����㼶
    public LayerMask layerGround;
    //����������뾶
    public float checkSphereRadius = 0.1f;
    //��ҿ�����
    private CharacterController controller;
    //����ƶ�����
    private Vector3 move;
    //�����ת����
    private Quaternion targetRotation;
    //��ҵ�ǰ�ƶ��ٶ�
    [SerializeField]
    private float nowSpeed; 
    //���Ŀ���ƶ��ٶ�
    [SerializeField]
    private float targetSpeed;
    //��Ҽ��ٶ�
    private float changeSpeed = 5f;
    //����Ƿ��ڵ���״̬
    [SerializeField]
    private bool isGround;
    //���y�᷽���ٶ�
    [SerializeField]
    private float nowYspeed;
    //���״̬
    public statePlayer state;
    //���Ѫ��
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
            //����
            case statePlayer.Idle:
                Idle();
                break;
            //�ƶ�
            case statePlayer.Move:
                Move();
                break;
            //��Ծ
            case statePlayer.Jump:
                Jump();
                break;
            //����
            case statePlayer.Atk:
                Atk();
                break;
            //����
            case statePlayer.OpenDoor:
                OpenDoor(); 
                break;
        }

    }
    /// <summary>
    /// ����
    /// </summary>
    public void Idle()
    {
        //�õ��ƶ�����
        move = Quaternion.Euler(0, CameraController.yaw, 0) * new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        move = move.magnitude > 1 ? move.normalized : move;
        //Ŀ���ƶ��ٶ���Ϊ0�����ٶ�ƽ���仯
        targetSpeed = 0;
        nowSpeed = Mathf.Lerp(nowSpeed, targetSpeed, Time.deltaTime * changeSpeed);
        if (Mathf.Abs(targetSpeed - nowSpeed) < 0.1f) nowSpeed = targetSpeed;
        //y���ٶȸ�Ϊ-1��������
        nowYspeed = -1;
        controller.Move(Vector3.up * nowYspeed * Time.deltaTime);
        //E���л�����״̬
        if (Input.GetKeyDown(KeyCode.E))
        {
            state = statePlayer.OpenDoor;
            return;
        }
        //����л�����״̬
        if (Input.GetMouseButtonDown(0))
        {
            state = statePlayer.Atk;
            return;
        }
        //�ո���л���Ծ״̬
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = statePlayer.Jump;
        }
        //û�а���WASD���Ŵ�������
        if (move == Vector3.zero)
        {
            animator.SetFloat("Speed", nowSpeed);
        }
        //����WASD�л��ƶ�״̬
        else
        {
            state = statePlayer.Move;
        }
            
    }
    /// <summary>
    /// �ƶ�
    /// </summary>
    public void Move()
    {
        //�õ��ƶ�����
        move = Quaternion.Euler(0, CameraController.yaw, 0) * new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        move = move.magnitude > 1 ? move.normalized : move;
        //E���л�����״̬
        if (Input.GetKeyDown(KeyCode.E))
        {
            state = statePlayer.OpenDoor;
            return;
        }
        //����л�����״̬
        if (Input.GetMouseButtonDown(0))
        {
            state = statePlayer.Atk;
            return;
        }
        //�ո��л���Ծ״̬
        if (Input.GetKeyDown(KeyCode.Space))
        {
            state = statePlayer.Jump;
            return;
        }
        //û�а���WASD�л�����״̬
        if (move == Vector3.zero)
        {
            state = statePlayer.Idle;
            return;
        }
        //���ת���ƶ�����
        targetRotation = Quaternion.LookRotation(move);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, roundSpeed * Time.deltaTime);
        //shift���ı��ƶ��ٶ�
        targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;
        //�ı䵱ǰ�ƶ��ٶ�
        nowSpeed = Mathf.Lerp(nowSpeed, targetSpeed, Time.deltaTime * changeSpeed);
        if (Mathf.Abs(targetSpeed - nowSpeed) < 0.1f) nowSpeed = targetSpeed;
        //��ɫ�ƶ�
        controller.Move(move * nowSpeed * Time.deltaTime);
        //�����ƶ�����
        animator.SetFloat("Speed", nowSpeed);
        
    }

    /// <summary>
    /// ��Ծ
    /// </summary>
    public void Jump()
    {
        //�õ��ƶ�����
        move = Quaternion.Euler(0, CameraController.yaw, 0) * new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        move = move.magnitude > 1 ? move.normalized : move;
        //�õ����ﴥ��״̬
        isGround = CheckGround();
        //���ﴥ����y���ٶ�Ϊ-1����y���ٶ���Ϊ�趨��Ծ�߶ȵõ��������ٶ�
        if (isGround && nowYspeed == -1)  
        {
            nowYspeed = Mathf.Sqrt(2 * 10 * jumpHeight);
        }
        //�ڿ���ʱ����������Ӱ�죬��������
        if (!isGround)
        {
            nowYspeed -= 10 * Time.deltaTime;
        }
        //�ڵ�����y���ٶ�С��0����y���ٶ���Ϊ-1��������
        else if (nowYspeed < 0)
        {
            nowYspeed = -1f;
            //��Ծ��ɣ�û��WASD����������״̬
            if (move == Vector3.zero)
            {
                state = statePlayer.Idle;
            }
            //��WASD��������ƶ�״̬
            else
            {
                state = statePlayer.Move;
            }
        }
        //������WASD������Գ��ƶ������ƶ�
        if (move != Vector3.zero)
        {
            targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, roundSpeed * Time.deltaTime);
            targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

            nowSpeed = Mathf.Lerp(nowSpeed, targetSpeed, Time.deltaTime * changeSpeed);
            if (Mathf.Abs(targetSpeed - nowSpeed) < 0.1f) nowSpeed = targetSpeed;
        }
        
        //������Ծ���ƶ�����
        controller.Move((Vector3.up * nowYspeed + move * nowSpeed) * Time.deltaTime);
        //������Ծ����
        animator.SetFloat("ySpeed", nowYspeed);
        animator.SetBool("IsGround", isGround);
    }

    /// <summary>
    /// ������ﴥ��״̬
    /// </summary>
    /// <returns></returns>
    public bool CheckGround()
    {
        return Physics.CheckSphere(foot.position, checkSphereRadius, layerGround, QueryTriggerInteraction.Ignore);
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Atk()
    {
        //����״̬�¹����㼶Ȩ��Ϊ0ʱ����Ȩ�ظ�Ϊ1�����Ź���������ֹͣ�����ƶ�����
        if (animator.GetLayerWeight(1) == 0)
        {
            animator.SetLayerWeight(1, 1);
            animator.SetTrigger("isAtk");
            animator.SetFloat("Speed", 0);
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public void OpenDoor()
    {
        //������ǰ���߼��
        Ray ray = new Ray(transform.position + Vector3.up, transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, 1 << LayerMask.NameToLayer("Door"), QueryTriggerInteraction.Ignore))
        {
            Door d = hit.collider.gameObject.GetComponent<Door>();
            print(d.gameObject.name);
            //����õ��Žű�����û�������
            if (d != null && !d.isOpen && !d.isLock) 
                d.OpenDoor();
        }
        //���һ�����ﷵ�ش���״̬
        state = statePlayer.Idle;
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Wound()
    {
        //Ѫ��-1
        hp--;
        //��Ϸ�������Ѫ��
        GamePanel.Instance.UpdatePlayerHp(hp, maxHp);
        if (hp <= 0)
        {
            //Application.Quit();
            //UnityEditor.EditorApplication.isPlaying = false;
            TipPanel.Instance.UpdateTip("�������");
            TipPanel.Instance.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// ��Ѫ
    /// </summary>
    public void AddHp()
    {
        hp = maxHp;
        GamePanel.Instance.UpdatePlayerHp(hp, maxHp);
        print("��һ�Ѫ");
    }

    /// <summary>
    /// ��������
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(foot.position, checkSphereRadius);
        Gizmos.DrawSphere(transform.position + Vector3.up * 1.3f + transform.forward * 0.8f + transform.right * 0.5f, 0.1f);
        Gizmos.DrawSphere(transform.position + Vector3.up * 1.3f + transform.right * 0.5f, 0.1f);
    }
}
