using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
public enum State
{
    Idle, 
    Patrol, 
    Chase, 
    Return,
    Atk
}


public class Monster : MonoBehaviour
{
    public Transform pos1;
    public Transform pos2;
    public GameObject player;

    private int hp = 5;
    [SerializeField]
    private Vector3 dir;
    private float speed = 2f;
    private CharacterController controller;
    private float time;
    private Vector3 targetCurrent;
    private State state;
    private const float cdTime = 2f;
    private float cTime;
    private float atkRange = 2f;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        targetCurrent = pos1.position;
    }

    // Update is called once per frame
    void Update()
    {
        switch(state)
        {
            case State.Idle:
                Idle();
                break;
            case State.Patrol:
                Patrol();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Return:
                Return();
                break;
            case State.Atk:
                Atk();
                break;
        }
    }

    public void Wound()
    {
        hp--;
        if (hp <= 0)
        {
            Destroy(gameObject);
            GamePanel.Instance.UpdateScore();
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Idle()
    {
        if (Vector3.Angle(transform.forward, player.transform.position - transform.position) < 150f
                && Vector3.Distance(transform.position, player.transform.position) < 5f)
        {
            print("��������");
            time = 0;
            
            state = State.Chase;
            return;
        }
        print("��ʼ����");
        time += Time.deltaTime;
        if (time >= 2f)
        {
            print("��������");
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
        if (Vector3.Angle(transform.forward, player.transform.position - transform.position) < 150f
                && Vector3.Distance(transform.position, player.transform.position) < 5f)
        {
            print("Ѳ������");
            
            
            state = State.Chase;
            return;
        }
        if (Vector3.Distance(transform.position, targetCurrent) < 0.2f)
        {
            print("�����ж�");
            state = State.Idle;
            return;
        }
        

        print("Ѳ��");
        controller.Move(dir * speed * Time.deltaTime);
    }

    /// <summary>
    /// ����
    /// </summary>
    public void Chase()
    {
        transform.LookAt(player.transform.position + new Vector3(0, transform.position.y, 0));
        targetCurrent = player.transform.position;
        dir = (targetCurrent - transform.position).normalized;
        controller.Move(dir * speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, targetCurrent) > 10f)
        {
            print("��ȥ");
            state = State.Return;
            return;
        }
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
        targetCurrent = Vector3.Distance(transform.position, pos1.position) > Vector3.Distance(transform.position, pos2.position) ? pos2.position : pos1.position;
        dir = (targetCurrent - transform.position).normalized;
        transform.LookAt(targetCurrent);
        controller.Move(dir * speed * Time.deltaTime);
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
        if (Vector3.Distance(transform.position, targetCurrent) > atkRange)
        {
            state = State.Chase;
            cTime = 0;
            return;
        }
        if (cTime == 0) 
            print("����");
        cTime += Time.deltaTime;
        if (cTime >= cdTime) 
        {
            cTime = 0;
            state = State.Chase;
        }
    }

}
