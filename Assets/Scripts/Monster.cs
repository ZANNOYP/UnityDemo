using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
/// <summary>
/// ����״̬ö��
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
/// ������
/// </summary>
public class Monster : MonoBehaviour
{
    //Ѳ�ߵ�1
    public Transform pos1;
    //Ѳ�ߵ�2
    public Transform pos2;
    //���
    public GameObject player;
    //����Ѫ��
    [SerializeField]
    private int hp;
    private int maxHp = 3;

    //�����ƶ�����
    [SerializeField]
    private Vector3 dir;
    //�����ƶ��ٶ�
    private float speed = 3f;
    //���������
    private CharacterController controller;
    //����ʱ��
    private float time;
    //Ŀ��λ��
    [SerializeField]
    private Vector3 targetCurrent;
    //����״̬
    [SerializeField]
    private State state;
    //�������ʱ��
    private const float cdTime = 2f;
    //���������ʱ
    private float cTime;
    //������Χ
    private float atkRange = 0.8f;
    //����״̬��
    private Animator animator;
    //Ŀ���ƶ��ٶ�
    private float targetSpeed;
    //��ǰ�ƶ��ٶ�
    private float nowSpeed;
    //�ƶ����ٶ�
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
            //����
            case State.Idle:
                Idle();
                break;
            //Ѳ��
            case State.Patrol:
                Patrol();
                break;
            //����
            case State.Chase:
                Chase();
                break;
            //����
            case State.Return:
                Return();
                break;
            //����
            case State.Atk:
                Atk();
                break;
            //����
            case State.Dead:
                Dead();
                break;
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Wound()
    {
        //Ѫ��-1
        hp--;
        //��Ϸ�������Ѫ��
        GamePanel.Instance.UpdateMonsterHp(hp,maxHp);
        if (hp <= 0)
        {
            //����
            state = State.Dead;

        }
    }

    /// <summary>
    /// �������Ŷ���
    /// </summary>
    public void Dead()
    {
        animator.SetTrigger("isDead");
    }

    /// <summary>
    /// ��������������ϵȴ�1�������Լ�
    /// </summary>
    public void DeadEvent()
    {
        //�����Լ�
        Destroy(gameObject, 1f);
        //��Ϸ������·���
        GamePanel.Instance.UpdateScore();
        //����Ѫ��
        Instantiate(Resources.Load<GameObject>("Heart"), transform.position + Vector3.up * 0.5f - transform.forward * 0.5f + transform.right, Quaternion.identity);
        Instantiate(Resources.Load<GameObject>("Key"), transform.position + Vector3.up * 0.5f- Vector3.forward * 0.5f - transform.right, Quaternion.identity);
    }


