using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// ���״̬ö��
/// </summary>
public enum statePlayer
{
    Idle,//����
    Move,//�ƶ�
    Jump,//��Ծ
    Atk,//����
    Interaction,//����
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
    //������Ѫ��
    private int maxHp = 5;
    //��
    public Door[] doors;
    //��Ѫί��
    public UnityAction<int, int> actionAddHp;
    //����ί��
    public UnityAction<int, int> actionWound;
    //����ί��
    public UnityAction actionDead;
    //������ʾί��
    public UnityAction<string> actionInteraction;
    //��������ί��
    public UnityAction actionInteraction2;
    //������ʾί��
    public UnityAction<Door> actionOpendoorTip;
    //ʰȡ��ʾί��
    public UnityAction<Key> actionPick;

    //�����ı���Ϣ
    public string interaction;
    //��ɫ�ܷ񹥻�����������ֹ�ر����ʱ��ɫ�Զ�����һ��
    public bool canControl;

    // Start is called before the first frame update
    void Start()
    {
        //��ɫĬ�Ͽɹ����뻥��
        canControl = true;
        //��ɫ������
        controller = GetComponent<CharacterController>();
        //Ĭ��״̬Ϊ����
        state = statePlayer.Idle;
        //������Ѫ
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
            case statePlayer.Interaction:
                Interaction(); 
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
        if (canControl && Input.GetKeyDown(KeyCode.E)) 
        {
            state = statePlayer.Interaction;
            return;
        }
        //����л�����״̬
        if (canControl && Input.GetMouseButtonDown(0)) 
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
        if (canControl && Input.GetKeyDown(KeyCode.E))
        {
            state = statePlayer.Interaction;
            return;
        }
        //����л�����״̬
        if (canControl && Input.GetMouseButtonDown(0)) 
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
        //��֤�ƶ�ʱ��ɫ����
        nowYspeed = -1;
        //��ɫ�ƶ�
        controller.Move((Vector3.up * nowYspeed + move * nowSpeed) * Time.deltaTime);
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
    public void Interaction()
    {
        //������ǰ���߼��
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, transform.forward * 0.5f);
        //�������
        if (Physics.Raycast(ray, out RaycastHit hit, 2f, 1 << LayerMask.NameToLayer("Door"), QueryTriggerInteraction.Ignore))
        {
            //�õ��Žű�
            Door d = hit.collider.gameObject.GetComponent<Door>();
            //�򿪿�����ʾ���
            actionOpendoorTip?.Invoke(d);
            //ʱ����ͣ
            Time.timeScale = 0;
            //������
            Cursor.lockState = CursorLockMode.None;
        }
        //ʰȡԿ�����
        //�õ�Կ�׶���
        Collider[] colliders = Physics.OverlapSphere(transform.position + Vector3.up * 0.5f + transform.forward * 0.5f, 0.5f, 1 << LayerMask.NameToLayer("Key"), QueryTriggerInteraction.Collide);
        foreach (Collider collider in colliders)
        {
            //�õ�Կ�׽ű�
            Key k = collider.gameObject.GetComponent<Key>();
            //��ʰȡ��ʾ���
            actionPick?.Invoke(k);
            //ʱ����ͣ
            Time.timeScale = 0;
            //������
            Cursor.lockState = CursorLockMode.None;
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
        actionWound?.Invoke(hp, maxHp);
        //hpС��0��ɫ����
        if (hp <= 0)
        {
            Dead();
            
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Dead()
    {
        //ʱ����ͣ
        Time.timeScale = 0;
        //������
        Cursor.lockState = CursorLockMode.None;
        //�����������
        actionDead?.Invoke();
    }

    /// <summary>
    /// ��Ѫ
    /// </summary>
    public void AddHp()
    {
        hp = maxHp;
        //Ѫ������
        actionAddHp?.Invoke(hp, maxHp);
    }

    private void OnTriggerEnter(Collider other)
    {
        //��������������
        Door door = other.gameObject.GetComponent<Door>();
        if (other.gameObject.layer == LayerMask.NameToLayer("Door") && !door.isOpen) 
        {
            //������ʾ��Ϣ
            interaction = "E����";
            //���������ʾ
            actionInteraction?.Invoke(interaction);
        }
        //������������Կ��
        if (other.gameObject.layer == LayerMask.NameToLayer("Key"))
        {
            //������ʾ��Ϣ
            interaction = "Eʰȡ";
            //���������ʾ
            actionInteraction?.Invoke(interaction);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //�������뿪�����
        if (other.gameObject.layer == LayerMask.NameToLayer("Door"))
        {
            //�����������
            actionInteraction2?.Invoke();
        }
        //�������뿪���Կ��
        if (other.gameObject.layer == LayerMask.NameToLayer("Key"))
        {
            //�����������
            actionInteraction2?.Invoke();
        }
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
        Gizmos.DrawLine(transform.position + Vector3.up * 0.5f, transform.position + Vector3.up * 0.5f + transform.forward * 0.5f);
    }
}