    /// <summary>
    /// ����
    /// </summary>
    public void Idle()
    {
        //����ڹ������з�Χ�л�����״̬
        if (Vector3.Angle(transform.forward, player.transform.position - transform.position) < 60f
                && Vector3.Distance(transform.position, player.transform.position) < 5f)
        {
            //����ʱ����Ϊ0
            time = 0;
            
            state = State.Chase;
            return;
        }
        //������ʱ
        time += Time.deltaTime;

        //�ٶȱ仯ƽ��
        targetSpeed = 0;
        nowSpeed = Mathf.Lerp(nowSpeed, targetSpeed, Time.deltaTime * changeSpeed);
        if (Mathf.Abs(targetSpeed - nowSpeed) < 0.1f) nowSpeed = targetSpeed;
        //�����ƶ�����
        animator.SetFloat("Speed", nowSpeed);

        if (time >= 2f)
        {
            //�����������л�Ѳ��״̬
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
    /// Ѳ��
    /// </summary>
    public void Patrol()
    {
        //����ڹ������з�Χ�л�����״̬
        if (Vector3.Angle(transform.forward, player.transform.position - transform.position) < 60f
                && Vector3.Distance(transform.position, player.transform.position) < 5f)
        {
            state = State.Chase;
            return;
        }
        //����Ѳ�ߵ��л�����״̬
        if (Vector3.Distance(transform.position, targetCurrent) < 0.2f)
        {
            state = State.Idle;
            return;
        }
        
        //�ٶȱ仯ƽ��
        targetSpeed = speed;
        nowSpeed = Mathf.Lerp(nowSpeed, targetSpeed, Time.deltaTime * changeSpeed);
        if (Mathf.Abs(targetSpeed - nowSpeed) < 0.1f) nowSpeed = targetSpeed;
        //�����ƶ�����
        animator.SetFloat("Speed", nowSpeed);
        //Ѳ��
        controller.Move(dir * nowSpeed * Time.deltaTime);

    }

    /// <summary>
    /// ����
    /// </summary>
    public void Chase()
    {
        //�������
        transform.LookAt(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z));
        //��Ŀ����Ϊ���
        targetCurrent = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        //�ƶ�����
        dir = (targetCurrent - transform.position).normalized;

        //�ٶȱ仯ƽ��
        targetSpeed = speed;
        nowSpeed = Mathf.Lerp(nowSpeed, targetSpeed, Time.deltaTime * changeSpeed);
        if (Mathf.Abs(targetSpeed - nowSpeed) < 0.1f) nowSpeed = targetSpeed;
        //�����ƶ�����
        animator.SetFloat("Speed", nowSpeed);

        //������ƶ�
        controller.Move(dir * nowSpeed * Time.deltaTime);
        //���Զ�����з�Χ����Ѳ�ߵ㣬�л�����״̬
        if (Vector3.Distance(transform.position, targetCurrent) > 10f)
        {
            state = State.Return;
            return;
        }
        //��ҽ�����﹥����Χ���л�����״̬
        if (Vector3.Distance(transform.position, targetCurrent) < atkRange)
        {
            state = State.Atk;
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Return()
    {
        //�õ������Ѳ�ߵ�λ��
        targetCurrent = Vector3.Distance(transform.position, pos1.position) > Vector3.Distance(transform.position, pos2.position) ? pos2.position : pos1.position;
        //�ƶ�����
        dir = (targetCurrent - transform.position).normalized;
        //����Ŀ��Ѳ�ߵ�
        transform.LookAt(targetCurrent);
        //�ƶ�
        controller.Move(dir * speed * Time.deltaTime);
        //����Ѳ�ߵ��л�����״̬
        if (Vector3.Distance(transform.position, targetCurrent) < 0.2f)
        {
            state = State.Idle;
        }
            
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Atk()
    {
        //���������﹥����Χ���л�����״̬���ָ�������ȴʱ��
        if (Vector3.Distance(transform.position, targetCurrent) > atkRange)
        {
            state = State.Chase;
            cTime = 0;
            return;
        }

        //����������ȴʱ�䣬���й���
        if (cTime == 0)
        {
            animator.SetFloat("Speed", 0);
            animator.SetTrigger("isAtk");
        }
            
        //������ȴ��ʱ��ʱ��һ���л�����״̬
        cTime += Time.deltaTime;
        if (cTime >= cdTime) 
        {
            cTime = 0;
            state = State.Chase;
        }
    }


    /// <summary>
    /// �������μ��
    /// </summary>
    public void AtkEvent()
    {
        //����������
        if (Physics.SphereCast(transform.position + Vector3.up * 1.3f ,
                                0.1f,
                                transform.forward,
                                out RaycastHit hit,
                                0.8f,
                                1 << LayerMask.NameToLayer("Player"),
                                QueryTriggerInteraction.Ignore))
        {
            Player p = hit.collider.gameObject.GetComponent<Player>();
            //����ң���ҵ�Ѫ
            if (p != null)
            {
                p.Wound();
            }

        }
    }

    /// <summary>
    /// ��������
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position + Vector3.up * 1.3f + transform.forward * 0.8f , 0.1f);
        Gizmos.DrawSphere(transform.position + Vector3.up * 1.3f , 0.1f);
    }
}
